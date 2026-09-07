using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarsController : MonoBehaviour
{
    // =========================================================
    // CONTROL MODE
    // =========================================================

    public enum ControlMode
    {
        Manual,
        Autonomous
    }

    [Header("Control")]
    public ControlMode controlMode = ControlMode.Manual;

    // =========================================================
    // WHEELS
    // =========================================================

    [Header("Wheels")]
    public WheelCollider frontLeft;
    public WheelCollider frontRight;
    public WheelCollider rearLeft;
    public WheelCollider rearRight;

    // =========================================================
    // CAMERAS
    // =========================================================

    [Header("Cameras")]
    public GameObject[] cams;

    // =========================================================
    // CAR SETTINGS
    // =========================================================

    [Header("Car Settings")]
    public float motorTorque = 2000f;
    public float maxSteerAngle = 30f;
    public float brakeTorque = 4000f;

    // =========================================================
    // RIGIDBODY
    // =========================================================

    private Rigidbody rb;
    private int currentCamera = 0;

    // =========================================================
    // TELEMETRY
    // =========================================================

    public float CurrentSpeed { get; private set; }

    public string CurrentMaterial { get; private set; } = "Unknown";

    public int CollisionCount { get; private set; }

    public float CurrentFPS { get; private set; }

    // =========================================================
    // AUTONOMOUS CONTROL VALUES
    // =========================================================

    private float autonomousSteering = 0f;

    private float autonomousThrottle = 0f;

    private float autonomousBrake = 0f;

    // =========================================================
    // FPS
    // =========================================================

    private float fpsTimer = 0f;

    private int frameCount = 0;

    // =========================================================
    // EXISTING EVENTS REFERENCE
    // =========================================================

    public GameObject[] events;

    // =========================================================
    // START
    // =========================================================

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.mass = 1400f;

        rb.centerOfMass =
            new Vector3(0f, -0.5f, 0f);

        // Start with the first camera
        if (cams != null && cams.Length > 0)
        {
            SetCamera(0);
        }
    }

    // =========================================================
    // UPDATE
    // =========================================================

    void Update()
    {
        SetCams();

        UpdateSpeed();

        UpdateMaterial();

        UpdateFPS();
    }

    // =========================================================
    // PHYSICS UPDATE
    // =========================================================

    void FixedUpdate()
    {
        if (controlMode == ControlMode.Manual)
        {
            HandleManualControl();
        }
        else
        {
            HandleAutonomousControl();
        }

        UpdateSpeed();

        UpdateMaterial();
    }

    // =========================================================
    // MANUAL CONTROL
    // =========================================================

    void HandleManualControl()
    {
        float throttleInput =
            Input.GetAxis("Vertical");

        float steeringInput =
            Input.GetAxis("Horizontal");

        bool braking =
            Input.GetKey(KeyCode.Space);

        ApplyVehicleControl(
            steeringInput,
            throttleInput,
            braking ? 1f : 0f
        );
    }

    // =========================================================
    // AUTONOMOUS CONTROL
    // =========================================================

    void HandleAutonomousControl()
    {
        ApplyVehicleControl(
            autonomousSteering,
            autonomousThrottle,
            autonomousBrake
        );
    }

    // =========================================================
    // VEHICLE CONTROL INTERFACE
    // =========================================================

    public void SetAutonomousControl(
        float steering,
        float throttle,
        float brake
    )
    {
        autonomousSteering =
            Mathf.Clamp(steering, -1f, 1f);

        autonomousThrottle =
            Mathf.Clamp(throttle, -1f, 1f);

        autonomousBrake =
            Mathf.Clamp01(brake);
    }

    // =========================================================
    // APPLY CONTROL TO VEHICLE
    // =========================================================

    private void ApplyVehicleControl(
        float steering,
        float throttle,
        float brake
    )
    {
        steering =
            Mathf.Clamp(steering, -1f, 1f);

        throttle =
            Mathf.Clamp(throttle, -1f, 1f);

        brake =
            Mathf.Clamp01(brake);

        // -----------------------------------------------------
        // STEERING
        // -----------------------------------------------------

        float steerAngle =
            steering * maxSteerAngle;

        frontLeft.steerAngle =
            steerAngle;

        frontRight.steerAngle =
            steerAngle;

        // -----------------------------------------------------
        // MOTOR
        // -----------------------------------------------------

        float motor =
            throttle * motorTorque;

        rearLeft.motorTorque =
            motor;

        rearRight.motorTorque =
            motor;

        // -----------------------------------------------------
        // BRAKES
        // -----------------------------------------------------

        float appliedBrake =
            brake * brakeTorque;

        frontLeft.brakeTorque =
            appliedBrake;

        frontRight.brakeTorque =
            appliedBrake;

        rearLeft.brakeTorque =
            appliedBrake;

        rearRight.brakeTorque =
            appliedBrake;
    }

    // =========================================================
    // CAMERA
    // =========================================================

    void SetCams()
    {
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

        if (Input.GetKeyDown(KeyCode.C))
        {
            CycleCamera();
        }
    }

    void CycleCamera()
    {
        if (cams == null || cams.Length == 0)
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
        if (cams == null || cams.Length == 0)
            return;

        if (
            cameraIndex < 0 ||
            cameraIndex >= cams.Length
        )
            return;

        currentCamera =
            cameraIndex;

        for (
            int i = 0;
            i < cams.Length;
            i++
        )
        {
            if (cams[i] != null)
            {
                cams[i].SetActive(
                    i == cameraIndex
                );
            }
        }
    }

    // =========================================================
    // FPS
    // =========================================================

    void UpdateFPS()
    {
        frameCount++;

        fpsTimer +=
            Time.unscaledDeltaTime;

        if (fpsTimer >= 1f)
        {
            CurrentFPS =
                frameCount / fpsTimer;

            frameCount = 0;

            fpsTimer = 0f;
        }
    }

    // =========================================================
    // COLLISION
    // =========================================================

    void OnCollisionEnter(
        Collision collision
    )
    {
        CollisionCount++;
    }

    // =========================================================
    // SPEED
    // =========================================================

    void UpdateSpeed()
    {
        CurrentSpeed =
            rb.linearVelocity.magnitude * 3.6f;
    }

    // =========================================================
    // GROUND MATERIAL
    // =========================================================

    void UpdateMaterial()
    {
        string material =
            GetWheelMaterial(frontLeft);

        if (material == "Unknown")
        {
            material =
                GetWheelMaterial(frontRight);
        }

        if (material == "Unknown")
        {
            material =
                GetWheelMaterial(rearLeft);
        }

        if (material == "Unknown")
        {
            material =
                GetWheelMaterial(rearRight);
        }

        CurrentMaterial =
            material;
    }

    string GetWheelMaterial(
        WheelCollider wheel
    )
    {
        if (wheel == null)
            return "Unknown";

        if (
            !wheel.GetGroundHit(
                out WheelHit hit
            )
        )
        {
            return "Unknown";
        }

        Collider groundCollider =
            hit.collider;

        if (groundCollider == null)
            return "Unknown";

        Renderer renderer =
            groundCollider.GetComponent<Renderer>();

        if (renderer == null)
        {
            renderer =
                groundCollider
                    .GetComponentInParent<Renderer>();
        }

        if (renderer == null)
        {
            renderer =
                groundCollider
                    .GetComponentInChildren<Renderer>();
        }

        if (renderer == null)
            return "Unknown";

        if (renderer.sharedMaterial == null)
            return "Unknown";

        return renderer.sharedMaterial.name;
    }
}