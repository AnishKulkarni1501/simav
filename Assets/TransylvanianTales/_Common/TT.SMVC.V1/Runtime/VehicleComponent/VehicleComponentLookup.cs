using System.Collections.Generic;

namespace TT.SMVC.V1
{
    public static class VehicleLookup
    {
        public static List<T> GetVehicleComponents<T>(this Vehicle vehicle) where T : VehicleComponent<T>
        {
            return VehicleComponent<T>.GetComponentsForVehicle(vehicle);
        }

        public static T GetVehicleComponent<T>(this Vehicle vehicle) where T : VehicleComponent<T>
        {
            return VehicleComponent<T>.GetComponentForVehicle(vehicle);
        }
    }
}