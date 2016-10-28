using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using Framework;
using Framework.Asset;

namespace Framework
{
    public class GlobalUtility
    {
        static public GameObject FindLevelObject()
        {
            GameObject levelObj = GameObject.FindWithTag("tagLevel");
            if (levelObj == null)
                Debug.LogError(@"Not found GameObject[LevelRoot] with tag[tagLevel]!");

            return levelObj;
        }

        static public T FindInParents<T>(GameObject go) where T : Component
        {
            if (go == null) return null;
            var comp = go.GetComponent<T>();

            if (comp != null)
                return comp;

            Transform t = go.transform.parent;
            while (t != null && comp == null)
            {
                comp = t.gameObject.GetComponent<T>();
                t = t.parent;
            }
            return comp;
        }

        static public string GetFullname(Transform tran)
        {
            if (tran == null) return string.Empty;

            string fullname = tran.name;
            if (tran.parent != null)
            {
                fullname = string.Concat(GetFullname(tran.parent), ".", tran.name);
            }
            return fullname;
        }

        // 从资源路径解析出其所在的 AssetBundle Name 和 Asset Name
        public static bool ParseAssetPath(string path, out string abName, out string assetName)
        {
            abName = null;
            assetName = null;

            string[] strList = path.Split('/');
            if (strList == null || strList.Length < 2)
                return false;

            abName = strList[0].ToLower();
            for (int i = 1; i < strList.Length - 1; ++i)
                abName += string.Concat("_", strList[i].ToLower());

            assetName = strList[strList.Length - 1];
            return true;
        }

        // 仅获得当前程序集的类类型
        public static Type GetClassType(string nameSpace, string className)
        {
            if( string.IsNullOrEmpty(className) )
                return null;

            Assembly ass = Assembly.GetExecutingAssembly();
            if (ass == null)
            {
                LogManager.LogWarning("GlobalUtility", "GetExecutingAssembly failed");
                return null;
            }

            string fullPath = string.IsNullOrEmpty(nameSpace) ? className : string.Concat(nameSpace, ".", className);
            Type type = ass.GetType(fullPath);
            if (type == null)
            {
                LogManager.LogWarning("GlobalUtility", "GetType failed, nameSpace["+nameSpace+"] className["+className+"]");
            }
            return type;
        }

        // "Framework.GameApp" -> "Framework" & "GameApp"
        public static bool ParseClassName(string str, out string nameSpace, out string className)
        {
            nameSpace = string.Empty;
            className = string.Empty;
            if( string.IsNullOrEmpty(str) )
                return false;

            int index = str.LastIndexOf(".");
            if (index == -1)
                return false;

            nameSpace = str.Substring(0, index);
            className = str.Substring(index + 1);
            return true;
        }

        // 卸载所有缓存及不再使用的资源，通常在切换地图时调用
        public static void UnloadAll()
        {
            AssetManager.Clear();
            AssetBundleManager.UnloadAll();

            Resources.UnloadUnusedAssets();

            System.GC.Collect();
        }

        public static GameObject CreateGameObject(string name, bool bDontDestroy)
        {
            GameObject go = new GameObject();
            go.hideFlags = HideFlags.DontSave;
            go.name = name;
            if (Application.isPlaying && bDontDestroy) UnityEngine.Object.DontDestroyOnLoad(go);
            return go;
        }

        // 同步加载单件
        public static void AddSingletonMono<T>(GameObject go = null) where T : SingletonMono<T>
        {
            if (UnityEngine.Object.FindObjectOfType<T>() == null)
            {
                GameObject host = go;
                if (host == null)
                {
                    host = CreateGameObject(string.Concat("[singleton]", typeof(T).ToString()), true);
                }

                if (host != null && host.GetComponent<T>() == null)
                {
                    T t = host.AddComponent<T>();
                    if (t == null)
                        return;
                    
                    SingletonMono<T>.instance = t;
                    t.Init();
                }
            }
        }

        public static void AddSingletonMono<T>(Type type, GameObject go = null) where T : SingletonMono<T>
        {
            if (type != null && UnityEngine.Object.FindObjectOfType(type) == null)
            {
                GameObject host = go;
                if (host == null)
                {
                    host = CreateGameObject(string.Concat("[singleton]", typeof(T).ToString()), true);
                }

                if (host != null && host.GetComponent(type) == null)
                {
                    T t = host.AddComponent(type) as T;
                    if (t == null)
                        return;

                    SingletonMono<T>.instance = t;
                    t.Init();
                }
            }
        }

        // 异步加载单件
        public static IEnumerator AddSingletonMonoAsync<T>(GameObject go = null) where T : SingletonMono<T>
        {
            if (UnityEngine.Object.FindObjectOfType<T>() == null)
            {
                GameObject host = go;
                if (host == null)
                {
                    host = CreateGameObject(string.Concat("[singleton]", typeof(T).ToString()), true);
                }

                if (host != null && host.GetComponent<T>() == null)
                {
                    T t = host.AddComponent<T>();
                    if (t == null)
                        yield break;
                    
                    SingletonMono<T>.instance = t;
                    yield return t.StartCoroutine(t.InitAysnc());
                }
            }
        }

        public static IEnumerator AddSingletonMonoAsync<T>(Type type, GameObject go = null) where T : SingletonMono<T>
        {
            if (type != null && UnityEngine.Object.FindObjectOfType(type) == null)
            {
                GameObject host = go;
                if (host == null)
                {
                    host = CreateGameObject(string.Concat("[singleton]", typeof(T).ToString()), true);
                }

                if (host != null && host.GetComponent(type) == null)
                {
                    T t = host.AddComponent(type) as T;
                    if( t == null )
                        yield break;

                    SingletonMono<T>.instance = t;
                    yield return t.StartCoroutine(t.InitAysnc());
                }
            }
        }
    }
}