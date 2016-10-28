
namespace Framework
{
    public class GameApp : SingletonMono<GameApp>
    {
        static protected bool _bInited = false;

        static public bool isInited { get { return _bInited; } }
    }
}