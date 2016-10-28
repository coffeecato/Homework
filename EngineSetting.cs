using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class EngineSetting : ScriptableObject
    {
        public bool _useDefaultFrameRate;           // 是否使用平台的默认帧率
        public int _fixedFrameRate = 30;            // 使用自定义的固定帧率，仅当不使用平台默认帧率时生效

        public enum eDimType
        {
            DimType_NeverSleep,
            DimType_SystemSetting,
            DimType_UserDefined,
        };
        public eDimType _sleepType;                 // 休眠方式
        public int _sleepTimeoutUserDefined;

        public enum eAutoOrientation
        {
            AutoOri_Portrait,
            AutoOri_Landscape,
        };
        public eAutoOrientation _autoOrientation;   // 自动转向方式

        public int _cachingSpace = 50;              // 缓存空间大小（M）

        public enum ELanguage
        {
            CHN,
            ENG,
            NUM
        }
        public ELanguage _defaultLanguage = ELanguage.CHN;

        public class LogFilter
        {
            public string FilterName;
            public bool bSuppress;
        }
        public bool EnableLogUI;
        public List<LogFilter> LogFilterList = new List<LogFilter>();

        public string _defaultGameAppClass = "Framework.GameApp";
        public Type _typeOfGameApp;

        public string _defaultUISysClass = "Framework.UI.UISysManager";
        public Type _typeOfUISys;

        public string _defaultBehaviourPostInitEngine = "Framework.Interlude_LoadScene";
        public Type _typeOfBehaviourPostInitEngine;

        public void CopyFrom(EngineSetting src)
        {
            if (src == null)
                return;

            _useDefaultFrameRate = src._useDefaultFrameRate;
            _fixedFrameRate = src._fixedFrameRate;
            _sleepType = src._sleepType;
            _sleepTimeoutUserDefined = src._sleepTimeoutUserDefined;
            _autoOrientation = src._autoOrientation;
            _cachingSpace = src._cachingSpace;
            _defaultLanguage = src._defaultLanguage;
            _defaultGameAppClass = src._defaultGameAppClass;
            _defaultUISysClass = src._defaultUISysClass;
            _defaultBehaviourPostInitEngine = src._defaultBehaviourPostInitEngine;

            Resolve();
        }

        void Resolve()
        {
            _typeOfGameApp = ConvertToType(_defaultGameAppClass, "Framework.GameApp");

            _typeOfUISys = ConvertToType(_defaultUISysClass, "Framework.UISysManager");

            _typeOfBehaviourPostInitEngine = ConvertToType(_defaultBehaviourPostInitEngine, "Framework.Interlude_LoadScene");
        }

        Type ConvertToType(string arg, string defArg)
        {
            string nameSpace;
            string className;
            GlobalUtility.ParseClassName(arg, out nameSpace, out className);
            Type type = GlobalUtility.GetClassType(nameSpace, className);
            if (type == null)
            {
                LogManager.LogWarning("EngineSetting", "failed to find the class type["+arg+"]");

                GlobalUtility.ParseClassName(defArg, out nameSpace, out className);
                type = GlobalUtility.GetClassType(nameSpace, className);
                if (type == null)
                {
                    LogManager.LogWarning("EngineSetting", "failed to find the DEFAULT class type["+defArg+"]");
                }
            }
            return type;
        }
    }
}