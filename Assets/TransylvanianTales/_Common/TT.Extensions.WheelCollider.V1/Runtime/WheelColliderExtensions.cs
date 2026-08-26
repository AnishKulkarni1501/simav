using UnityEngine;

namespace TT.Extensions.V1
{
    public static class WheelColliderExtensions
    {
        public static void Setup(WheelCollider wheelCollider, Renderer wheelRenderer)
        {
            Bounds bounds = wheelRenderer.bounds;

            float worldRadius = bounds.extents.y;

            Vector3 lossyScale = wheelCollider.transform.lossyScale;
            float localRadius = worldRadius / Mathf.Max(lossyScale.y, 0.0001f);

            wheelCollider.radius = localRadius;

            Transform parent = wheelCollider.transform.parent;

            if (parent != null)
            {
                Vector3 localCenter = parent.InverseTransformPoint(bounds.center);
                wheelCollider.transform.localPosition = localCenter;
            }
            else
            {
                wheelCollider.transform.position = bounds.center;
            }
        }
    }
}