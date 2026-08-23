using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarsController : MonoBehaviour
{
    public WheelCollider frontLeft;
    public WheelCollider frontRight;
    public WheelCollider rearLeft;
    public WheelCollider rearRight;

    public float motorTorque = 2000f;
    public float maxSteerAngle = 30f;
    public float brakeTorque = 4000f;

    private Rigidbody rb;

    public float CurrentSpeed { get; private set; }
    public string CurrentMaterial { get; private set; } = "Unknown";

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = 1400f;
        rb.centerOfMass = new Vector3(0f, -0.5f, 0f);
    }

    void LateUpdate()
    {
        float motor = Input.GetAxis("Vertical") * motorTorque;
        float steer = Input.GetAxis("Horizontal") * maxSteerAngle;
        bool braking = Input.GetKey(KeyCode.Space);

        frontLeft.steerAngle = steer;
        frontRight.steerAngle = steer;

        rearLeft.motorTorque = motor;
        rearRight.motorTorque = motor;

        float brake = braking ? brakeTorque : 0f;

        frontLeft.brakeTorque = brake;
        frontRight.brakeTorque = brake;
        rearLeft.brakeTorque = brake;
        rearRight.brakeTorque = brake;

        UpdateSpeed();
        UpdateMaterial();
    }

    void UpdateSpeed()
    {
        CurrentSpeed = rb.linearVelocity.magnitude * 3.6f;
    }

    void UpdateMaterial()
    {
        // Check all four wheels
        string material = GetWheelMaterial(frontLeft);

        if (material == "Unknown")
            material = GetWheelMaterial(frontRight);

        if (material == "Unknown")
            material = GetWheelMaterial(rearLeft);

        if (material == "Unknown")
            material = GetWheelMaterial(rearRight);

        CurrentMaterial = material;
    }

    string GetWheelMaterial(WheelCollider wheel)
    {
        if (wheel.GetGroundHit(out WheelHit hit))
        {
            Collider groundCollider = hit.collider;

            if (groundCollider == null)
                return "Unknown";

            Renderer renderer = groundCollider.GetComponent<Renderer>();

            if (renderer == null)
                renderer = groundCollider.GetComponentInParent<Renderer>();

            if (renderer != null)
            {
                Material material = renderer.sharedMaterial;

                if (material != null)
                    return material.name;
            }
        }

        return "Unknown";
    }
}