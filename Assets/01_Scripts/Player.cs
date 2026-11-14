using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 달리기, 점프, 슬라이딩
    // 2단 점프 (플랫폼에서 점프 1회, 플랫폼에서 몸이 떨어졌을 때 점프 1회) -> 점프 카운트
    // 피격 (장애물, 데미지 및 무적), 낙하 (낭떠러지, 사망), 사망(낙하, 체력 전부 소모)
    // 무적 아이템 획득 -> 무적

    private Rigidbody2D rigid;
    private SpriteRenderer spriter;

    [SerializeField] private float maxHealth;
    [SerializeField] private float curHealth;

    [SerializeField] private float jumpHeight;
    [SerializeField] private int jumpCount;
    [SerializeField, Min(0f)] private float jumpBufferDuration; // 점프 보정

    private float jumpBufferTimer;

    public float MaxHealth => maxHealth;
    public float CurHealth => curHealth;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        curHealth = maxHealth;
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameover)
            return;

        bool jump = Input.GetKeyDown(KeyCode.Space);

        if (jump)
        {
            jumpBufferTimer = jumpBufferDuration;
        }
        else if (jumpBufferTimer > 0f)
        {
            jumpBufferTimer = Mathf.Max(0f, jumpBufferTimer - Time.deltaTime);
        }

        if ((jump || jumpBufferTimer > 0f) && jumpCount < 2)
        {
            jumpBufferTimer = 0f;
            jumpCount++;
            rigid.velocity = Vector2.zero;
            Jump();
        }
    }

    private void Jump()
    {
        float gravity = Mathf.Abs(Physics2D.gravity.y * rigid.gravityScale);
        float jumpSpeed = Mathf.Sqrt(2f * gravity * jumpHeight);

        rigid.velocity = new Vector2(rigid.velocity.x, jumpSpeed);
    }

    public void Hit(float damage)
    {
        curHealth -= damage;

        if (curHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(HitEffect());
        }
    }

    public void OnDamageOverTime(float damage)
    {
        curHealth -= damage;

        if (curHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator HitEffect()
    {
        Color color = spriter.color;

        color.a = 0.5f;
        spriter.color = color;
        gameObject.tag = "PlayerHit";

        yield return new WaitForSeconds(1f);

        color.a = 1f;
        spriter.color = color;
        gameObject.tag = "Player";
    }

    public void Die()
    {
        rigid.simulated = false;
        GameManager.Instance.GameOver();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Platform"))
            return;

        for (int i = 0; i < collision.contactCount; i++) // 플랫폼에 '착지'했을 때만 점프 초기화
        {
            ContactPoint2D contact = collision.GetContact(i);

            if (contact.normal.y > 0.5f)
            {
                jumpCount = 0;
                rigid.velocity = Vector2.zero;
                break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gameObject.CompareTag("PlayerHit"))
        {
            if (collision.CompareTag("Spikes"))
            {
                Hit(10);
            }
        }

        if (collision.CompareTag("Deadline"))
        {
            Die();
        }
    }
}
