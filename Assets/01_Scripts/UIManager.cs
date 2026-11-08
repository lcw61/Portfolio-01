using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum SliderType { Hp, }

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private Slider sliderHp;
    [SerializeField] private TextMeshProUGUI textScore;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SliderUpdate(SliderType type, float curValue, float maxValue)
    {
        switch (type)
        {
            case SliderType.Hp:
                if (curValue < 0)
                {
                    curValue = 0;
                }
                sliderHp.value = curValue / maxValue;
                break;
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
