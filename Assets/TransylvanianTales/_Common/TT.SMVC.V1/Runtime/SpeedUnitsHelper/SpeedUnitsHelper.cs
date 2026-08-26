
namespace TT.SMVC.V1
{
    public static class SpeedUnitsHelper
    {
        public static float ConvertRigidbodyVelocityToKPH(float rigidbodyVelocity)
        {
            return rigidbodyVelocity * 3.6f;
        }

        public static float ConvertRigidbodyVelocityToMPH(float rigidbodyVelocity)
        {
            return rigidbodyVelocity * 2.23694f;
        }


        public static float ConvertKPHToMPH(float KPH)
        {
            return KPH * 0.621371f;
        }

        public static float ConvertMPHToKPH(float MPH)
        {
            return MPH * 1.60934f;
        }
    }
}