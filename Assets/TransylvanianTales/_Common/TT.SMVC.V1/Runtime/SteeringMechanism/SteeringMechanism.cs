using TT.Core.V1;
using UnityEngine;

namespace TT.SMVC.V1
{
    [AddComponentMenu("Transylvanian Tales/Simple Modular Vehicle Controller/V1/Steering Mechanism Module/Steering Mechanism")]
    public class SteeringMechanism : VehicleComponent<SteeringMechanism>
    {
        public Wheel[] wheels;
        public SteeringMechanismConfiguration configuration;

        [Space]

        [ReadOnly]
        public float input;
    }
}