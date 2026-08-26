using UnityEngine;

public class Bus : MonoBehaviour
{
    [Header("Waypoints")]
    public Transform[] waypoints;

    public float speed = 5f;
    public float rotationSpeed = 5f;
    public float waypointDistance = 0.5f;

    [Header("Random Stops")]
    public float minStopTime = 1f;
    public float maxStopTime = 4f;
    public float stopChance = 0.01f;

    private int currentWaypoint = 0;
    private bool stopped = false;
    private float stopTimer;

    void Start()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogWarning("Bus has no waypoints assigned.");
            return;
        }

        currentWaypoint = 0;
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0)
            return;

        // Currently stopped
        if (stopped)
        {
            stopTimer -= Time.deltaTime;

            if (stopTimer <= 0f)
            {
                stopped = false;
            }

            return;
        }

        // Randomly stop
        if (Random.value < stopChance * Time.deltaTime)
        {
            stopped = true;
            stopTimer = Random.Range(minStopTime, maxStopTime);
            return;
        }

        FollowWaypoints();
    }

    void FollowWaypoints()
    {
        Transform target = waypoints[currentWaypoint];

        Vector3 direction = target.position - transform.position;
        direction.y = 0f;

        // Reached waypoint
        if (direction.magnitude <= waypointDistance)
        {
            currentWaypoint++;

            // Loop back to first waypoint
            if (currentWaypoint >= waypoints.Length)
            {
                currentWaypoint = 0;
            }

            return;
        }

        if (direction.sqrMagnitude < 0.001f)
            return;

        Vector3 moveDirection = direction.normalized;

        // Move
        transform.position += moveDirection * speed * Time.deltaTime;

        // Face movement direction
        Quaternion targetRotation =
            Quaternion.LookRotation(moveDirection) *
            Quaternion.Euler(0f, 180f, 0f);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}
