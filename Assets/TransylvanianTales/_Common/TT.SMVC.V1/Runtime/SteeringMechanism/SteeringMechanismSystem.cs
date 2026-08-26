using TT.Core.MonoBehaviourComponentSystem.V1;
using UnityEngine;

namespace TT.SMVC.V1
{
    [AddComponentMenu("Transylvanian Tales/Simple Modular Vehicle Controller/V1/Steering Mechanism Module/Steering Mechanism System")]
    public class SteeringMechanismSystem : GenericSystemBase<SteeringMechanismSystem>
    {
        public override bool PersistBetweenScenes => true;


        void FixedUpdate()
        {
            ProcessSteeringMechanisms();
        }


        void ProcessSteeringMechanisms()
        {
            foreach (var steeringMechanism in SteeringMechanism.Registry.Components)
            {
                ProcessSteeringMeschanism(steeringMechanism);
            }
        }

        void ProcessSteeringMeschanism(SteeringMechanism steeringMechanism)
        {
            float steerAngle = steeringMechanism.input * steeringMechanism.configuration.maxSteerAngle;

            foreach (var wheel in steeringMechanism.wheels)
            {
                wheel.wheelCollider.steerAngle = steerAngle;
            }
        }
    }
}