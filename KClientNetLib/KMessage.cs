using System;
using System.Collections.Generic;

namespace KClientNetLib
{
	public class KMessage
	{
        /*
         * 实现KClientNet的消息队列功能
         */
        public enum KMessageType
        {
            KMessageType_Invalid = 0,
            KMessageType_ConnectCallback,
            KMessageType_OnError,
            KMessageType_OnData
        }
        private KMessageType m_iId;
        public KMessageType ID { get { return m_iId; } protected set { m_iId = value; } }
	}

    public class KMessageOnConnectCallback : KMessage
    {

        public KConnectRet m_iRez;
        public KMessageOnConnectCallback()
        {
            ID = KMessage.KMessageType.KMessageType_ConnectCallback;
            m_iRez = KConnectRet.KCALLBACK_SUCCESS;
        }
    }

    public class KMessageOnError : KMessage
    {

        public KErrorCode m_iRez;
        public int m_iWsaRez;

        public KMessageOnError()
        {
            ID = KMessage.KMessageType.KMessageType_OnError;
            m_iRez = KErrorCode.KERROR_SUCCESS;
            m_iWsaRez = 0;
        }
    }

    public class KMessageOnData : KMessage
    {
        public UInt16 m_iMsgId;
        public byte[] m_content;

        public KMessageOnData()
        {
            ID = KMessage.KMessageType.KMessageType_OnData;
        }
    }
}
