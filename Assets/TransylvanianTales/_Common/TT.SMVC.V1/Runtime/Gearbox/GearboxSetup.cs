using System;
using UnityEngine;

namespace TT.SMVC.V1
{
    [CreateAssetMenu(menuName = "Transylvanian Tales/Simple Modular Vehicle Controller/V1/Gearbox Module/Gearbox Setup")]
    public class GearboxSetup : ScriptableObject
    {
        public GearboxConfiguration gearbox;
        public SpeedometerConfiguration speedometer;

        [Header("Adjustments")]
        public int gearCount = 6;
        public float firstGearRatio = 3.5f;
        public float topGearRatio = 0.85f;

        [Space]

        public float gearOverlap = 0.2f;
        public float upShiftPoint = 0.85f;
        public float downShiftPoint = 0.1f;

        [Header("Ratios(automatically computed from adjustments)")]
        public float[] gearRatios;
        public float[] gearMinSpeedPercentages;
        public float[] gearMaxSpeedPercentages;
        public float[] targetSpeedForNextGearPercentages;
        public float[] targetSpeedForPreviousGearPercentages;


        public void ComputeValues()
        {
            ComputeGearRatios();
            ComputeSpeedLimitsPercentages();
            ComputeShiftSpeedPercentages();
        }

        void ComputeGearRatios()
        {
            float[] ratios = new float[gearCount];
            double factor = Math.Pow(topGearRatio / firstGearRatio, 1.0 / (gearCount - 1));

            for (int i = 0; i < gearCount; i++)
            {
                ratios[i] = (float)(firstGearRatio * Math.Pow(factor, i));
            }

            gearRatios = ratios;
        }

        void ComputeSpeedLimitsPercentages()
        {
            float totalInverse = 0f;

            for (int i = 0; i < gearCount; i++)
            {
                totalInverse += 1f / gearRatios[i];
            }

            float[] minSpeeds = new float[gearCount];
            float[] maxSpeeds = new float[gearCount];

            float current = 0f;

            for (int i = 0; i < gearCount; i++)
            {
                float ratioPortion = (1f / gearRatios[i]) / totalInverse;

                float next = current + ratioPortion;

                minSpeeds[i] = Mathf.Max(0f, current - ratioPortion * gearOverlap);
                maxSpeeds[i] = Mathf.Min(1f, next + ratioPortion * gearOverlap);

                current = next;
            }

            gearMinSpeedPercentages = minSpeeds;
            gearMaxSpeedPercentages = maxSpeeds;
        }

        void ComputeShiftSpeedPercentages()
        {
            float[] targetUp = new float[gearCount];
            float[] targetDown = new float[gearCount];

            for (int i = 0; i < gearCount; i++)
            {
                float min = gearMinSpeedPercentages[i];
                float max = gearMaxSpeedPercentages[i];

                targetUp[i] = Mathf.Lerp(min, max, upShiftPoint);
                targetDown[i] = Mathf.Lerp(min, max, downShiftPoint);
            }

            targetDown[0] = 0f;
            targetUp[gearCount - 1] = 1f;

            targetSpeedForNextGearPercentages = targetUp;
            targetSpeedForPreviousGearPercentages = targetDown;
        }


        public void SetupGearbox()
        {
            SetupGearbox(gearbox, speedometer);
        }

        void SetupGearbox(GearboxConfiguration gearboxData, SpeedometerConfiguration speedometerData)
        {
            SetupDGears(gearboxData, speedometerData);
            SetupNGear(gearboxData, speedometerData);
            SetupRGear(gearboxData, speedometerData);
        }

        void SetupDGears(GearboxConfiguration gearboxData, SpeedometerConfiguration speedometerData)
        {
            Gear[] gears = new Gear[gearRatios.Length];

            for (int i = 0; i < gears.Length; i++)
            {
                float minSpeedForGear = speedometerData.maxSpeedKPH * gearMinSpeedPercentages[i];
                float maxSpeedForGear = speedometerData.maxSpeedKPH * gearMaxSpeedPercentages[i];
                float targetSpeedForNextGear = speedometerData.maxSpeedKPH * targetSpeedForNextGearPercentages[i];
                float targetSpeedForPreviousGear = speedometerData.maxSpeedKPH * targetSpeedForPreviousGearPercentages[i];

                gears[i] = new Gear
                {
                    ratio = gearRatios[i],
                    minSpeed = Mathf.RoundToInt(minSpeedForGear),
                    maxSpeed = Mathf.RoundToInt(maxSpeedForGear),
                    targetSpeedForNextGear = Mathf.RoundToInt(targetSpeedForNextGear),
                    targetSpeedForPreviousGear = Mathf.RoundToInt(targetSpeedForPreviousGear)
                };
            }

            gearboxData.DGears = gears;
        }

        void SetupRGear(GearboxConfiguration gearboxData, SpeedometerConfiguration speedometerData)
        {
            Gear RGear = new Gear
            {
                ratio = -gearRatios[0],
                maxSpeed = Mathf.RoundToInt(speedometerData.maxSpeedKPH * gearMaxSpeedPercentages[0]),
                minSpeed = 0,
                targetSpeedForNextGear = 0,
                targetSpeedForPreviousGear = 0
            };

            gearboxData.RGear = RGear;
        }

        void SetupNGear(GearboxConfiguration gearboxData, SpeedometerConfiguration speedometerData)
        {
            Gear NGear = new Gear
            {
                ratio = 0f,
                maxSpeed = 0,
                minSpeed = 0,
                targetSpeedForNextGear = 0,
                targetSpeedForPreviousGear = 0
            };

            gearboxData.NGear = NGear;
        }
    }
}