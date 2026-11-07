using UnityEngine;

public class Scrolling : MonoBehaviour
{
    [SerializeField] private float speed;

    private void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
    }
}
