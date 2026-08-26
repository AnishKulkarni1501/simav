using UnityEngine;

namespace TT.SMVC.V1
{
    [CreateAssetMenu(menuName = "Transylvanian Tales/Simple Modular Vehicle Controller/V1/Brake Module/Brake Configuration")]
    public class BrakeConfiguration : ScriptableObject
    {
        public float maxBrakeTorque = 5000f;
    }
}