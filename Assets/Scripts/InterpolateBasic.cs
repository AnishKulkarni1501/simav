using UnityEngine;

public class InterpolateBasic : MonoBehaviour
{
    public float speed = 8f;
    public float rotationSpeed = 5f;

    public Transform[] waypoints;

    public Transform spawnloc;
    public bool minus90mod;
    private int currentWaypoint;
    private bool stopped;

    public void Start()
    {
        if(spawnloc.position != null){
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

        Transform target =
            waypoints[currentWaypoint];

        Vector3 direction =
            target.position - transform.position;

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

        transform.position +=
            direction * speed * Time.deltaTime;

        Quaternion targetRotation = minus90mod? Quaternion.LookRotation(direction*(-90f)) :  Quaternion.LookRotation(direction);

        transform.rotation =
            Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
    }

    public void SetDestination(Vector3 destination)
    {
        // Simple implementation:
        // create a temporary direction toward destination

        Vector3 direction =
            destination - transform.position;

        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            transform.rotation =
                Quaternion.LookRotation(direction);
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
