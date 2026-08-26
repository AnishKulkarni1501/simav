using UnityEngine;

namespace TT.SMVC.V1
{
    [CreateAssetMenu(menuName = "Transylvanian Tales/Simple Modular Vehicle Controller/V1/Speedometer Module/Speedometer Configuration")]
    public class SpeedometerConfiguration : ScriptableObject
    {
        public float maxSpeedKPH = 240f;
    }
}