using UnityEngine;

namespace TT.SMVC.V1
{
    [AddComponentMenu("Transylvanian Tales/Simple Modular Vehicle Controller/V1/Cameras/Chase Camera")]
    public class ChaseCamera : MonoBehaviour
    {
        [Header("Target")]
        public Transform target;

        [Header("Position & Rotation")]
        public float followSpeed = 10f;
        public float rotationSpeed = 10f;
        public float maxDistance = 1f;

        private Vector3 initialLocalPosition;
        private Quaternion initialLocalRotation;
        private float currentYaw;


        void Awake()
        {
            Initialize();
        }

        void FixedUpdate()
        {
            UpdateCamera();
        }


        private void Initialize()
        {
            if (!target) return;

            initialLocalPosition = target.InverseTransformPoint(transform.position);
            initialLocalRotation = Quaternion.Inverse(target.rotation) * transform.rotation;
            currentYaw = transform.eulerAngles.y;
        }

        void UpdateCamera()
        {
            if (!target) return;

            Vector3 idealPosition = target.TransformPoint(initialLocalPosition);
            Vector3 nextPosition = Vector3.Lerp(transform.position, idealPosition, followSpeed * Time.deltaTime);
            Vector3 vectorToIdeal = nextPosition - idealPosition;

            if (vectorToIdeal.magnitude > maxDistance)
            {
                nextPosition = idealPosition + (vectorToIdeal.normalized * maxDistance);
            }

            transform.position = nextPosition;

            Vector3 lookDirection = (target.position - transform.position);
            lookDirection.y = 0f;

            if (lookDirection.sqrMagnitude > 0.001f)
            {
                float targetYaw = Mathf.Atan2(lookDirection.x, lookDirection.z) * Mathf.Rad2Deg;

                currentYaw = Mathf.LerpAngle(currentYaw, targetYaw, rotationSpeed * Time.deltaTime);

                Quaternion targetRot = target.rotation * initialLocalRotation;
                transform.rotation = Quaternion.Euler(targetRot.eulerAngles.x, currentYaw, targetRot.eulerAngles.z);
            }
        }
    }
}