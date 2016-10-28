using UnityEngine;

namespace Framework
{
    public delegate void PropertyChangedEventHandler(object sender, string propertyName);

    public interface IPropertyChanged
    {
        event PropertyChangedEventHandler PropertyChanged;

        void RaisePropertyChanged(string propertyName);
    }

    public abstract class ViewModelBase : MonoBehaviour, IPropertyChanged, ISubscribe, IPublish
    {
        // IPropertyChanged interface
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, propertyName);
            }
        }

        // IPublish interface
        public event PublishArgsEventHandler PublishArgs;

        public void Notify(int evt, object args)
        {
            if (PublishArgs != null)
                PublishArgs(this, evt, args);
        }

        // ISubscribe interface
        abstract public void eventSubscribeArgs(object sender, int evt, object args);

        public void Attach(IPublish pub)
        {
            if( pub != null )
                pub.PublishArgs += eventSubscribeArgs;
        }

        public void Detach(IPublish pub)
        {
            if( pub != null )
                pub.PublishArgs -= eventSubscribeArgs;
        }

        // MonoBehaviour
        protected virtual void OnEnable()
        {
            AttachModels();
        }

        protected virtual void OnDisable()
        {
            DetachModels();
        }

        protected abstract void AttachModels();
        protected abstract void DetachModels();
    }
}