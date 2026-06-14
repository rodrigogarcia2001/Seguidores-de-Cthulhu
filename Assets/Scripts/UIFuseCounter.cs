using System.Collections;
using TMPro;
using UnityEngine;

public class UIFuseCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text counterText;
    [SerializeField] private GameObject uiContainer;
    [SerializeField] private RectTransform fuseUI;

    private void Start()
    {
        UpdateText(
            FuseCounter.Instance.CurrentFuses,
            FuseCounter.Instance.TotalFuses);

        FuseCounter.Instance.OnFusesChanged += UpdateText;
    }

    private void OnDestroy()
    {
        if (FuseCounter.Instance != null)
            FuseCounter.Instance.OnFusesChanged -= UpdateText;
    }

    private void UpdateText(int current, int total)
    {
        uiContainer.SetActive(current > 0);

        counterText.text = $"{current}/{total}";

        StartCoroutine(PopAnimation());
    }

    private IEnumerator PopAnimation()
    {
        Vector3 originalScale = fuseUI.localScale;
        Vector3 targetScale = originalScale * 1.25f;

        float duration = 0.1f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            fuseUI.localScale = Vector3.Lerp(originalScale, targetScale, timer / duration);
            yield return null;
        }

        timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            fuseUI.localScale = Vector3.Lerp(targetScale, originalScale, timer / duration);
            yield return null;
        }

        fuseUI.localScale = originalScale;
    }
}