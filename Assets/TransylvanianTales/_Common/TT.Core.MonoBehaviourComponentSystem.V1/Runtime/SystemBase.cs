using UnityEngine;

namespace TT.Core.MonoBehaviourComponentSystem.V1
{
    [DefaultExecutionOrder(-500)]
    public abstract class SystemBase : MonoBehaviour
    {
        public abstract bool PersistBetweenScenes { get; }
    }
}