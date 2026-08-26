using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarsController : MonoBehaviour
{
    [Header("Wheels")]
    public WheelCollider frontLeft;
    public WheelCollider frontRight;
    public WheelCollider rearLeft;
    public WheelCollider rearRight;

    [Header("Cameras")]
    public GameObject[] cams;

    [Header("Car Settings")]
    public float motorTorque = 2000f;
    public float maxSteerAngle = 30f;
    public float brakeTorque = 4000f;

    private Rigidbody rb;
    private int currentCamera = 0;

    public float CurrentSpeed { get; private set; }
    public string CurrentMaterial { get; private set; } = "Unknown";

    public GameObject[] events;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.mass = 1400f;
        rb.centerOfMass = new Vector3(0f, -0.5f, 0f);

        // Start with the first camera
        if (cams.Length > 0)
        {
            SetCamera(0);
        }
    }

    void Update()
    {
        SetCams();
        UpdateSpeed();
        UpdateMaterial();
        UpdateFPS();
    }

    void FixedUpdate()
    {
        float motor = Input.GetAxis("Vertical") * motorTorque;
        float steer = Input.GetAxis("Horizontal") * maxSteerAngle;
        bool braking = Input.GetKey(KeyCode.Space);

        // Steering
        frontLeft.steerAngle = steer;
        frontRight.steerAngle = steer;

        // Motor
        rearLeft.motorTorque = motor;
        rearRight.motorTorque = motor;

        // Brakes
        float brake = braking ? brakeTorque : 0f;

        frontLeft.brakeTorque = brake;
        frontRight.brakeTorque = brake;
        rearLeft.brakeTorque = brake;
        rearRight.brakeTorque = brake;

        UpdateSpeed();
        UpdateMaterial();
    }

    // =========================================================
    // CAMERA
    // =========================================================

    void SetCams()
    {
        // Number keys
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetCamera(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetCamera(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetCamera(2);
        }

        // Cycle cameras with C
        if (Input.GetKeyDown(KeyCode.C))
        {
            CycleCamera();
        }
    }
    public int CollisionCount { get; private set; }

    public float CurrentFPS { get; private set; }

    private float fpsTimer = 0f;
    private int frameCount = 0;


    void UpdateFPS()
    {
        frameCount++;
        fpsTimer += Time.unscaledDeltaTime;

        if (fpsTimer >= 1f)
        {
            CurrentFPS = frameCount / fpsTimer;

            frameCount = 0;
            fpsTimer = 0f;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        CollisionCount++;
    }

    void CycleCamera()
    {
        if (cams.Length == 0)
            return;

        currentCamera++;

        if (currentCamera >= cams.Length)
        {
            currentCamera = 0;
        }

        SetCamera(currentCamera);
    }

    void SetCamera(int cameraIndex)
    {
        if (cams.Length == 0)
            return;

        if (cameraIndex < 0 || cameraIndex >= cams.Length)
            return;

        currentCamera = cameraIndex;

        for (int i = 0; i < cams.Length; i++)
        {
            cams[i].SetActive(i == cameraIndex);
        }
    }

    // =========================================================
    // SPEED
    // =========================================================

    void UpdateSpeed()
    {
        CurrentSpeed = rb.linearVelocity.magnitude * 3.6f;
    }

    // =========================================================
    // GROUND MATERIAL
    // =========================================================

    void UpdateMaterial()
    {
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
        if (!wheel.GetGroundHit(out WheelHit hit))
            return "Unknown";

        Collider groundCollider = hit.collider;

        if (groundCollider == null)
            return "Unknown";

        // Look for renderer on the collider
        Renderer renderer = groundCollider.GetComponent<Renderer>();

        // Look on parent
        if (renderer == null)
            renderer = groundCollider.GetComponentInParent<Renderer>();

        // Look in children
        if (renderer == null)
            renderer = groundCollider.GetComponentInChildren<Renderer>();

        if (renderer == null)
            return "Unknown";

        if (renderer.sharedMaterial == null)
            return "Unknown";

        return renderer.sharedMaterial.name;
    }
}
