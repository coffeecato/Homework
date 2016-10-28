using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class LogManager : SingletonMono<LogManager>
    {
        private struct UILogMsg
        {
            public string msg;
            public LogType type;

            public UILogMsg(string str, LogType t)
            {
                msg = str;
                type = t;
            }
        };
        private static List<UILogMsg> _uiMsgList = new List<UILogMsg>();
        private static bool _bEnableLogUI = true;
        private const int _uiMsgCountMax = 10;

        private static List<string> _tagFilters = new List<string>();
        private static StreamWriter _logFile;

        private static string FormatMsg(string strLevel, string strTag, string strMsg)
        {
            int index = _tagFilters.IndexOf(strTag);
            if (index != -1)
                return "";

            return string.Format("[{0}:{1:N2}][{2}][{3}]{4}", Time.frameCount, Time.realtimeSinceStartup, strLevel, strTag, strMsg);
        }

        // 各种等级的Log方法
        public static void LogDebug(string strTag, string strMsg)
        {
            string strFinalMsg = FormatMsg("Debug", strTag, strMsg);
            if (!string.IsNullOrEmpty(strFinalMsg))
                Debug.Log(strFinalMsg);
        }

        public static void LogError(string strTag, string strMsg)
        {
            string strFinalMsg = FormatMsg("Error", strTag, strMsg);
            if (!string.IsNullOrEmpty(strFinalMsg))
                Debug.LogError(strFinalMsg);
        }

        public static void LogWarning(string strTag, string strMsg)
        {
            string strFinalMsg = FormatMsg("Warning", strTag, strMsg);
            if (!string.IsNullOrEmpty(strFinalMsg))
                Debug.LogWarning(strFinalMsg);
        }

        // 添加Tag过滤项
        public static void AddTagFilter(string strTag)
        {
            int index = _tagFilters.IndexOf(strTag);
            if (index != -1)
                return;

            _tagFilters.Add(strTag);
        }

        // 移除Tag过滤项
        public static void RemoveTagFilter(string strTag)
        {
            _tagFilters.Remove(strTag);
        }

        // 是否在UI上显示Log
        public static void EnableLogUI(bool bEnable)
        {
            _bEnableLogUI = bEnable;
        }

        //=====================
        // 以下方法不提供手动调用
        //=====================
        override public IEnumerator InitAysnc()
        {
            _Init();
            yield break;
        }

        override public void Init()
        {
            _Init();
        }

        private void _Init()
        {
            if (_logFile == null)
            {
                string path = Application.persistentDataPath + "/Unity_log.txt";
                _logFile = new StreamWriter(path, false, Encoding.Unicode, 4096);
            }
            Application.logMessageReceived += HandleLog;

            // init log filter from global config
            foreach (EngineSetting.LogFilter filter in EngineApp.instance.engineSetting.LogFilterList)
            {
                if (filter.bSuppress && !string.IsNullOrEmpty(filter.FilterName))
                {
                    AddTagFilter(filter.FilterName);
                }
            }

            _bEnableLogUI = EngineApp.instance.engineSetting.EnableLogUI;
        }

        void OnDestroy()
        {
            Application.logMessageReceived -= HandleLog;
            if (_logFile != null)
            {
                _logFile.Flush();
                _logFile.Close();
                _logFile = null;
            }
        }

        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            string finalMsg = logString;
            if (type != LogType.Warning && type != LogType.Log)
            {
                finalMsg += "\n\t";
                finalMsg += stackTrace.Replace("\n", "\n\t");
            }

            // 输出到log.tx
            if (_logFile != null)
            {
                _logFile.WriteLine(finalMsg);
            }

            // 输出到游戏UI
            if (_uiMsgList.Count >= _uiMsgCountMax)
                _uiMsgList.RemoveAt(0);
            _uiMsgList.Add(new UILogMsg(finalMsg, type));
        }

        void OnGUI()
        {
            GUI.color = Color.red;
            GUILayout.Label(EngineApp.fps.ToString());

            if (!_bEnableLogUI)
                return;

            for (int i = _uiMsgList.Count - 1; i >= 0; --i)
            {
                UILogMsg temp = _uiMsgList[i];

                Color curColor;
                switch (temp.type)
                {
                    case LogType.Assert:
                    case LogType.Error:
                    case LogType.Exception:
                        curColor = Color.red;
                        break;
                    case LogType.Warning:
                        curColor = Color.yellow;
                        break;
                    default:
                        curColor = Color.white;
                        break;
                }

                if (GUI.color != curColor)
                    GUI.color = curColor;

                GUILayout.Label(temp.msg);
            }
        }
    }
}
