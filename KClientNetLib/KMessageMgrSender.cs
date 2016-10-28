using System.Collections;

namespace KClientNetLib
{
    public partial class KMessageMgr
    {
        public static void Initialzie()
        {
			s_dicMsgId2Msg.Add(513, typeof(KLogin.UDPLinkOpenReq));
			s_dicMsg2MsgId.Add(typeof(KLogin.UDPLinkOpenReq), 513);
			s_dicMsgId2Msg.Add(514, typeof(KLogin.UDPLinkCloseReq));
			s_dicMsg2MsgId.Add(typeof(KLogin.UDPLinkCloseReq), 514);
			s_dicMsgId2Msg.Add(515, typeof(KLogin.HeartBeatReq));
			s_dicMsg2MsgId.Add(typeof(KLogin.HeartBeatReq), 515);
			s_dicMsgId2Msg.Add(516, typeof(KLogin.Login1Req));
			s_dicMsg2MsgId.Add(typeof(KLogin.Login1Req), 516);
			s_dicMsgId2Msg.Add(517, typeof(KLogin.Login1Req.ClientVersion));
			s_dicMsg2MsgId.Add(typeof(KLogin.Login1Req.ClientVersion), 517);
			s_dicMsgId2Msg.Add(518, typeof(KLogin.Login2Req));
			s_dicMsg2MsgId.Add(typeof(KLogin.Login2Req), 518);
			s_dicMsgId2Msg.Add(519, typeof(KLogin.Login2Req.ClientVersion));
			s_dicMsg2MsgId.Add(typeof(KLogin.Login2Req.ClientVersion), 519);
			s_dicMsgId2Msg.Add(769, typeof(KLogin.HeartBeatAck));
			s_dicMsg2MsgId.Add(typeof(KLogin.HeartBeatAck), 769);
			s_dicMsgId2Msg.Add(770, typeof(KLogin.UDPLinkOpenAck));
			s_dicMsg2MsgId.Add(typeof(KLogin.UDPLinkOpenAck), 770);
			s_dicMsgId2Msg.Add(771, typeof(KLogin.UDPLinkCloseAck));
			s_dicMsg2MsgId.Add(typeof(KLogin.UDPLinkCloseAck), 771);
			s_dicMsgId2Msg.Add(772, typeof(KLogin.Login1Ack));
			s_dicMsg2MsgId.Add(typeof(KLogin.Login1Ack), 772);
			s_dicMsgId2Msg.Add(773, typeof(KLogin.LoginQueuingNtf));
			s_dicMsg2MsgId.Add(typeof(KLogin.LoginQueuingNtf), 773);
			s_dicMsgId2Msg.Add(774, typeof(KLogin.LoginConfigMD5Ntf));
			s_dicMsg2MsgId.Add(typeof(KLogin.LoginConfigMD5Ntf), 774);
			s_dicMsgId2Msg.Add(1025, typeof(KLGWServerMsg.LoginQueuingNtf));
			s_dicMsg2MsgId.Add(typeof(KLGWServerMsg.LoginQueuingNtf), 1025);
			s_dicMsgId2Msg.Add(1026, typeof(KLGWServerMsg.LoginConfigMD5Ntf));
			s_dicMsg2MsgId.Add(typeof(KLGWServerMsg.LoginConfigMD5Ntf), 1026);

        }
    }
}