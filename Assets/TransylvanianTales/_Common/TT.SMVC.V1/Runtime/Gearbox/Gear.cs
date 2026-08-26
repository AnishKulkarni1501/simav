using UnityEngine;

namespace TT.SMVC.V1
{
    [System.Serializable]
    public struct Gear
    {
        public float ratio;

        [Space]

        public float minSpeed;
        public float maxSpeed;

        [Space]

        public float targetSpeedForPreviousGear;
        public float targetSpeedForNextGear;
    }
}