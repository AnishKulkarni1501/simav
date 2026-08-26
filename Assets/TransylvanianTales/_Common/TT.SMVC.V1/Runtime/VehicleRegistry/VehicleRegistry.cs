using System.Collections.Generic;
using TT.Core.MonoBehaviourComponentSystem.V1;
using UnityEngine;
using UnityEngine.Events;

namespace TT.SMVC.V1
{
    [AddComponentMenu("Transylvanian Tales/Simple Modular Vehicle Controller/V1/Vehicle/Vehicle Registry")]
    [DefaultExecutionOrder(-999)]
    public class VehicleRegistry : GenericSystemBase<VehicleRegistry>
    {
        public override bool PersistBetweenScenes => true;

        private Vehicle playerVehicle;

        public UnityAction OnPlayerVehicleChanged;


        public IEnumerable<Vehicle> GetActiveVehicles()
        {
            return Vehicle.Registry.Components;
        }


        public void SetPlayerVehicle(Vehicle vehicle, bool notify = true)
        {
            playerVehicle = vehicle;

            if (notify)
            {
                OnPlayerVehicleChanged?.Invoke();
            }
        }

        public void ClearPlayerVehicle(bool notify = true)
        {
            playerVehicle = null;

            if (notify)
            {
                OnPlayerVehicleChanged?.Invoke();
            }
        }


        public Vehicle GetPlayerVehicle()
        {
            if (playerVehicle != null)
            {
                return playerVehicle;
            }

            var vehicles = Vehicle.Registry.Components;

            if (vehicles == null)
            {
                return null;
            }

            using var enumerator = vehicles.GetEnumerator();

            if (!enumerator.MoveNext())
            {
                return null;
            }
                
            var single = enumerator.Current;

            if (enumerator.MoveNext())
            {
                return null;
            }

            return single;
        }
    }
}