using UnityEngine;

namespace GRV.ToolsModule
{
    public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; protected set; }

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                DontDestroyOnLoad(gameObject);
                Instance = this as T;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}