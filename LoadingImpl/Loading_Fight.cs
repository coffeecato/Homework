using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
using Framework.Asset;

namespace UnitTest
{
    public class Loading_Fight : Interlude
    {
        protected override void InitUnitList()
        {
            //_unitList.Add(new LoadingUnit_Level("Login")); // 加载场景
        }

        protected override IEnumerator PrepareInterludeAnim()
        {
            using (AssetLoaderAsync assetLoader = new AssetLoaderAsync("assets_prefabs", 0, "StartupLoadingUI", typeof(GameObject)))
            {
                if (assetLoader.asset == null)
                    yield return StartCoroutine(assetLoader);

                GameObject pref = assetLoader.asset as GameObject;
                if (pref != null)
                    Instantiate(pref);
            }
        }
        protected override IEnumerator SuspendInterludeAnim()
        {
            yield break;
        }
        protected override IEnumerator StartInterludeAnim()
        {
            yield break;
        //    //UIController UI = EngineApp.UI;
        //    //if (UI == null)
        //    //    yield break;

        //    //UIView originView = UI.GetView("StartupLoadingUI");
        //    //StartupLoadingUIView view = originView as StartupLoadingUIView;
        //    //if (view == null)
        //    //    yield break;

        //    //while (!IsCompleted)
        //    //{
        //    //    //view.UpdateProgress(TotalProgress);
        //    //    yield return null;
        //    //}
        //    ////view.UpdateProgress(TotalProgress);
        }

        protected override IEnumerator EndInterludeAnim()
        {
            yield break;
        //    StartupLoadingUIView view = EngineApp.UI.GetView("StartupLoadingUI") as StartupLoadingUIView;
        //    if (view == null)
        //        yield break;

        //    float speed = 1.0f;
        //    float alpha = 1.0f;
        //    while (alpha > 0)
        //    {
        //        alpha -= speed * Time.smoothDeltaTime;
        //        view.UpdateAlpha(alpha);
        //        yield return null;
        //    }
        }
    }
}
