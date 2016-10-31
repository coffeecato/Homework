using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

namespace UnitTest
{
    public class LoadingUnit_AutoLogin : InterludeUnit
    {
        private float _timeout;
        private float _beginTime;

        public LoadingUnit_AutoLogin(float timeout = 2.0f)
        {
            _timeout = timeout;
            _beginTime = Time.unscaledTime;

            NetWork.instance.BeginConnectLGW();
        }

        public override int Weight
        {
            get { return 50; }
        }

        public override IEnumerator DoWork()
        {
            bool bDone = Timeout() || NetWork.instance.LoginSuccess();
            while (!bDone)
            {
                bDone = Timeout() || NetWork.instance.LoginSuccess();
                yield return null;
            }
        }

        public bool Timeout()
        {
            return Time.unscaledTime - _beginTime > _timeout;
        }
    }
}
