using System.Collections.Generic;
using TT.Core.MonoBehaviourComponentSystem.V1;
using UnityEngine;

namespace TT.SMVC.V1
{
    public abstract class VehicleComponent<T> : ComponentBase<T> where T : VehicleComponent<T>
    {
        public Vehicle Vehicle { get; private set; }
        private static readonly Dictionary<Vehicle, List<T>> vehicleComponentsLookup = new();


        protected virtual void Awake()
        {
            Vehicle = GetComponentInParent<Vehicle>();

#if UNITY_EDITOR
            if (Vehicle == null)
            {

                Debug.LogError($"{GetType().Name} must be a child of a Vehicle", this);
            }
#endif
        }

        public override void OnEnable()
        {
            base.OnEnable();
            CacheComponent();
        }

        public override void OnDisable()
        {
            base.OnDisable();
            RemoveComponentFromCache();
        }


        void CacheComponent()
        {
            if (Vehicle == null) return;

            if (!vehicleComponentsLookup.TryGetValue(Vehicle, out var list))
            {
                list = new List<T>();
                vehicleComponentsLookup[Vehicle] = list;
            }

            list.Add((T)this);
        }

        void RemoveComponentFromCache()
        {
            if (Vehicle == null) return;

            if (vehicleComponentsLookup.TryGetValue(Vehicle, out var list))
            {
                list.Remove((T)this);

                if (list.Count == 0)
                {
                    vehicleComponentsLookup.Remove(Vehicle);
                }
            }
        }


        public static List<T> GetComponentsForVehicle(Vehicle vehicle)
        {
            if (vehicle == null)
            {
                return null;
            }

            if (vehicleComponentsLookup.TryGetValue(vehicle, out var list))
            {
                return list;
            }

            return null;
        }

        public static T GetComponentForVehicle(Vehicle vehicle)
        {
            if (vehicle == null)
            {
                return null;
            }

            var list = GetComponentsForVehicle(vehicle);
            return (list != null && list.Count > 0) ? list[0] : null;
        }
    }
}