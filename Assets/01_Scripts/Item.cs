using UnityEngine;

public enum ItemType
{
    Score,
    Coin,
}

public class Item : MonoBehaviour
{
    [SerializeField] private ItemType type;
    [SerializeField] private int value;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("PlayerHit"))
        {
            switch (type)
            {
                case ItemType.Score:
                    GameManager.Instance.AddScore(value);
                    gameObject.SetActive(false);
                    break;
                case ItemType.Coin:
                    GameManager.Instance.AddCoin(value);
                    gameObject.SetActive(false);
                    break;
            }
        }
    }
}
