using TMPro;
using UnityEngine;
using System.Collections;
public class UIKeysCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text counterText;
    [SerializeField] private RectTransform keyUI;
    private void Start()
    {
        UpdateText(
            KeyCounter.Instance.CurrentKeys,
            KeyCounter.Instance.TotalKeys);

        KeyCounter.Instance.OnKeysChanged += UpdateText;
    }

    private void OnDestroy()
    {
        if (KeyCounter.Instance != null)
            KeyCounter.Instance.OnKeysChanged -= UpdateText;
    }

    private void UpdateText(int current, int total)
    {
        counterText.text = $"{current}/{total}";
        StartCoroutine(PopAnimation());
    }

    private IEnumerator PopAnimation()
    {
        Vector3 originalScale = keyUI.localScale;
        Vector3 targetScale = originalScale * 1.25f;

        float duration = 0.1f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            keyUI.localScale = Vector3.Lerp(originalScale, targetScale, timer / duration);
            yield return null;
        }

        timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            keyUI.localScale = Vector3.Lerp(targetScale, originalScale, timer / duration);
            yield return null;
        }

        keyUI.localScale = originalScale;
    }
}