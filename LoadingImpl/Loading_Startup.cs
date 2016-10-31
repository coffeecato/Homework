using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
using Framework.Asset;

namespace UnitTest
{
    public class Loading_Startup : Interlude
    {
        protected override void InitUnitList()
        {
            //_unitList.Add(new LoadingUnit_AutoLogin(5.0f));
            //_unitList.Add(new LoadingUnit_Level()); // 加载场景
            //_unitList.Add(new LoadingUnit_SceneMgr());
            //_unitList.Add(new LoadingUnit_LocalPlayer());
        }
        protected override IEnumerator SuspendInterludeAnim()
        {
            yield break;
        }

        protected override IEnumerator PrepareInterludeAnim()
        {
            EngineApp.StopAssetLoadingCoroutines();

            using (AssetLoaderAsync assetLoader = new AssetLoaderAsync("assets_prefabs", 0, "StartupLoadingUI.prefab", typeof(GameObject)))
            {
                if (assetLoader.asset == null)
                    yield return StartCoroutine(assetLoader);

                GameObject pref = assetLoader.asset as GameObject;
                if (pref != null)
                    Instantiate(pref);
            }
        }

        protected override IEnumerator StartInterludeAnim()
        {
            yield break;
        //    UIController UI = EngineApp.UI;
        //    if (UI == null)
        //        yield break;

        //    UIView originView = UI.GetView("StartupLoadingUI");
        //    StartupLoadingUIView view = originView as StartupLoadingUIView;
        //    if (view == null)
        //        yield break;

        //    while (!IsCompleted)
        //    {
        //        //view.UpdateProgress(TotalProgress);
        //        yield return null;
        //    }
        //    //view.UpdateProgress(TotalProgress);
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
