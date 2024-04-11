using System.Collections;
using UnityEngine;

public static class UIExtension
{
    public static IEnumerator FadeToAlpha(this CanvasGroup canvasGroup, float alpha, float duration)
    {
        float timer = 0f;
        float originalAlpha = canvasGroup.alpha;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(originalAlpha, alpha, timer / duration);
            yield return new WaitForEndOfFrame();
        }
        canvasGroup.alpha = alpha;
    }
}