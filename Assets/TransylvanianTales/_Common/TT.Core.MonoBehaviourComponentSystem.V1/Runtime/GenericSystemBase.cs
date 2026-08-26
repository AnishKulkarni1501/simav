using UnityEngine;

namespace TT.Core.MonoBehaviourComponentSystem.V1
{
    public abstract class GenericSystemBase<T> : SystemBase where T : Component
    {
        public static T Instance { get; private set; }


        public virtual void Awake()
        {
            if (Instance != null && Instance != this as T)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this as T;

            if (PersistBetweenScenes)
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        public virtual void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}