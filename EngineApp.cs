using System;
using System.Collections;
using UnityEngine;
using Framework.Asset;
using Framework.UI;

namespace Framework
{
    // 控制生命周期为整个app的全局对象管理类，例如各种单件、网络层、配置文件读取等
    public class EngineApp : MonoBehaviour
    {
        private static EngineApp _instance = null;               // self

        private EngineSetting _engineSetting;
        public EngineSetting engineSetting
        {
            get 
            {
                if (_engineSetting == null)
                {
                    _engineSetting = ScriptableObject.CreateInstance<EngineSetting>();
                }
                return _engineSetting; 
            }
            set { _engineSetting = value; }
        }

        // 计算FPS相关参数
        private static float _UpdateInterval = 1.0f;
        private static double _LastTime;
        private static int _FrameCount;
        private static float _FPS;
               
        void Awake()
        {
            instance = this;
        }

        public static EngineApp instance
        {
            get
            {
                return _instance;
            }
            set
            {
                if (_instance != null && value == null)
                {
                    UnityEngine.Object.Destroy(_instance.gameObject);
                    _instance = null;
                }
                else if (_instance == null && value != null)
                {
                    _instance = value;
                    GameObject.DontDestroyOnLoad(_instance.gameObject);
                }
                else if (_instance != null && value != null)
                {
                    Debug.Log("EngineApp has been instanced.");
                }
            }
        }

        IEnumerator Start()
        {
            _LastTime = Time.realtimeSinceStartup;
            _FrameCount = 0;

            // 优先初始化资源加载模块
            yield return StartCoroutine(GlobalUtility.AddSingletonMonoAsync<AssetBundleManager>());
            yield return StartCoroutine(GlobalUtility.AddSingletonMonoAsync<AssetManager>());

            // 加载环境变量
            yield return StartCoroutine(LoadSettingConfig());

            // 初始化引擎设置
            yield return StartCoroutine(InitEngineSetting());

            // 初始化各种Manager
            yield return StartCoroutine(GlobalUtility.AddSingletonMonoAsync<LogManager>());

            yield return StartCoroutine(GlobalUtility.AddSingletonMonoAsync<UISysManager>(engineSetting._typeOfUISys));
            
            yield return StartCoroutine(GlobalUtility.AddSingletonMonoAsync<GameApp>(engineSetting._typeOfGameApp));
            
            // 执行默认行为
            //StartInterlude(engineSetting._typeOfBehaviourPostInitEngine, new System.Object[1] {"login"});
        }

        void Update()
        {
            UpdateFPS();

            if (GameApp.isInited)
            {
                GameApp.instance.Tick();
            }
        }

        private IEnumerator LoadSettingConfig()
        {
            using (AssetLoaderAsync assetLoader = new AssetLoaderAsync("assets_config", 0, "EngineSetting", typeof(EngineSetting), false, AssetBundleManager.AssetBundleLoadMethod.FromFile))
            {
                if (assetLoader.asset == null)
                    yield return StartCoroutine(assetLoader);

                engineSetting.CopyFrom(assetLoader.asset as EngineSetting);
            }

            yield break;
        }

        private IEnumerator InitEngineSetting()
        {
            QualitySettings.vSyncCount = 0;         // 关闭vSync，targetFrameRate才会生效
#if UNITY_EDITOR
            Application.targetFrameRate = engineSetting._useDefaultFrameRate ? -1 : Mathf.Clamp(engineSetting._fixedFrameRate, 10, 60);
#else
            Application.targetFrameRate = Mathf.Clamp(engineSetting._fixedFrameRate, 10, 60);
#endif
            switch (engineSetting._sleepType)
            {
                case EngineSetting.eDimType.DimType_NeverSleep:
                    Screen.sleepTimeout = SleepTimeout.NeverSleep;
                    break;
                case EngineSetting.eDimType.DimType_SystemSetting:
                    Screen.sleepTimeout = SleepTimeout.SystemSetting;
                    break;
                case EngineSetting.eDimType.DimType_UserDefined:
                    Screen.sleepTimeout = Mathf.Clamp(engineSetting._sleepTimeoutUserDefined, 10, 60);
                    break;
            }

            Screen.orientation = ScreenOrientation.AutoRotation;
            switch (engineSetting._autoOrientation)
            {
                case EngineSetting.eAutoOrientation.AutoOri_Portrait:
                    Screen.autorotateToLandscapeLeft = false;
                    Screen.autorotateToLandscapeRight = false;
                    Screen.autorotateToPortrait = true;
                    Screen.autorotateToPortraitUpsideDown = true;
                    break;
                case EngineSetting.eAutoOrientation.AutoOri_Landscape:
                    Screen.autorotateToLandscapeLeft = true;
                    Screen.autorotateToLandscapeRight = true;
                    Screen.autorotateToPortrait = false;
                    Screen.autorotateToPortraitUpsideDown = false;
                    break;
            }

            Caching.CleanCache();
            Caching.maximumAvailableDiskSpace = 1024 * 1024 * Mathf.Clamp(engineSetting._cachingSpace, 10, 100);

            yield break;
        }
        
        private void UpdateFPS()
        {
            ++_FrameCount;

            float TimeNow = Time.realtimeSinceStartup;
            if( TimeNow > _LastTime + _UpdateInterval )
            {
                _FPS = (float)(_FrameCount/(TimeNow - _LastTime));
                _FrameCount = 0;
                _LastTime = TimeNow;
            }
        }

        public static float fps
        {
            get
            {
                return _FPS;
            }            
        }
        
        public static bool StartInterlude<T>(System.Object[] param) where T : Interlude
        {
            return StartInterlude(typeof(T), param);
        }

        public static bool StartInterlude(Type type, System.Object[] param)
        {
            if (instance == null)
            {
                LogManager.LogError("EngineApp::StartInterlude", "EngineApp is not inited yet!");
                return false;
            }

            if( type == null || !type.IsSubclassOf(typeof(Interlude)) )
            {
                return false;
            }

            Interlude comp = (Interlude)instance.gameObject.AddComponent(type);
            if (comp == null)
            {
                LogManager.LogError("EngineApp::StartInterlude", "Failed to create interlude [" + type.Name + "]");
                return false;
            }
            comp.Init(param);

            return true;
        }

        ///////////////// 所有关于time的封装暂时不用
        //public static float timeScale
        //{
        //    get
        //    {
        //        return Time.timeScale;
        //    }
        //    set
        //    {
        //        Time.timeScale = Mathf.Clamp(value, 0.0f, 100.0f);
        //    }
        //}

        //// 受timeScale影响的逻辑bIgnoreTimescale = false
        //// 不受timeScale影响的逻辑bIgnoreTimescale = true，例如UI、背景音乐
        //// 同一帧多次获取的值相同
        //public static float GetDeltaTime(bool bIgnoreTimescale = false)
        //{
        //    return bIgnoreTimescale ? Time.unscaledDeltaTime : Time.deltaTime;
        //}

        //// 场景加载后开始计时
        //// 同一帧多次获取的值相同
        //public static float GetTimeSinceLevelLoad(bool bIgnoreTimescale = false)
        //{
        //    return bIgnoreTimescale ? Time.unscaledTime : Time.time;
        //}

        //public static int frameCount
        //{
        //    get
        //    {
        //        return Time.frameCount;
        //    }
        //}

        //// 获得游戏启动后经过的时间，同一帧多次获取得到的时间可能相同也可能不相同,不受timeScale影响
        //public static float realTimeSinceStartup
        //{
        //    get
        //    {
        //        return Time.realtimeSinceStartup;
        //    }
        //}

        // 终止所有资源加载相关的协程
        public static void StopAssetLoadingCoroutines()
        {
            if (AssetBundleManager.instance)
                AssetBundleManager.instance.StopAllCoroutines();

            if (AssetManager.instance)
                AssetManager.instance.StopAllCoroutines();
        }
   }
}