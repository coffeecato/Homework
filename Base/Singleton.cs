using System;

namespace Framework
{
    public abstract class Singleton<T> where T : Singleton<T>
    {
        private static T _instance = null;
        public static T instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (T)System.Activator.CreateInstance(typeof(T));
                    _instance.Init();
                }
                return _instance;
            }
        }

        public virtual void Init() {}
    }
}