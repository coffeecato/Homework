using UnityEngine;
using Framework;

namespace Framework.UI
{
    [ExecuteInEditMode]
    public abstract class UIViewModel : ViewModelBase
    {
        static int _id = -1;

        public static int id { get { return _id; } set { _id = value; } }

        protected override void OnEnable()
        {
            base.OnEnable();

            id = UISysManager.instance.Register(this);
        }

        protected override void OnDisable()
        {
            // ugly code!!!!
            // 销毁时会调用OnDisable，且时序不确定，可能UISysManager已被销毁，故要判断UISysManager的有效性
            if( UISysManager.instance != null )
                UISysManager.instance.Unregister(this);

            base.OnDisable();
        }
    }
}