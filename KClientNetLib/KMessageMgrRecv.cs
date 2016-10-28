using System;
using System.IO;
using System.Collections;
using Framework;

namespace KClientNetLib
{
    public partial class KMessageMgr
    {
        static public void OnProtocolExecute(UInt16 iMsgId, byte[] content)
        {
            // validate message
            Type msgType = GetMessageType(iMsgId);
            if (!ProtoBuf.Serializer.NonGeneric.CanSerialize(msgType))
            {
                LogManager.LogError("", "Illegal MsgId: " + iMsgId);
                return;
            }

            // parse message
            object msgObject = ProtoBuf.Serializer.NonGeneric.Deserialize(msgType, new MemoryStream(content));

            // dispatch message
            switch (iMsgId)
            {
                case 769:
                    {
                        KLogin.HeartBeatAck t = msgObject as KLogin.HeartBeatAck;
                        t.OnExecute();
                    }
                    break;
                case 770:
                    {
                        KLogin.UDPLinkOpenAck t = msgObject as KLogin.UDPLinkOpenAck;
                        t.OnExecute();
                    }
                    break;
                case 771:
                    {
                        KLogin.UDPLinkCloseAck t = msgObject as KLogin.UDPLinkCloseAck;
                        t.OnExecute();
                    }
                    break;
                case 772:
                    {
                        KLogin.Login1Ack t = msgObject as KLogin.Login1Ack;
                        t.OnExecute();
                    }
                    break;
                case 773:
                    {
                        KLogin.LoginQueuingNtf t = msgObject as KLogin.LoginQueuingNtf;
                        t.OnExecute();
                    }
                    break;
                case 774:
                    {
                        KLogin.LoginConfigMD5Ntf t = msgObject as KLogin.LoginConfigMD5Ntf;
                        t.OnExecute();
                    }
                    break;
                case 1025:
                    {
                        KLGWServerMsg.LoginQueuingNtf t = msgObject as KLGWServerMsg.LoginQueuingNtf;
                        t.OnExecute();
                    }
                    break;
                case 1026:
                    {
                        KLGWServerMsg.LoginConfigMD5Ntf t = msgObject as KLGWServerMsg.LoginConfigMD5Ntf;
                        t.OnExecute();
                    }
                    break;

            }
		}
	}
}