using System;
using System.Collections;
using UnityEngine;
using Framework;

namespace UnitTest
{
    public class LoadingUnit_Level : InterludeUnit
    {
        private AsyncOperation _asyncOp = null;
        private string _levelName = null;

        public LoadingUnit_Level(string levelName = null)
        {
            _levelName = levelName;
        }

        public override int Weight
        {
            get { return 50; }
        }

        public override float Progress
        {
            get { return _asyncOp == null ? 0 : _asyncOp.progress; }
        }

        public override IEnumerator DoWork()
        {
            float beginTime = Time.unscaledTime;
            while (_levelName == null)
            {
                if (Time.unscaledTime - beginTime > 5.0f)
                {
                    LogManager.LogDebug("LoadingUnit_Level::DoWork", "网络故障，使用单机地图");
                    _levelName = "SandBox2";
                }
                else
                    yield return null;
            }

            _asyncOp = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_levelName);
            yield return _asyncOp;
        }

        //public override void OnEvent(string strEvent, object param)
        //{
        //    if (strEvent == "mapId")
        //    {
        //        MapDesc mapDesc = ConfigStoreManager.GetMapDesc((int)(UInt32)param);
        //        if (mapDesc != null)
        //            _levelName = mapDesc.name;
        //        else
        //            LogManager.LogDebug("LoadingUnit_Level::OnEvent", "invalid mapId");
        //    }
        //}
    }
}
