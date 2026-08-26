using UnityEngine;

namespace TT.SMVC.V1
{
    [CreateAssetMenu(menuName = "Transylvanian Tales/Simple Modular Vehicle Controller/V1/Gearbox Module/Gearbox Configuration")]
    public class GearboxConfiguration : ScriptableObject
    {
        public Gear[] DGears;
        public Gear NGear;
        public Gear RGear;

        [Space]

        public int shiftDownRPM = 1400;
        public int shiftUpRPM = 5000;

        [Space]

        public int currentGearInAwake = 1;
        public float gearShiftTime = 0.35f;

        [Space]

        public bool allowShiftIntoSameGear = true;
        public bool stopCurrentShiftWhenNewShift = true;
    }
}