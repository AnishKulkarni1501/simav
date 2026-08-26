using TT.Core.V1;
using UnityEngine;

namespace TT.SMVC.V1
{
    [AddComponentMenu("Transylvanian Tales/Simple Modular Vehicle Controller/V1/Engine Module/Engine")]
    public class Engine : VehicleComponent<Engine>
    {
        public Gearbox gearbox;
        public Speedometer speedometer;
        public Wheel[] wheels;
        public EngineConfiguration configuration;

        [Space]

        [ReadOnly]
        public float throttleInput;

        [ReadOnly]
        public bool cutGas;

        [ReadOnly]
        public bool cutGasDueToSpeedLimit;

        [ReadOnly]
        public float engineRPM;

        [ReadOnly]
        public float normalizedEngineRPM;

        [ReadOnly]
        public float velocity;

        [ReadOnly]
        public float motorTorque;
    }
}