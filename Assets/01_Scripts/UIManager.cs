using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum SliderType { Hp, Progress }
public enum TextType { Score, RunGold }

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private Slider sliderHp;
    [SerializeField] private Slider sliderProgress;
    [SerializeField] private TextMeshProUGUI textRunScore;
    [SerializeField] private TextMeshProUGUI textRunGold;

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
        if (maxValue == 0)
            return;

        switch (type)
        {
            case SliderType.Hp:
                sliderHp.value = curValue / maxValue;
                break;
            case SliderType.Progress:
                sliderProgress.value = curValue / maxValue;
                break;
        }
    }

    public void TextUpdate(TextType type, int value)
    {
        switch (type)
        {
            case TextType.Score:
                textRunScore.text = "SCORE:" + value.ToString();
                break;
            case TextType.RunGold:
                textRunGold.text = "GOLD:" + value.ToString();
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
