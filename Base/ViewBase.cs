using UnityEngine;

namespace Framework
{
    public abstract class ViewBase : MonoBehaviour, ISubscribe
    {
        protected bool _bStart = false;                 // 是否已运行Start()

        protected ViewModelBase _viewModel;

        public ViewModelBase viewModel { get { return _viewModel; } }

        // ISubscribe
        abstract public void eventSubscribeArgs(object sender, int evt, object args);

        public void Attach(IPublish pub)
        {
            if (pub != null)
                pub.PublishArgs += eventSubscribeArgs;
        }

        public void Detach(IPublish pub)
        {
            if (pub != null)
                pub.PublishArgs -= eventSubscribeArgs;
        }


        // MonoBehaviour
        protected abstract ViewModelBase FindViewModel();

        protected virtual void Start()
        {
            _bStart = true;
            _viewModel = FindViewModel();
            Attach(_viewModel);
        }

        protected virtual void OnEnable()
        {
            if (_bStart)
            {
                _viewModel = FindViewModel();
                Attach(_viewModel);
            }
        }

        protected virtual void OnDisable()
        {
            Detach(_viewModel);
            _viewModel = null;
        }
    }
}