
namespace Framework
{
    public delegate void PublishArgsEventHandler(object sender, int evt, object args);

    public interface IPublish
    {
        event PublishArgsEventHandler PublishArgs;

        void Notify(int evt, object args);
    }

    //public class PublishList : IList<IPublish>
    //{
    //    private List<IPublish> publishList = new List<IPublish>();

    //    public IPublish this[int index]
    //    {
    //        get
    //        {
    //            return publishList[index];
    //        }
    //        set
    //        {
    //            publishList[index] = value;
    //        }
    //    }

    //    public int IndexOf(IPublish item)
    //    {
    //        return publishList.IndexOf(item);
    //    }
    //    public void Insert(int index, IPublish item)
    //    {
    //        publishList.Insert(index, item);
    //    }
    //    public void RemoveAt(int index)
    //    {
    //        publishList.RemoveAt(index);
    //    }

    //    public int Count
    //    {
    //        get
    //        {
    //            return publishList.Count;
    //        }
    //    }

    //    public bool IsReadOnly
    //    {
    //        get
    //        {
    //            return false;
    //        }
    //    }

    //    public void Add(IPublish item)
    //    {
    //        publishList.Add(item);
    //    }

    //    public void Clear()
    //    {
    //        publishList.Clear();
    //    }

    //    public bool Contains(IPublish item)
    //    {
    //        return publishList.Contains(item);
    //    }

    //    public void CopyTo(IPublish[] array, int arrayIndex)
    //    {
    //        publishList.CopyTo(array, arrayIndex);
    //    }

    //    public bool Remove(IPublish item)
    //    {
    //        return publishList.Remove(item);
    //    }

    //    IEnumerator System.Collections.IEnumerable.GetEnumerator()
    //    {
    //        return publishList.GetEnumerator();
    //    }

    //    public IEnumerator<IPublish> GetEnumerator()
    //    {
    //        return publishList.GetEnumerator();
    //    }
    //}
}