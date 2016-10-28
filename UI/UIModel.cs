using Framework;

namespace Framework.UI
{
    public abstract class UIModel<T> : ModelBase<T> where T : UIModel<T>
    {
    }
}