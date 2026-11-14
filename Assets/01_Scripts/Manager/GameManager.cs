using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Player player;

    private bool isPause;
    private bool isGameover;
    public bool IsGameover => isGameover;

    // Score        - 최고 점수, 점수 배율 추가 필요
    private float itemScore; // 해당 게임 아이템 점수
    public float DistanceScore => totalDistance * scorePerDistance; // 해당 게임 거리 점수 

    // Coin         - 총 보유 골드, 골드 획득 배율 추가 필요
    private int runGold; // 해당 게임에서 획득한 골드

    // Progress
    private int stage;
    private float totalDistance; // 총 거리
    [SerializeField] private float[] maxDistance; // 최대 거리
    [SerializeField] private float curDistance; // 현재 스테이지 거리

    public float MaxDistance => maxDistance[stage];
    public float CurDistance => curDistance;

    [SerializeField] private float damageOverTime;
    [SerializeField] private float distancePerSecond;
    [SerializeField] private float scorePerDistance;

    private WaitForSeconds updateInterval = new WaitForSeconds(0.1f);

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

        StartCoroutine(DamageOverTime());
        StartCoroutine(UpdateUIRoutine());
    }

    private void Update()
    {
        if (isGameover)
            return;

        curDistance += Time.deltaTime * distancePerSecond;
        totalDistance += Time.deltaTime * distancePerSecond;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPause)
            {
                GamePause();
            }
            else if(isPause)
            {
                GameResume();
            }
        }
    }

    private IEnumerator DamageOverTime()
    {
        while (!isGameover)
        {
            yield return updateInterval;

            player.OnDamageOverTime(damageOverTime);

            if (IsGameover)
                break;
        }
    }

    private IEnumerator UpdateUIRoutine()
    {
        while (!IsGameover)
        {
            UIManager.Instance.TextUpdate(TextType.Score, Mathf.FloorToInt(DistanceScore + itemScore));
            UIManager.Instance.SliderUpdate(SliderType.Hp, player.CurHealth, player.MaxHealth);
            UIManager.Instance.SliderUpdate(SliderType.Progress, curDistance, MaxDistance);

            yield return updateInterval;
        }
    }

    public void AddCoin(int amount)
    {
        runGold += amount;
        UIManager.Instance.TextUpdate(TextType.RunGold, runGold);
    }

    public void AddScore(float amount)
    {
        itemScore += amount;
    }

    public void GameOver() // 바로 씬 옮기지 말고 약간의 대기시간 주기
    {
        isGameover = true;
        SceneManager.LoadScene(2);
    }

    private void GamePause()
    {
        isPause = true;
        Time.timeScale = 0f;
        UIManager.Instance.ShowPauseUI();
    }

    private void GameResume() // 코루틴 이용해서 3, 2, 1 이후에 시작 하도록 바꾸기
    {
        isPause = false;
        Time.timeScale = 1f;
        UIManager.Instance.ClosePauseUI();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
