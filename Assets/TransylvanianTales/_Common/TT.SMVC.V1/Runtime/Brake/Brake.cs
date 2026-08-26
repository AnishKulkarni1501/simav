using TT.Core.V1;
using UnityEngine;

namespace TT.SMVC.V1
{
    [AddComponentMenu("Transylvanian Tales/Simple Modular Vehicle Controller/V1/Brake Module/Brake")]
    public class Brake : VehicleComponent<Brake>
    {
        public Wheel wheel;
        public BrakeConfiguration configuration;

        [Space]

        [ReadOnly]
        public float input;
    }
}