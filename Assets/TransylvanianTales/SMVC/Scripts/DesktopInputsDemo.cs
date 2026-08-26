using System.Collections.Generic;
using UnityEngine;
using TT.SMVC.V1;

#if HAS_INPUT_SYSTEM && ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace TT.SMVC.Demo
{
    public class DesktopInputsDemo : MonoBehaviour
    {
        public float throttleSpeed = 2f;
        public float brakeSpeed = 2f;
        public float steerSpeed = 1.5f;


        void Awake()
        {
#if HAS_INPUT_SYSTEM && ENABLE_INPUT_SYSTEM
            InitializeInputSystem();
#endif
        }

        void FixedUpdate()
        {
            Process();
        }


        bool IsForwardPressed()
        {
#if HAS_INPUT_SYSTEM && ENABLE_INPUT_SYSTEM
            return Keyboard.current != null && (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed);
#else
            return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
#endif
        }

        bool IsBackwardPressed()
        {
#if HAS_INPUT_SYSTEM && ENABLE_INPUT_SYSTEM
            return Keyboard.current != null && (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed);
#else
            return Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
#endif
        }

        bool IsRightPressed()
        {
#if HAS_INPUT_SYSTEM && ENABLE_INPUT_SYSTEM
            return Keyboard.current != null && (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed);
#else
            return Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
#endif
        }

        bool IsLeftPressed()
        {
#if HAS_INPUT_SYSTEM && ENABLE_INPUT_SYSTEM
            return Keyboard.current != null && (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed);
#else
            return Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
#endif
        }


#if HAS_INPUT_SYSTEM && ENABLE_INPUT_SYSTEM
        void InitializeInputSystem()
        {
            if (Keyboard.current != null)
            {
                InputSystem.EnableDevice(Keyboard.current);
            }
        }
#endif

        void Process()
        {
            Vehicle playerVehicle = VehicleRegistry.Instance.GetPlayerVehicle();
            if (playerVehicle == null) return;

            ApplyThrottle(playerVehicle);
            ApplyBrake(playerVehicle);
            ApplySteer(playerVehicle);
        }


        void ApplyThrottle(Vehicle vehicle)
        {
            if (vehicle == null) return;

            Engine engine =  vehicle.GetVehicleComponent<Engine>();
            if (engine == null) return;

            float target = IsForwardPressed() ? 1f : 0f;
            engine.throttleInput = Mathf.MoveTowards(engine.throttleInput, target, Time.deltaTime * throttleSpeed);
        }

        void ApplyBrake(Vehicle vehicle)
        {
            if (vehicle == null) return;

            List<Brake> brakes = vehicle.GetVehicleComponents<Brake>();
            if (brakes == null) return;

            float target = IsBackwardPressed() ? 1f : 0f;

            foreach (var brake in brakes)
            {
                brake.input = Mathf.MoveTowards(brake.input, target, Time.deltaTime * brakeSpeed);
            }
        }

        void ApplySteer(Vehicle vehicle)
        {
            if (vehicle == null) return;

            SteeringMechanism steeringMechanism = vehicle.GetVehicleComponent<SteeringMechanism>();
            if (steeringMechanism == null) return;

            float target = IsRightPressed() ? 1f : IsLeftPressed() ? -1f : 0f;
            steeringMechanism.input = Mathf.Lerp(steeringMechanism.input, target, Time.deltaTime * steerSpeed);
        }
    }
}