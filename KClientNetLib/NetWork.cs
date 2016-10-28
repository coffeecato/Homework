using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

namespace UnitTest
{
    public class NetWorkImp : KClientNetLib.KClientNet
    {
        private NetWork m_networkRef;

        public NetWorkImp(NetWork network)
        {
            m_networkRef = network;
        }
        override public void OnConnectEnd(KClientNetLib.KConnectRet iRet)
        {
            m_networkRef.OnConnectEnd(iRet);
        }
        override public void OnError(KClientNetLib.KErrorCode iError, int iWsaRet)
        {
            m_networkRef.OnError(iError, iWsaRet);
        }
    }

    public class NetWork : SingletonMono<NetWork>
    {
        private enum PlayerState
        {
            PlayerStateNone = 0,
            PlayerStateConnectingLGW,
            PlayerStateLoginingLGW,
            PlayerStateLoginingLGWOK,
            PlayerStateConnectingGW,
            PlayerStateLoginingGW,
            PlayerStateLoginingGWOK
        };

        public NetWorkImp m_NetWork;
        public string m_strLGWIp = "222.73.104.135";
        public ushort m_iLGWPort = 2301;
        private string m_strGWIp;
        private uint m_iGWPort;

        private PlayerState m_iState = PlayerState.PlayerStateNone;

        // Use this for initialization
        override public IEnumerator InitAysnc()
        {
            _Init();
            yield break;
        }

        override public void Init()
        {
            _Init();
        }

        private void _Init()
        {
            KClientNetLib.KMessageMgr.Initialzie();

            m_NetWork = new NetWorkImp(this);
        }

        void OnDestroy()
        {
            m_NetWork = null;
        }

        // Update is called once per frame
        override public void Tick()
        {
            m_NetWork.Tick();
        }

        public void OnConnectEnd(KClientNetLib.KConnectRet iRet)
        {
            if (iRet == KClientNetLib.KConnectRet.KCALLBACK_SUCCESS)
            {
                if (m_iState == PlayerState.PlayerStateConnectingLGW)
                {
                    EndConnectLGW();
                }
                else if (m_iState == PlayerState.PlayerStateConnectingGW)
                {
                    EndConnectGW();
                }
            }
            else
            {
                switch (iRet)
                {
                    case KClientNetLib.KConnectRet.KCALLBACK_CONNECTFAILED:
                        LogManager.LogError("", "网络开启异常，请查看设置");
                        break;
                }

                Reset();
            }
        }

        public void OnError(KClientNetLib.KErrorCode iError, int iWsaRet)
        {
            string str = String.Format("{0} {1}", iError, iWsaRet);
            LogManager.LogDebug("OnError", str);

            if (m_iState == PlayerState.PlayerStateLoginingLGWOK)
            { // 正常，断开与LGW的连接
                BeginConnectGW(m_strGWIp, (int)m_iGWPort);
            }
            else
            {
                Reset();
            }
        }

        public void Reset()
        {
            m_iState = PlayerState.PlayerStateNone;
            m_NetWork.Reset();
        }

        public void BeginConnectLGW()
        {
            if (m_iState == PlayerState.PlayerStateNone)
            {
                m_iState = PlayerState.PlayerStateConnectingLGW;        // 开始连接LGW
                m_NetWork.Login(m_strLGWIp, m_iLGWPort);

                LogManager.LogDebug("BeginConnectLGW", string.Format("current state:{0} ip:{1} port:{2}", m_iState.ToString(), m_strLGWIp, m_iLGWPort));
            }
            else
            {
                LogManager.LogError("BeginConnectLGW", "error state: " + m_iState.ToString());
            }
        }

        void EndConnectLGW()
        {
            if (m_iState == PlayerState.PlayerStateConnectingLGW)
            {
                m_iState = PlayerState.PlayerStateLoginingLGW;

                KLogin.Login1Req.Send(SystemInfo.deviceUniqueIdentifier + ";123456", 0, 1, 0);

                LogManager.LogDebug("EndConnectLGW", "current state: " + m_iState.ToString());
            }
            else
            {
                LogManager.LogError("EndConnectLGW", "error state: " + m_iState.ToString());
            }
        }

        public void OnLogin1Ack(int iRet, uint iIp, uint iPort)
        {
            string strLog = String.Format("iRet={0} iIp={1} iPort={2}", iRet, iIp, iPort);
            LogManager.LogDebug("OnLogin1Ack", strLog);

            if (iRet != 0)
            {
                Reset();
                return;
            }

            switch (m_iState)
            {
                case PlayerState.PlayerStateLoginingLGW:
                    m_iState = PlayerState.PlayerStateLoginingLGWOK;

                    uint iIp4 = iIp >> 24;
                    uint iIp3 = (iIp >> 16) % 256;
                    uint iIp2 = (iIp >> 8) % 256;
                    uint iIp1 = iIp % 256;
                    m_strGWIp = iIp1.ToString() + "." + iIp2.ToString() + "." + iIp3.ToString() + "." + iIp4.ToString();
                    m_iGWPort = iPort;
                                        
                    break;
            }
        }

        void BeginConnectGW(string strGWIp, int iGWPort)
        {
            if (m_iState == PlayerState.PlayerStateLoginingLGWOK)
            {
                m_iState = PlayerState.PlayerStateConnectingGW;
                m_NetWork.Login(strGWIp, iGWPort);

                LogManager.LogDebug("BeginConnectGW", "current state: " + m_iState.ToString());
            }
            else
            {
                LogManager.LogError("BeginConnectGW", "error state: " + m_iState.ToString());
            }
        }

        void EndConnectGW()
        {
            if (m_iState == PlayerState.PlayerStateConnectingGW)
            {
                m_iState = PlayerState.PlayerStateLoginingGW;

                KLogin.Login2Req.Send(SystemInfo.deviceUniqueIdentifier + ";123456", 0, 1, 0);

                LogManager.LogDebug("EndConnectGW", "current state: "+m_iState.ToString());
            }
            else
            {
                LogManager.LogError("EndConnectGW", "error state: " + m_iState.ToString());
            }
        }

        public bool LoginSuccess()
        {
            return m_iState >= PlayerState.PlayerStateLoginingGW;
        }

        static public void SendProtocol<T>(T msg)
        {
            MemoryStream stream = new MemoryStream();
            ProtoBuf.Serializer.Serialize<T>(stream, msg);

            NetWork.instance.m_NetWork.Send(stream.ToArray(), KClientNetLib.KMessageMgr.GetMessageId(msg.GetType()));
        }

        ///////////////////////// 除登录流程之外的协议
    }
}