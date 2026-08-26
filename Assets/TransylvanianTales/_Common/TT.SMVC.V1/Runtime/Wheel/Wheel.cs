using UnityEngine;

namespace TT.SMVC.V1
{
    [AddComponentMenu("Transylvanian Tales/Simple Modular Vehicle Controller/V1/Wheel Module/Wheel")]
    public class Wheel : VehicleComponent<Wheel>
    {
        public Transform model;
        public WheelCollider wheelCollider;
        public WheelConfiguration configuration;
    }
}