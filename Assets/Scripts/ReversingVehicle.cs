using UnityEngine;

public class ReversingVehicle : MonoBehaviour
{
    public Transform[] waypoints;

    public float speed = 5f;
    public float rotationSpeed = 5f;
    public float waypointDistance = 0.5f;

    private int currentWaypoint = 0;

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0)
            return;

        Transform target = waypoints[currentWaypoint];

        Vector3 direction = target.position - transform.position;
        direction.y = 0f;

        // Reached waypoint
        if (direction.magnitude <= waypointDistance)
        {
            currentWaypoint++;

            // LOOP BACK TO FIRST WAYPOINT
            if (currentWaypoint >= waypoints.Length)
            {
                currentWaypoint = 0;
            }

            target = waypoints[currentWaypoint];

            direction = target.position - transform.position;
            direction.y = 0f;
        }

        if (direction.sqrMagnitude < 0.001f)
            return;

        Vector3 moveDirection = direction.normalized;

        // Move toward waypoint
        transform.position += moveDirection * speed * Time.deltaTime;

        // Face waypoint
        Quaternion targetRotation =
            Quaternion.LookRotation(moveDirection) *
            Quaternion.Euler(-90f, 0f, 270f);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}
