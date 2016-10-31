using System.Collections;
using UnityEngine;
using Framework;

namespace UnitTest
{
    public class LoadingUnit_SceneMgr : InterludeUnit
    {
        private float fakeProgress = 0.0f;

        public override int Weight
        {
            get { return 50; }
        }

        public override float Progress
        {
            get
            {
                if (IsCompleted)
                    return 1.0f;

                fakeProgress += 0.01f;
                if (fakeProgress > 0.95f)
                    fakeProgress = 0.95f;

                return fakeProgress;
            }
        }

        public override IEnumerator DoWork()
        {
            yield break;
            //GameObject go = new GameObject();
            //go.name = "SceneMgr";

            //SceneManager mgr = go.AddComponent<SceneManager>();
            //yield return EngineApp.instance.StartCoroutine(mgr.Init());
        }
    }
}
