using System.Collections;
using System.Threading;
using UnityEngine;

namespace Framework
{
    public class YieldThread : IEnumerator
    {
        public delegate void YieldThreadCallBack (object param);
        private YieldThreadCallBack _callback = null;
        private object _param = null;
        private bool _bRunning = true;

        public YieldThread(YieldThreadCallBack cb, object param = null)
        {
            _callback = cb;
            _param = param;

            Thread thread = new Thread(DoWork);
            if (thread == null)
            {
                _bRunning = false;
                return;
            }

            thread.IsBackground = true;
            thread.Start();
        }

        private void DoWork()
        {
            if (_callback != null)
                _callback(_param);

            _param = null;
            _bRunning = false;
        }

        public object Current
        {
            get { return null; }
        }

        public bool MoveNext()
        {
            return _bRunning;
        }

        public void Reset()
        {
        }
    }
}
