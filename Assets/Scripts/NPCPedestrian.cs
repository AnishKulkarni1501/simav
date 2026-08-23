using UnityEngine;

public class NPCPedestrian :
    MonoBehaviour,
    ISimulatedAgent
{
    public float speed = 1.5f;

    private Vector3 destination;
    private bool hasDestination;
    private bool stopped;

    public void InitializeAgent()
    {
        stopped = false;
        hasDestination = false;
    }

    void Update()
    {
        if (stopped || !hasDestination)
            return;

        Vector3 direction =
            destination - transform.position;

        direction.y = 0f;

        if (direction.magnitude < 0.5f)
        {
            hasDestination = false;
            return;
        }

        direction.Normalize();

        transform.position +=
            direction * speed * Time.deltaTime;

        Quaternion targetRotation =
            Quaternion.LookRotation(direction);

        transform.rotation =
            Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                5f * Time.deltaTime
            );
    }

    public void SetDestination(
        Vector3 newDestination)
    {
        destination = newDestination;
        hasDestination = true;
    }

    public void StopAgent()
    {
        stopped = true;
    }

    public void ResumeAgent()
    {
        stopped = false;
    }
}