using UnityEngine;

public class PedWalk : MonoBehaviour
{
    public float speed = 5f;

    public Transform pointA;
    public Transform pointB;

    private Transform target;

    void Start()
    {
        target = pointA;
    }

    void Update()
    {
        if (target == null)
            return;

        Vector3 direction = target.position - transform.position;
        direction.y = 0f;

        if (direction.magnitude < 0.1f)
        {
            target = target == pointA ? pointB : pointA;
            return;
        }

        direction.Normalize();

        transform.position += direction * speed * Time.deltaTime;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(direction),
            5f * Time.deltaTime
        );
    }
}
