using System.Collections.Generic;
using UnityEngine;
using Framework;

namespace Framework.UI
{
    [ExecuteInEditMode]
    public abstract class UIView : ViewBase
    {
        protected List<string> _widgetPathList = new List<string>();

        protected virtual void Awake()
        {
            InitWidgets();

            foreach (string path in _widgetPathList)
            {
                Transform tran = gameObject.transform.Find(path);
                if (tran != null)
                {
                    WidgetInitialized(path, tran.gameObject);
                }
                else
                {
                    LogManager.LogWarning("LoginView:WidgetInitialized", "can not find widget from path[" + string.Concat(GlobalUtility.GetFullname(gameObject.transform), ".", path) + "]");
                }
            }
        }

        protected override void Start()
        {
            base.Start();

            SetAnchors();

            OnShow();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (_bStart)
            {
                OnShow();
            }
        }

        protected abstract void InitWidgets();
        protected abstract void WidgetInitialized(string widgetPath, GameObject widget);        
        protected abstract void OnShow();                  // 显示UI

        protected virtual void SetAnchors()
        {
            UIRect rect = GetComponent<UIRect>();
            if (rect != null && !rect.isAnchored)
            {
                Transform parent = transform.parent != null ? transform.parent : transform.root;
                rect.SetAnchor(parent.gameObject, 0, 0, 0, 0);
            }
        }
    }
}