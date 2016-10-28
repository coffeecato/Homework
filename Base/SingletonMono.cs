using System.Collections;
using UnityEngine;

namespace Framework
{
    public abstract class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
    {
        private static T _instance;
        
        private static object _lock = new object();

        private static bool _applicationIsQuitting = false;

        public static T instance
        {
            get
            {
                if (_applicationIsQuitting)        // app退出时单件不可用，始终返回null
                    return null;

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<T>();
                        if (_instance == null)
                        {
                            GameObject go = GlobalUtility.CreateGameObject(string.Concat("[singleton]", typeof(T).ToString()), true);

                            _instance = go.AddComponent<T>();
                            _instance.Init();
                        }
                    }
                    return _instance;
                }
            }
            set
            {
                if( _instance == null && !_applicationIsQuitting )
                {
                    _instance = value;
                }
            }
        }
        
        void OnDestroy()
        {
            _applicationIsQuitting = true;
        }

        // 异步初始化
        public virtual IEnumerator InitAysnc() { yield break; }

        // 同步初始化
        public virtual void Init() { }
        
        // 需要轮询操作的，对时序有要求的使用Tick，其他可使用Update
        public virtual void Tick() { }
    }
}