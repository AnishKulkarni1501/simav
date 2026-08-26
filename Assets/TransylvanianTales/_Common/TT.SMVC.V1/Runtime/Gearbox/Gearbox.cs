using TT.Core.V1;
using UnityEngine;

namespace TT.SMVC.V1
{
    [AddComponentMenu("Transylvanian Tales/Simple Modular Vehicle Controller/V1/Gearbox Module/Gearbox")]
    public class Gearbox : VehicleComponent<Gearbox>
    {
        public Engine engine;
        public Speedometer speedometer;
        public GearboxConfiguration configuration;

        [Space]

        [ReadOnly]
        public int currentGearValue;

        [ReadOnly]
        public Gear currentGear;

        [ReadOnly]
        public bool isChangingGear;

        [ReadOnly]
        public WaitForSeconds waitForGearShiftTime;

        [ReadOnly]
        public Coroutine changeGearCoroutine;
    }
}