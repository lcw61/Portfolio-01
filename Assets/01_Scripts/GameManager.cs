using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Player player;

    [SerializeField] private float damageOverTime;
    private bool isGameover;
    public bool IsGameover => isGameover;

    private WaitForSeconds updateDelayTime = new WaitForSeconds(0.1f);

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        isGameover = false;

        StartCoroutine(HpUpdateRoutine());
    }

    public void GameOver()
    {
        isGameover = true;
    }

    private IEnumerator HpUpdateRoutine()
    {
        while (!isGameover)
        {
            UIManager.Instance.SliderUpdate(SliderType.Hp, player.CurHealth, player.Maxhealth);

            yield return updateDelayTime;

            player.OnDamageOverTime(damageOverTime);

            UIManager.Instance.SliderUpdate(SliderType.Hp, player.CurHealth, player.Maxhealth);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
