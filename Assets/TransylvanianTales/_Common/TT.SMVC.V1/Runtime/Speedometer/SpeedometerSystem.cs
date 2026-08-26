using TT.Core.MonoBehaviourComponentSystem.V1;
using UnityEngine;

namespace TT.SMVC.V1
{
    [AddComponentMenu("Transylvanian Tales/Simple Modular Vehicle Controller/V1/Speedometer Module/Speedometer System")]
    public class SpeedometerSystem : GenericSystemBase<SpeedometerSystem>
    {
        public override bool PersistBetweenScenes => true;


        void Update()
        {
            ProcessSpeedometers();
        }


        void ProcessSpeedometers()
        {
            foreach (var speedometer in Speedometer.Registry.Components)
            {
                ProcessSpeedometer(speedometer);
            }
        }

        void ProcessSpeedometer(Speedometer speedometer)
        {
            speedometer.rigidbodySpeed = speedometer.rigidbody.linearVelocity.magnitude;
            speedometer.speedKPH = SpeedUnitsHelper.ConvertRigidbodyVelocityToKPH(speedometer.rigidbodySpeed);
            speedometer.speedMPH = SpeedUnitsHelper.ConvertRigidbodyVelocityToMPH(speedometer.rigidbodySpeed);
        }
    }
}