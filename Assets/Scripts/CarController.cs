using UnityEngine;

public class CarController : MonoBehaviour
{
public float speed = 10f;


void Update()
{
    float move = Input.GetAxis("Vertical") * speed * Time.deltaTime;
    transform.Translate(0, 0, move);
}

}
