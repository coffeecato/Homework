using System.Collections;
using Framework.Asset;
using UnityEngine;
using Framework;

namespace UnitTest
{
    public class LoadingUnit_Asset : InterludeUnit
    {
        private string _assetBundleName;
        private string _assetName;
        private int _version;
        private bool _instantiateNow;

        public LoadingUnit_Asset(string assetBundleName, int version, string assetName, bool instantiateNow)
        {
            _assetBundleName = assetBundleName;
            _assetName = assetName;
            _version = version;
            _instantiateNow = instantiateNow;
        }

        public override int Weight
        {
            get { return 90; }
        }

        public override float Progress
        {
            get
            {
                if (IsCompleted)
                    return 1.0f;
                return AssetBundleManager.GetABLoadProgressWithDependencies(_assetBundleName, _version);
            }
        }

        public override IEnumerator DoWork()
        {
            using (AssetLoaderAsync assetLoader = new AssetLoaderAsync(_assetBundleName, _version, _assetName, typeof(GameObject)))
            {
                if (assetLoader.asset == null)
                    yield return AssetManager.instance.StartCoroutine(assetLoader);

                if (_instantiateNow)
                {
                    GameObject pref = assetLoader.asset == null ? null : assetLoader.asset as GameObject;
                    if (pref != null)
                        UnityEngine.Object.Instantiate(pref);
                }
            }
        }
    }
}
