using TT.Core.MonoBehaviourComponentSystem.V1;
using UnityEngine;

namespace TT.SMVC.V1
{
    [AddComponentMenu("Transylvanian Tales/Simple Modular Vehicle Controller/V1/Wheel Module/Wheel System")]
    public class WheelSystem : GenericSystemBase<WheelSystem>
    {
        public override bool PersistBetweenScenes => true;


        void LateUpdate()
        {
            ProcessWheels();
        }


        void ProcessWheels()
        {
            foreach (var wheel in Wheel.Registry.Components)
            {
                ProcessWheel(wheel);
            }
        }

        void ProcessWheel(Wheel wheel)
        {
            UpdateWheelModel(wheel);
        }

        void UpdateWheelModel(Wheel wheel)
        {
            wheel.wheelCollider.GetWorldPose(out Vector3 position, out Quaternion rotation);
            wheel.model.SetPositionAndRotation(position, rotation);
        }
    }
}