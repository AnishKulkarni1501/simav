using TT.Core.V1;
using UnityEngine;

namespace TT.SMVC.V1
{
    [AddComponentMenu("Transylvanian Tales/Simple Modular Vehicle Controller/V1/Speedometer Module/Speedometer")]
    public class Speedometer : VehicleComponent<Speedometer>
    {
        public new Rigidbody rigidbody;
        public SpeedometerConfiguration configuration;

        [Space]

        [ReadOnly]
        public float rigidbodySpeed;

        [ReadOnly]
        public float speedKPH;

        [ReadOnly]
        public float speedMPH;
    }
}