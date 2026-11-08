using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 달리기, 점프, 슬라이딩
    // 2단 점프 (플랫폼에서 점프 1회, 플랫폼에서 몸이 떨어졌을 때 점프 1회) -> 점프 카운트
    // 피격 (장애물, 데미지 및 무적), 낙하 (낭떠러지, 사망), 사망(낙하, 체력 전부 소모)
    // 무적 아이템 획득 -> 무적

    private Rigidbody2D rigid;
    private CapsuleCollider2D coll; // 애니메이터 에서? or 동작 할때 바꾸기.. 고민중
    private SpriteRenderer spriter;

    [SerializeField] private float health; // 게임매니저로 옮기는게 나은듯?
    [SerializeField] private float jumpHeight;
    [SerializeField] private int jumpCount;
    [SerializeField] private float offset = 2.01f;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        spriter = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2)
        {
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
        health -= damage;

        if (health <= 0)
        {
            Dead();
        }
        else
        {
            StartCoroutine(HitEffect());
        }
    }

    IEnumerator HitEffect()
    {
        Color color = spriter.color;

        color.a = 0.5f;
        spriter.color = color;
        // 레이어 변경

        yield return new WaitForSeconds(0.3f);

        color.a = 1f;
        spriter.color = color;
        // 원래 레이어로 변경
    }

    public void Dead()
    {
        Time.timeScale = 0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            if (collision.gameObject.transform.position.y - offset < transform.position.y)
            {
                jumpCount = 0;
                rigid.velocity = Vector2.zero;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Spikes"))
        {
            Hit(1);
        }
        else if (collision.CompareTag("Deadline"))
        {
            Dead();
        }
    }
}
