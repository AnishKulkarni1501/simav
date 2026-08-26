using UnityEngine;

public class InterpolateBasic : MonoBehaviour
{
    public float speed = 8f;
    public float rotationSpeed = 5f;

    public Transform[] waypoints;
    public Transform spawnloc;

    [Header("Orientation")]
    public float incorrectAngle = -90f;
    public bool incorrectOrien;

    private int currentWaypoint;
    private bool stopped;

    public void Start()
    {
        if (spawnloc != null)
        {
            transform.position = spawnloc.position;
        }
    }

    public void InitializeAgent()
    {
        currentWaypoint = 0;
        stopped = false;
    }

    void Update()
    {
        if (stopped)
            return;

        FollowWaypoints();
    }

    void FollowWaypoints()
    {
        if (waypoints == null || waypoints.Length == 0)
            return;

        Transform target = waypoints[currentWaypoint];

        Vector3 direction = target.position - transform.position;
        direction.y = 0f;

        if (direction.magnitude < 3f)
        {
            currentWaypoint++;

            if (currentWaypoint >= waypoints.Length)
            {
                currentWaypoint = 0;
            }

            return;
        }

        direction.Normalize();

        // Move
        transform.position += direction * speed * Time.deltaTime;

        // Get rotation toward waypoint
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Apply orientation correction
        if (incorrectOrien)
        {
            targetRotation *= Quaternion.Euler(0f, incorrectAngle, 0f);
        }

        // Smooth rotation
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    public void SetDestination(Vector3 destination)
    {
        Vector3 direction = destination - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            if (incorrectOrien)
            {
                targetRotation *= Quaternion.Euler(
                    0f,
                    incorrectAngle,
                    0f
                );
            }

            transform.rotation = targetRotation;
        }
    }

    public void StopAgent()
    {
        stopped = true;
    }

    public void ResumeAgent()
    {
        stopped = false;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}
