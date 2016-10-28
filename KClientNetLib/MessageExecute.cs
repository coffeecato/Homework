using System.IO;
using System.Collections;
using System.Collections.Generic;
using System;
using Framework;
using UnitTest;

namespace KLogin
{
    public partial class Login1Req
    {
        static public void Send(string strToken, uint branchVersion, uint branchChildVersion, uint hotfixVersion)
        {
            KLogin.Login1Req.ClientVersion version = new KLogin.Login1Req.ClientVersion();
            version.branchVersion = branchVersion;
            version.branchChildVersion = branchChildVersion;
            version.hotfixVersion = hotfixVersion;

            KLogin.Login1Req msg = new KLogin.Login1Req();
            msg.iVersion = (int)KClientNetLib.ClientVersion.ClientVersion_Value;
            msg.strToken = strToken;
            msg.sClientVersion = version;

            NetWork.SendProtocol<KLogin.Login1Req>(msg);
        }
    }

    public partial class Login1Ack
    {
        public void OnExecute()
        {
            NetWork.instance.OnLogin1Ack(iRet, iGWIp, iGWPort);
        }
    }

    public partial class Login2Req
    {
        static public void Send(string strToken, uint branchVersion, uint branchChildVersion, uint hotfixVersion)
        {
            KLogin.Login2Req.ClientVersion version = new KLogin.Login2Req.ClientVersion();
            version.branchVersion = branchVersion;
            version.branchChildVersion = branchChildVersion;
            version.hotfixVersion = hotfixVersion;

            KLogin.Login2Req msg = new KLogin.Login2Req();
            msg.iVersion = (int)KClientNetLib.ClientVersion.ClientVersion_Value;
            msg.strToken = strToken;
            msg.sClientVersion = version;

            NetWork.SendProtocol<KLogin.Login2Req>(msg);
        }
    }

    public partial class HeartBeatAck
    {
        public void OnExecute()
        {
        }
    }

    public partial class UDPLinkOpenAck
    {
        public void OnExecute()
        {
        }
    }

    public partial class UDPLinkCloseAck
    {
        public void OnExecute()
        {
        }
    }

    public partial class LoginQueuingNtf
    {
        public void OnExecute()
        {
        }
    }

    public partial class LoginConfigMD5Ntf
    {
        public void OnExecute()
        {
        }
    }
}

namespace KLGWServerMsg
{
    public partial class LoginQueuingNtf
    {
        public void OnExecute()
        {
        }
    }

    public partial class LoginConfigMD5Ntf
    {
        public void OnExecute()
        {
        }
    }
}
