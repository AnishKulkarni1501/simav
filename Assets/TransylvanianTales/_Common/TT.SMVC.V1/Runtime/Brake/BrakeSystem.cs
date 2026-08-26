using TT.Core.MonoBehaviourComponentSystem.V1;
using UnityEngine;

namespace TT.SMVC.V1
{
    [AddComponentMenu("Transylvanian Tales/Simple Modular Vehicle Controller/V1/Brake Module/Brake System")]
    public class BrakeSystem : GenericSystemBase<BrakeSystem>
    {
        public override bool PersistBetweenScenes => true;


        void FixedUpdate()
        {
            ProcessBrakes();
        }


        void ProcessBrakes()
        {
            foreach (var brake in Brake.Registry.Components)
            {
                ProcessBrake(brake);
            }
        }

        void ProcessBrake(Brake brake)
        {
            float brakeTorque = brake.input * brake.configuration.maxBrakeTorque;
            brake.wheel.wheelCollider.brakeTorque = brakeTorque;
        }
    }
}