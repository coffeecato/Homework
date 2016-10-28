using UnityEngine;

namespace Framework
{
    public class LevelInfo : MonoBehaviour
    {
        virtual protected void Awake()
        {
            ClearScene();

            EnterScene();
        }

        virtual protected void EnterScene()
        {
        }
        
        virtual protected void ClearScene()
        {
            GlobalUtility.UnloadAll();
        }
    }
}