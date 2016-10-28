
namespace Framework
{
    public abstract class ModelBase<T> : Singleton<T>, IPublish where T : Singleton<T>
    {
        public event PublishArgsEventHandler PublishArgs;

        public void Notify(int evt, object args)
        {
            if (PublishArgs != null)
                PublishArgs(this, evt, args);
        }
    }
}