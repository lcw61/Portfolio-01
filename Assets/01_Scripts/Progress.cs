using UnityEngine;

public class Progress : MonoBehaviour
{
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameover)
            return;

        // -10 ~ 140
        float x = -10 + 150 * (GameManager.Instance.CurDistance / GameManager.Instance.MaxDistance);

        rectTransform.anchoredPosition = new Vector3(x, 5, 0);
    }
}
