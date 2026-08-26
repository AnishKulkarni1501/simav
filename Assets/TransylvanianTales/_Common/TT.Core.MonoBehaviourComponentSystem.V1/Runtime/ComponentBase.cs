using UnityEngine;

namespace TT.Core.MonoBehaviourComponentSystem.V1
{
    public abstract class ComponentBase<TComponent> : MonoBehaviour where TComponent : ComponentBase<TComponent>
    {
        private static readonly ComponentRegistry<TComponent> registry = new();
        public static ComponentRegistry<TComponent> Registry => registry;

        [SerializeField]
        private bool autoRegister = true;


        public virtual void OnEnable()
        {
            if (autoRegister)
            {
                registry.Register((TComponent)this);
            }
        }

        public virtual void OnDisable()
        {
            if (autoRegister)
            {
                registry.Unregister((TComponent)this);
            }
        }
    }
}