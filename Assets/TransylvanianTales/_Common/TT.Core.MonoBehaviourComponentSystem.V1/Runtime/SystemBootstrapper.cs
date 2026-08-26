using UnityEngine;

namespace TT.Core.MonoBehaviourComponentSystem.V1
{
    [AddComponentMenu("Transylvanian Tales/Core/MonoBehaviour Component System/V1/System Bootstrapper")]
    [DefaultExecutionOrder(-1000)]
    public class SystemBootstrapper : MonoBehaviour
    {
        [SerializeField] private SystemBase[] systemPrefabs;


        private void Awake()
        {
            InstantiatePrefabs();
        }

        private void InstantiatePrefabs()
        {
            foreach (var prefab in systemPrefabs)
            {
                if (prefab == null) continue;

                Instantiate(prefab);
            }
        }
    }
}