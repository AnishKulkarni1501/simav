using UnityEngine;

namespace TT.SMVC.V1
{
    [AddComponentMenu("Transylvanian Tales/Simple Modular Vehicle Controller/V1/Dashboard Display/Dashboard Needle")]
    public class DashboardMeterNeedle : MonoBehaviour
    {
        public int minZRotation = 90;
        public int maxZRotation = -180;

        public float NormalizedValue { get; private set; }


        public void SetNormalizedValue(float normalizedValue)
        {
            NormalizedValue = normalizedValue;
            Vector3 needleEulerAngles = transform.localEulerAngles;
            needleEulerAngles.z = Mathf.Lerp(minZRotation, maxZRotation, normalizedValue);
            transform.localEulerAngles = needleEulerAngles;
        }
    }
}