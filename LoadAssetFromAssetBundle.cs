using System.Collections;
using UnityEngine;
using Framework.Asset;

namespace Framework
{
    class LoadAssetFromAssetBundle : MonoBehaviour
    {
        private UnityEngine.GameObject ObjInstance = null;
        public string AssetBundleName = "";
        public string AssetName = "";
        public int Version = 0;

        IEnumerator Start()
        {
            using (AssetLoaderAsync assetLoader = new AssetLoaderAsync(AssetBundleName, Version, AssetName, typeof(GameObject)))
            {
                if (assetLoader.asset == null)
                    yield return StartCoroutine(assetLoader);

                if (assetLoader.asset != null)
                    ObjInstance = (GameObject)Instantiate(assetLoader.asset);
            }
        }

        public UnityEngine.GameObject GetInstanceGameObject()
        {
            return ObjInstance;
        }
    }
}

