using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class HUDStats : MonoBehaviour
{    
    [SerializeField] RectTransform statsHolder;
    [SerializeField] RectTransform statsOutsideTarget;
    [SerializeField] RectTransform statsInsideTarget;

    [SerializeField] TMP_Text primalText;
    [SerializeField] TMP_Text sentienceText;
    [SerializeField] TMP_Text speedText;
    [SerializeField] TMP_Text vitalityText;

    [SerializeField] Color statColorFlash = Color.green;
    [SerializeField] float colorFlashTime = 0.25f;
    [SerializeField] float timeBetweenFlashAndSlide = 0.5f;

    public void ShowStats()
    {
        RefreshStats();
        GetComponent<HUDController>().SlideHUDElement(statsHolder, statsOutsideTarget, statsInsideTarget);
    }

    public void HideStats()
    {
        RefreshStats();
        GetComponent<HUDController>().SlideHUDElement(statsHolder, statsInsideTarget, statsOutsideTarget);
    }

    public void ImproveStat(string improvedStat)
    {
        StartCoroutine(ImproveStatCoroutine(improvedStat));
    }

    IEnumerator ImproveStatCoroutine(string improvedStat)
    {
        Debug.Log("1");
        //Color Flash
        yield return StartCoroutine(ColorFlashStat(improvedStat));

        Debug.Log("2");

        yield return new WaitForSeconds(timeBetweenFlashAndSlide);

        Debug.Log("3");
        //Then slide off
        GetComponent<HUDController>().SlideHUDElement(statsHolder, statsInsideTarget, statsOutsideTarget);
    }

    IEnumerator ColorFlashStat(string improvedStat)
    {
        TMP_Text improvedStatText = primalText;
        switch (improvedStat)
        {
            case "Primal":
                improvedStatText = primalText;
                break;
            case "Sentience":
                improvedStatText = sentienceText;
                break;
            case "Speed":
                improvedStatText = speedText;
                break;
            case "Vitality":
                improvedStatText = vitalityText;
                break;
        }

        float flashElapsedTime = 0f;
        float t;
        Color originalColor = improvedStatText.color;
        while (flashElapsedTime < colorFlashTime)
        {
            t = DylanTree.EaseOutQuart(flashElapsedTime / colorFlashTime);

            improvedStatText.color = Color.Lerp(originalColor, statColorFlash, t);

            flashElapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        improvedStatText.color = statColorFlash;

        RefreshStats();

        float returnElapsedTime = 0f;
        while (returnElapsedTime < colorFlashTime)
        {
            t = returnElapsedTime / colorFlashTime;

            improvedStatText.color = Color.Lerp(statColorFlash, originalColor, t);

            returnElapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        improvedStatText.color = originalColor;
    }

    void RefreshStats()
    {
        CharacterStats characterStats = GameObject.FindWithTag("currentPlayer").GetComponent<CharacterStats>();

        primalText.text = characterStats.primalLevel.ToString();
        sentienceText.text = characterStats.sentienceLevel.ToString();
        speedText.text = characterStats.speedLevel.ToString();
        vitalityText.text = characterStats.vitalityLevel.ToString();
    }
}
