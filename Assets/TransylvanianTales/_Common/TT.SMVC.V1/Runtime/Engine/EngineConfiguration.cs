using UnityEngine;

namespace TT.SMVC.V1
{
    [CreateAssetMenu(menuName = "Transylvanian Tales/Simple Modular Vehicle Controller/V1/Engine Module/Engine Configuration")]
    public class EngineConfiguration : ScriptableObject
    {
        public float maxEngineTorque = 500f;
        public float finalEngineRatio = 2f;

        [Space]

        public float idleRPM = 1000f;
        public float revLimiterRPM = 6500f;
        public float maxEngineRPM = 7000f;

        [Space]

        public float rpmSmoothTime = 0.15f;
    }
}