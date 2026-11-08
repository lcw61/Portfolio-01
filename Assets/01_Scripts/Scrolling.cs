using UnityEngine;

public class Scrolling : MonoBehaviour
{
    private Rigidbody2D rigid;

    [SerializeField] private float speed;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.IsGameover)
            return;

        rigid.MovePosition(rigid.position + Vector2.left * speed * Time.fixedDeltaTime);
    }
}
