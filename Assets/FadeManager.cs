using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;
    private CanvasGroup fadeCanvasGroup;
    private Coroutine currentFadeCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Create a new Canvas GameObject
            GameObject canvasObj = new GameObject("FadeCanvas");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler canvasScaler = canvasObj.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasObj.AddComponent<GraphicRaycaster>();

            // Create a new CanvasGroup for fading
            fadeCanvasGroup = canvasObj.AddComponent<CanvasGroup>();
            fadeCanvasGroup.alpha = 0;

            // Create a new Image GameObject
            GameObject imageObj = new GameObject("FadeImage");
            imageObj.transform.parent = canvasObj.transform;
            Image image = imageObj.AddComponent<Image>();
            image.color = Color.black;
            RectTransform rectTransform = image.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;

        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartFade(float duration)
    {
        if (currentFadeCoroutine != null)
        {
            StopCoroutine(currentFadeCoroutine);
        }
        currentFadeCoroutine = StartCoroutine(FadeInOut(duration));
    }

    private IEnumerator FadeInOut(float duration)
    {
        yield return Fade(1, duration / 2);
        yield return new WaitForSeconds(duration / 2);
        yield return Fade(0, duration / 2);
    }

    private IEnumerator Fade(float targetAlpha, float duration)
    {
        float startAlpha = fadeCanvasGroup.alpha;
        float time = 0;

        while (time < duration)
        {
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        fadeCanvasGroup.alpha = targetAlpha;
    }
}
