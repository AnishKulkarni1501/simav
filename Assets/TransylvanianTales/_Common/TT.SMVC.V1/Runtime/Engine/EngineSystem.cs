using TT.Core.MonoBehaviourComponentSystem.V1;
using UnityEngine;

namespace TT.SMVC.V1
{
    [AddComponentMenu("Transylvanian Tales/Simple Modular Vehicle Controller/V1/Engine Module/Engine System")]
    public class EngineSystem : GenericSystemBase<EngineSystem>
    {
        public override bool PersistBetweenScenes => true;


        void FixedUpdate()
        {
            ProcessEngines();
        }


        void ProcessEngines()
        {
            foreach (var engine in Engine.Registry.Components)
            {
                ProcessEngine(engine);
            }
        }

        void ProcessEngine(Engine engine)
        {
            ProcessCutGas(engine);
            ProcessCutGasDueToSpeedLimit(engine);
            CalculateRPM(engine);
            CalculateMotorTorque(engine);
            ApplyMotorTorqueToWheels(engine);
        }

        void ProcessCutGas(Engine engine)
        {
            if (engine.gearbox.isChangingGear || engine.engineRPM >= (engine.configuration.revLimiterRPM * 1.025f))
            {
                engine.cutGas = true;
            }
            else if (!engine.gearbox.isChangingGear && engine.engineRPM < (engine.configuration.revLimiterRPM * 0.975f))
            {
                engine.cutGas = false;
            }
        }

        void ProcessCutGasDueToSpeedLimit(Engine engine)
        {
            if (engine.speedometer.speedKPH > engine.gearbox.currentGear.maxSpeed)
            {
                engine.cutGasDueToSpeedLimit = true;
            }
            else if (engine.speedometer.speedKPH <= engine.gearbox.currentGear.maxSpeed)
            {
                engine.cutGasDueToSpeedLimit = false;
            }
        }

        void CalculateRPM(Engine engine)
        {
            float NormalizedSpeedForCurrentGear = Mathf.InverseLerp(engine.gearbox.currentGear.minSpeed, engine.gearbox.currentGear.maxSpeed, engine.speedometer.speedKPH);
            float targetEngineRPM = engine.cutGas ? 0f : (engine.configuration.maxEngineRPM * NormalizedSpeedForCurrentGear);

            targetEngineRPM = Mathf.Clamp(targetEngineRPM, 0f, engine.configuration.maxEngineRPM);
            engine.velocity = 0f;

            float rawEngineRPM = Mathf.SmoothDamp(engine.engineRPM, targetEngineRPM, ref engine.velocity, engine.configuration.rpmSmoothTime);
            rawEngineRPM = Mathf.Max(rawEngineRPM, engine.configuration.idleRPM);

            engine.engineRPM = Mathf.Clamp(rawEngineRPM, 0f, engine.configuration.maxEngineRPM);
            engine.normalizedEngineRPM = engine.engineRPM / engine.configuration.maxEngineRPM;
        }

        void CalculateMotorTorque(Engine engine)
        {
            float cutGasMultiplier = engine.cutGas || engine.cutGasDueToSpeedLimit ? 0f : 1f;

            engine.motorTorque = Mathf.Clamp(engine.throttleInput, 0f, 1f) *
                engine.configuration.maxEngineTorque *
                engine.configuration.finalEngineRatio *
                engine.gearbox.currentGear.ratio *
                cutGasMultiplier;
        }

        void ApplyMotorTorqueToWheels(Engine engine)
        {
            float totalMultiplier = 0f;

            foreach (var wheel in engine.wheels)
            {
                totalMultiplier += 1f;
            }

            if (totalMultiplier == 0f) return;

            foreach (var wheel in engine.wheels)
            {
                if (wheel.wheelCollider == null) continue;

                float normalizedMultiplier = 1f / totalMultiplier;
                wheel.wheelCollider.motorTorque = engine.motorTorque * normalizedMultiplier;
            }
        }
    }
}