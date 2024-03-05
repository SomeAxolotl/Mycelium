using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private float hudFadeTransitionTime = 0.25f;

    public void FadeInHUD()
    {
        CanvasGroup hudCanvasGroup = GetComponent<CanvasGroup>();

        if (hudCanvasGroup.alpha != 1f)
        {
            StartCoroutine(FadeCanvasIn(hudCanvasGroup, hudFadeTransitionTime));
        }
    }

    IEnumerator FadeCanvasIn(CanvasGroup canvasGroup, float transitionTime)
    {
        Debug.Log("hud fade in " + transitionTime);

        float elapsedTime = 0f;
        float t = 0f;

        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

        while (elapsedTime < transitionTime)
        {
            t = elapsedTime / transitionTime;

            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);

            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;

        try
        {
            GameObject.Find("PauseMenuCanvas").GetComponent<PauseMenu>().Resume();
        }
        catch
        {
            
        }
    }
}
