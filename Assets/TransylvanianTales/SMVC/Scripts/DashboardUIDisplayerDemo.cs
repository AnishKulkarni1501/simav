using UnityEngine;
using UnityEngine.UI;
using TT.SMVC.V1;

#if HAS_TMP
using TMPro;
#endif

namespace TT.SMVC.Demo
{
    public class DashboardUIDisplayerDemo : MonoBehaviour
    {
        public Image filledImage;
        public float filledImageOffset;

        [Space]

        public Image gradientImage;
        public Gradient gradient;

        [Space]
        public DashboardMeterNeedle needle;

#if HAS_TMP
        [Space]
        public TextMeshProUGUI gearText;
        public TextMeshProUGUI speedUnitsText;
        public TextMeshProUGUI speedText;
#endif


        void LateUpdate()
        {
            DisplayDashboard();
        }

        void DisplayDashboard()
        {
            Vehicle vehicle = VehicleRegistry.Instance.GetPlayerVehicle();
            if (vehicle == null) return;

            Engine engine = vehicle.GetVehicleComponent<Engine>();
            Speedometer speedometer = vehicle.GetVehicleComponent<Speedometer>();
            Gearbox gearbox = vehicle.GetVehicleComponent<Gearbox>();


            if (engine != null)
            {
                float normalizedRPM = Mathf.InverseLerp(0f, engine.configuration.maxEngineRPM, engine.engineRPM);

                needle.SetNormalizedValue(normalizedRPM);
                filledImage.fillAmount = Mathf.Lerp(filledImageOffset, 1f - filledImageOffset, normalizedRPM);
                gradientImage.color = gradient.Evaluate(normalizedRPM);
            }

#if HAS_TMP
            if (speedometer != null)
            {
                speedText.text = speedometer.speedKPH.ToString("0");
                speedUnitsText.text = "KM/H";
            }

            if (gearbox != null)
            {
                if (gearbox.currentGearValue > 0)
                {
                    gearText.text = gearbox.currentGearValue.ToString();
                }
                else
                {
                    gearText.text = "N";
                }
            }
#endif
        }
    }
}