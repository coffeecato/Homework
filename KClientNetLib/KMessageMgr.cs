using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace KClientNetLib
{
    public enum ClientVersion
    {
        ClientVersion_Value = 1,
    }

    public partial class KMessageMgr
    {
        private static Dictionary<UInt16, Type> s_dicMsgId2Msg = new Dictionary<UInt16, Type>();
        private static Dictionary<Type, UInt16> s_dicMsg2MsgId = new Dictionary<Type, UInt16>();
        
        public static bool ValidMessage(UInt16 iMsgId)
        {
            if( !s_dicMsgId2Msg.ContainsKey(iMsgId) )
                return false;
            return true;
        }
        
        public static UInt16 GetMessageId(Type t)
        {
            UInt16 iMsgId = 0;
            s_dicMsg2MsgId.TryGetValue(t, out iMsgId);
            return iMsgId;
        }

        public static Type GetMessageType(UInt16 iMsgId)
        {
            Type msgType = null;
            s_dicMsgId2Msg.TryGetValue(iMsgId, out msgType);
            return msgType;
        }
    }
}