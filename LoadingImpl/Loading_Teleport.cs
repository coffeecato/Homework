using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
using Framework.Asset;

namespace UnitTest
{
    public class Loading_Teleport : Interlude
    {
        protected override void InitUnitList()
        {
        }
        protected override IEnumerator SuspendInterludeAnim()
        {
            yield break;
        }

        protected override IEnumerator PrepareInterludeAnim()
        {
            using (AssetLoaderAsync assetLoader = new AssetLoaderAsync("assets_prefabs", 0, "TeleportLoadingUI", typeof(GameObject)))
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

        //    UIView originView = UI.GetView("TeleportLoadingUI");
        //    StartupLoadingUIView view = originView as StartupLoadingUIView;
        //    if (view == null)
        //        yield break;
        }

        protected override IEnumerator EndInterludeAnim()
        {
            yield break;
            //while (!SceneManager.instance.IsTeleportEnd()) yield return null;

            //StartupLoadingUIView view = EngineApp.UI.GetView("TeleportLoadingUI") as StartupLoadingUIView;
            //if (view == null)
            //    yield break;

            //float speed = 1.0f;
            //float alpha = 1.0f;
            //while (alpha > 0)
            //{
            //    alpha -= speed * Time.smoothDeltaTime;
            //    view.UpdateAlpha(alpha);
            //    yield return null;
            //}
        }
    }
}
