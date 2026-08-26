using UnityEngine;

namespace TT.SMVC.V1
{
    [CreateAssetMenu(menuName = "Transylvanian Tales/Simple Modular Vehicle Controller/V1/Steering Mechanism Module/Steering Mechanism Configuration")]
    public class SteeringMechanismConfiguration : ScriptableObject
    {
        public float maxSteerAngle = 45f;
    }
}