
namespace Framework
{
    public interface ISubscribe
    {
        void Attach(IPublish pub);

        void Detach(IPublish pub);
    }

    //public class SubscribeList : IList<ISubscribe>
    //{
    //    private List<ISubscribe> subscribeList = new List<ISubscribe>();

    //    public ISubscribe this[int index]
    //    {
    //        get
    //        {
    //            return subscribeList[index];
    //        }
    //        set
    //        {
    //            subscribeList[index] = value;
    //        }
    //    }

    //    public int IndexOf(ISubscribe item)
    //    {
    //        return subscribeList.IndexOf(item);
    //    }
    //    public void Insert(int index, ISubscribe item)
    //    {
    //        subscribeList.Insert(index, item);
    //    }
    //    public void RemoveAt(int index)
    //    {
    //        subscribeList.RemoveAt(index);
    //    }

    //    public int Count
    //    {
    //        get
    //        {
    //            return subscribeList.Count;
    //        }
    //    }

    //    public bool IsReadOnly
    //    {
    //        get
    //        {
    //            return false;
    //        }
    //    }

    //    public void Add(ISubscribe item)
    //    {
    //        subscribeList.Add(item);
    //    }

    //    public void Clear()
    //    {
    //        subscribeList.Clear();
    //    }

    //    public bool Contains(ISubscribe item)
    //    {
    //        return subscribeList.Contains(item);
    //    }

    //    public void CopyTo(ISubscribe[] array, int arrayIndex)
    //    {
    //        subscribeList.CopyTo(array, arrayIndex);
    //    }

    //    public bool Remove(ISubscribe item)
    //    {
    //        return subscribeList.Remove(item);
    //    }

    //    IEnumerator System.Collections.IEnumerable.GetEnumerator()
    //    {
    //        return subscribeList.GetEnumerator();
    //    }

    //    public IEnumerator<ISubscribe> GetEnumerator()
    //    {
    //        return subscribeList.GetEnumerator();
    //    }
    //}
}