using System;
using System.Collections;
using System.Collections.Generic;

using System.Net.Sockets;
using System.Net;

using Framework;

namespace KClientNetLib
{
    // Connect返回值罗列
    public enum KConnectRet
    {
        KCALLBACK_SUCCESS = 0,          // 成功
        KCALLBACK_CONNECTFAILED = 1,    // 网络开启异常，请查看设置
        KCALLBACK_TIMEOUT,              // 连接超时，请重试
        KCALLBACK_REFUSED,              // 服务器没有开启
    }

    public enum KErrorCode
    {
        KERROR_SUCCESS = 0,             // 成功
        KERROR_SEND_FAILED = 1,         // 发送失败
        KERROR_SEND_STATE,              // 发送失败
        KERROR_SEND_CALLBACKFAILED,     // 发送回调失败
        KERROR_RECV_FAILED,             // 接收失败
        KERROR_RECV_CALLBACKFAILED,     // 接收回调失败
        KERROR_DISCONNECT_BYREMOTE ,    // 远端断开连接，可能是网络也可能是服务器
        KERROR_DISCONNECT_BYCLIENT,     // 客户端主动断开连接
        KERROR_RECV_ID_ERROR,           // 接收数据错误
    }

    abstract public class KClientNet
    {
        /*
         * 实现客户端网络库
         * 异步接收
         * 主线程进行接收消息处理
         * 实现符合服务器登录LoginAgent、LGW和GW的流程
         */
        private KClientNetState m_iState;
        public KClientNetState State
        {
            get{return m_iState;}
            protected set { m_iState = value;}
        }

        private Socket m_Sock = null;
        private byte[] m_recvBuf = new byte[m_iRecvBufLen];
        private int m_iRecvedLen = 0;                                           // 需要在连接成功时进行清空
        private const int m_iRecvBufLen = 64 * 1024;
        private const int m_iMessageHead = 4;                                   // 包头长度4
        private LinkedList<KMessage> m_lMessageWait;

        public enum KClientNetState
        {
            KClientNet_None = 0,        // 未创建Socket
            KClientNet_Connecting,      // 连接过程中
            KClientNet_Connected,       // 连接成功
            KClientNet_Logined,         // 登录成功
            KClientNet_ConnectedFailed, // 连接失败
            KClientNet_Disconnected,    // 断开连接
        }

        public KClientNet()
        {
            m_iState = KClientNetState.KClientNet_None;
            m_lMessageWait = new LinkedList<KMessage>();
        }

        public void Reset()
        {
            m_iRecvedLen = 0;
            m_iState = KClientNetState.KClientNet_None;
            m_lMessageWait.Clear();

            // m_Sock.Connected表示的是最近一次接收或发送时的连接状态，并不表示当前连接状态
            if (null != m_Sock && m_Sock.Connected)
            {
                bool bSocketClosed = false;
                try
                {
                    m_Sock.Shutdown(SocketShutdown.Both);                    
                }
                catch (SocketException e)
                {
                    LogManager.LogDebug("KClientNet::Reset", String.Format("{0}:{1} ErrorCode={2}", e.ToString(), e.Message, e.ErrorCode));
                }
                catch (ObjectDisposedException e)
                {
                    bSocketClosed = true;
                    LogManager.LogDebug("KClientNet::Reset", String.Format("success to close socket : {0}", e.Message));
                }
                catch
                {
                    LogManager.LogError("KClientNet::Reset", "unknown error");
                }

                try
                {
                    if (!bSocketClosed)
                        m_Sock.Close();
                }
                catch
                {
                    LogManager.LogError("KClientNet::Reset", "socket close error");
                }
                m_Sock = null;
            }
        }
        
        abstract public void OnConnectEnd(KConnectRet iRet);
        abstract public void OnError(KErrorCode iRet, int iWsaRet);

        // 登录相关
        public int Login(string strIP, int iPort)
        {
            // 重置状态，确保socket已断开
            Reset();

            if (m_iState != KClientNetState.KClientNet_None)
            {
                // 状态错误，无法进行连接
                return 1;
            }

            if (null == m_Sock)
            {
                m_Sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }

            // 开始异步连接
            m_iState = KClientNetState.KClientNet_Connecting;

            try
            {
                m_iRecvedLen = 0;
                AsyncCallback callBack = new AsyncCallback(OnConnectCallback);
                IPAddress ip = IPAddress.Parse(strIP);
                IPEndPoint iep = new IPEndPoint(ip, iPort);
                m_Sock.BeginConnect(iep, callBack, this);	
            }
            catch
            {
                LogManager.LogError("KClientNet", "KClientNet::Login error");
            }

            return 0;
        }

        static void OnConnectCallback(IAsyncResult ar)
        {
            KClientNet clientNet = (KClientNet)ar.AsyncState;
            string strRes = "connected";
            KMessageOnConnectCallback msg = new KMessageOnConnectCallback();
            try
            {
                msg.m_iRez = KConnectRet.KCALLBACK_SUCCESS;
                clientNet.m_Sock.EndConnect(ar);
            }
            catch (SocketException e)
            {
                strRes = String.Format("{0}:{1} ErrorCode={2}", e.ToString(), e.Message, e.ErrorCode);
                LogManager.LogError("KClientNet", strRes);
                switch(e.ErrorCode)
                {
                    case 10060: // 连接超时
                        msg.m_iRez = KConnectRet.KCALLBACK_TIMEOUT;
                        break;
                    case 10051: // 目标不可达(本地网络没有目标网络路由)
                        msg.m_iRez = KConnectRet.KCALLBACK_CONNECTFAILED;
                        break;
                    case 10061: // 目标拒绝连接，一般是端口没有开启,端口错误
                        msg.m_iRez = KConnectRet.KCALLBACK_REFUSED;
                        break;
                    default:    // 连接异常
                        msg.m_iRez = KConnectRet.KCALLBACK_CONNECTFAILED;
                        break;
                }               
            }
            catch
            {
                strRes = String.Format("error");
                LogManager.LogError("KClientNet", strRes);
                msg.m_iRez = KConnectRet.KCALLBACK_CONNECTFAILED;
            }

            clientNet.PushMessage(msg);
        }

        public void Send(byte[] pb, UInt16 iMsgId)
        {
            UInt16 iLen = (UInt16)(pb.Length + 4);

            // buff组成：iLen + iMsgId + pb
            byte[] sendBuff = new byte[iLen];
            System.BitConverter.GetBytes(iLen).CopyTo(sendBuff, 0);
            System.BitConverter.GetBytes(iMsgId).CopyTo(sendBuff, 2);
            pb.CopyTo(sendBuff, 4);

            Send(sendBuff);
        }

        void Send(byte[] sendBuff)
        {
            if(!m_Sock.Connected
                || (m_iState != KClientNetState.KClientNet_Connected && m_iState != KClientNetState.KClientNet_Logined))
            {
                KMessageOnError msg = new KMessageOnError();
                msg.m_iRez = KErrorCode.KERROR_SEND_STATE;
                PushMessage(msg);
                return;
            }
            try
            {
                AsyncCallback callBack = new AsyncCallback(OnSendCallback);
                m_Sock.BeginSend(sendBuff, 0, sendBuff.Length, SocketFlags.None, callBack, this);
            }
            catch (SocketException e)
            {
                string strRes = String.Format("{0}:{1} ErrorCode={2}", e.ToString(), e.Message, e.ErrorCode);
                LogManager.LogError("KClientNet", strRes);

                KMessageOnError msg = new KMessageOnError();
                msg.m_iRez = KErrorCode.KERROR_SEND_CALLBACKFAILED;
                msg.m_iWsaRez = e.ErrorCode;
                PushMessage(msg);
            }
            catch
            {
                string strRes = String.Format("error");
                LogManager.LogError("KClientNet", strRes);

                KMessageOnError msg = new KMessageOnError();
                msg.m_iRez = KErrorCode.KERROR_SEND_CALLBACKFAILED;
                PushMessage(msg);
            }
        }

        static void OnSendCallback(IAsyncResult ar)
        {
            KClientNet clientNet = (KClientNet)ar.AsyncState;
            try
            {
                clientNet.m_Sock.EndSend(ar);
            }
            catch (SocketException e)
            {
                string strRes = "SendFailed";
                strRes = String.Format("{0}:{1} ErrorCode={2}", e.ToString(), e.Message, e.ErrorCode);
                LogManager.LogError("KClientNet", strRes);
                switch (e.ErrorCode)
                {
                    case 10060: // 连接超时
                        //msg.m_iRez = KConnectRet.KCALLBACK_TIMEOUT;
                        break;
                    case 10051: // 目标不可达(本地网络没有目标网络路由)
                        //msg.m_iRez = KConnectRet.KCALLBACK_CONNECTFAILED;
                        break;
                    case 10061: // 目标拒绝连接，一般是端口没有开启,端口错误
                        //msg.m_iRez = KConnectRet.KCALLBACK_REFUSED;
                        break;
                    default:    // 连接异常
                        //msg.m_iRez = KConnectRet.KCALLBACK_CONNECTFAILED;
                        break;
                }

                KMessageOnError msg = new KMessageOnError();
                msg.m_iRez = KErrorCode.KERROR_SEND_CALLBACKFAILED;
                msg.m_iWsaRez = e.ErrorCode;
                clientNet.PushMessage(msg);
            }
            catch
            {
                string strRes = "SendFailed";
                strRes = String.Format("error");
                LogManager.LogError("KClientNet", strRes);

                KMessageOnError msg = new KMessageOnError();
                msg.m_iRez = KErrorCode.KERROR_SEND_CALLBACKFAILED;
                clientNet.PushMessage(msg);
            }    
        }

        static void OnRecvCallback(IAsyncResult ar)
        {
            KClientNet clientNet = (KClientNet)ar.AsyncState;
            AsyncCallback callBack = new AsyncCallback(OnRecvCallback);
            try
            {
                int byterecv = clientNet.m_Sock.EndReceive(ar);
                if(byterecv <= 0)
                {
                    // 连接断开
                    KMessageOnError msg = new KMessageOnError();
                    msg.m_iRez = KErrorCode.KERROR_DISCONNECT_BYREMOTE;
                    clientNet.PushMessage(msg);
                    return;
                }
                // 进行消息处理
                clientNet.m_iRecvedLen = clientNet.m_iRecvedLen + byterecv;
                while (clientNet.m_iRecvedLen >= m_iMessageHead && clientNet.m_iRecvedLen >= BitConverter.ToUInt16(clientNet.m_recvBuf, 0))
                {
                    // 长度大于2字节长度+2字节ID，并且大于包长
                    UInt16 iLen = BitConverter.ToUInt16(clientNet.m_recvBuf, 0);
                    if (iLen < m_iMessageHead)
                    {
                        // 报文异常
                        throw new Exception();
                    }

                    //进行报文处理
                    UInt16 iId = BitConverter.ToUInt16(clientNet.m_recvBuf, 2);
                    byte[] data = new byte[iLen - 4];
                    Array.Copy(clientNet.m_recvBuf, 4, data, 0, iLen - 4);
                    Array.Copy(clientNet.m_recvBuf, iLen, clientNet.m_recvBuf, 0, clientNet.m_iRecvedLen - iLen);
                    clientNet.m_iRecvedLen -= iLen;

                    if (!KMessageMgr.ValidMessage(iId))
                    {
                        // pdl错误
                        KMessageOnError msg = new KMessageOnError();
                        msg.m_iRez = KErrorCode.KERROR_RECV_ID_ERROR;
                        clientNet.PushMessage(msg);
                        break;
                    }
                    KMessageOnData msgOnData = new KMessageOnData();
                    msgOnData.m_iMsgId = iId;
                    msgOnData.m_content = data;
                    clientNet.PushMessage(msgOnData);
                }
                // 开始接收操作
                clientNet.m_Sock.BeginReceive(clientNet.m_recvBuf, clientNet.m_iRecvedLen, m_iRecvBufLen - clientNet.m_iRecvedLen, SocketFlags.None, callBack, clientNet);
            }
            catch (SocketException e)
            {
                string strRes = String.Format("{0}:{1} ErrorCode={2}", e.ToString(), e.Message, e.ErrorCode);
                LogManager.LogError("KClientNet", strRes);
                switch (e.ErrorCode)
                {
                    case 10060: // 连接超时
                        //msg.m_iRez = KConnectRet.KCALLBACK_TIMEOUT;
                        break;
                    case 10051: // 目标不可达(本地网络没有目标网络路由)
                        //msg.m_iRez = KConnectRet.KCALLBACK_CONNECTFAILED;
                        break;
                    case 10061: // 目标拒绝连接，一般是端口没有开启,端口错误
                        //msg.m_iRez = KConnectRet.KCALLBACK_REFUSED;
                        break;
                    default:    // 连接异常
                        //msg.m_iRez = KConnectRet.KCALLBACK_CONNECTFAILED;
                        break;
                }

                KMessageOnError msg = new KMessageOnError();
                msg.m_iWsaRez = e.ErrorCode;
                msg.m_iRez = KErrorCode.KERROR_RECV_CALLBACKFAILED;
                clientNet.PushMessage(msg);
            }
            catch
            {
                string strRes = String.Format("error");
                LogManager.LogError("KClientNet", strRes);

                KMessageOnError msg = new KMessageOnError();
                msg.m_iRez = KErrorCode.KERROR_RECV_CALLBACKFAILED;
                clientNet.PushMessage(msg);
            }
        }

        // 逻辑处理相关
        public void Tick()
        {
            KMessage[] arMsg = null;
            lock(m_lMessageWait)
            {
                if(m_lMessageWait.Count > 0)
                {
                    arMsg = new KMessage[m_lMessageWait.Count];
                    m_lMessageWait.CopyTo(arMsg, 0);
                    m_lMessageWait.Clear();
                }
            }

            if(arMsg == null)
            {
                return;
            }

            foreach (KMessage msg in arMsg)
            {
                switch (msg.ID)
                {
                    case KMessage.KMessageType.KMessageType_ConnectCallback:
                        {
                            KMessageOnConnectCallback msgConnectcallback = (KMessageOnConnectCallback)msg;
                            if (m_Sock.Connected)
                            {
                                // 连接成功
                                m_iState = KClientNetState.KClientNet_Connected;

                                // 开始接收操作
                                try
                                {
                                    AsyncCallback callBack = new AsyncCallback(OnRecvCallback);
                                    m_Sock.BeginReceive(m_recvBuf, 0, m_iRecvBufLen, SocketFlags.None, callBack, this);
                                }
                                catch (SocketException e)
                                {
                                    string strRes = String.Format("{0}:{1} ErrorCode={2}", e.ToString(), e.Message, e.ErrorCode);
                                    LogManager.LogError("KClientNet", strRes);

                                    KMessageOnError msgE = new KMessageOnError();
                                    msgE.m_iRez = KErrorCode.KERROR_RECV_FAILED;
                                    msgE.m_iWsaRez = e.ErrorCode;
                                    PushMessage(msgE);
                                }
                                catch
                                {
                                    string strRes = String.Format("error");
                                    LogManager.LogError("KClientNet", strRes);

                                    KMessageOnError msgE = new KMessageOnError();
                                    msgE.m_iRez = KErrorCode.KERROR_RECV_FAILED;
                                    PushMessage(msgE);
                                }
                            }

                            OnConnectEnd(msgConnectcallback.m_iRez);
                        }
                        break;
                    case KMessage.KMessageType.KMessageType_OnError:
                        {
                            // 发生了异常
                            KMessageOnError msgError = (KMessageOnError)msg;
                            OnError(msgError.m_iRez, msgError.m_iWsaRez);
                        }
                        break;
                    case KMessage.KMessageType.KMessageType_OnData:
                        {
                            KMessageOnData msgOndata = (KMessageOnData)msg;
                            KMessageMgr.OnProtocolExecute(msgOndata.m_iMsgId, msgOndata.m_content);
                        }
                        break;
                }
            }
        }
    
        // Message相关
        void PushMessage(KMessage msg)
        {
            lock (m_lMessageWait)
            {
                m_lMessageWait.AddLast(msg);
            }
        }
    }
}