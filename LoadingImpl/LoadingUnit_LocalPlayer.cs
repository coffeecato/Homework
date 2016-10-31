using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

namespace UnitTest
{
    public class LoadingUnit_LocalPlayer : InterludeUnit
    {
        //private PlayerController _pc = null;
        private bool _bInitedByServer = false;
        //private UInt64 _actorId;
        //private string _nickName;
        //private UInt32 _charaId;
        //private Vector3 _startPos;

        public LoadingUnit_LocalPlayer()
        {
        }

        public override int Weight
        {
            get { return 50; }
        }

        public override float Progress
        {
            get
            {
                return 0;   // (_pc == null || _pc.character == null) ? 0f : 1f;
            }
        }

        public override IEnumerator DoWork()
        {
            //NetWork.instance.PlayerEnterWorldPost();

            int secCount = 5;
            while (_bInitedByServer == false && secCount-- > 0)
                yield return new WaitForSeconds(1f);

            //if (_bInitedByServer)
            //{
            //    LevelInfo.Instance.AddCharacterController(_actorId, _nickName, (int)_charaId, true, _startPos);
            //}
            //else
            //{
            //    LogManager.LogDebug("网络故障", "使用单机角色");
            //    LevelInfo.Instance.AddCharacterController(1, "test", 1, true);
            //}

            //_pc = LevelInfo.Instance.GetLocalCharacterController();
            //if (_pc == null)
            //    yield break;
            
            //while (_pc.character == null)
                yield return null;
        }

        //public override void OnEvent(string strEvent, object param)
        //{
        //    if (strEvent == "PlayerDataNtf")
        //    {
        //        Dictionary<string, System.Object> paramDict = (Dictionary<string, System.Object>)param;
        //        System.Object temp;

        //        if (paramDict.TryGetValue("actorId", out temp))
        //            _actorId = (UInt64)temp;

        //        if (paramDict.TryGetValue("nickName", out temp))
        //            _nickName = (string)temp;

        //        if (paramDict.TryGetValue("charaId", out temp))
        //            _charaId = (UInt32)temp;

        //        if (paramDict.TryGetValue("startPos", out temp))
        //            _startPos = (Vector3)temp;

        //        _bInitedByServer = true;
        //    }
        //}
    }
}
