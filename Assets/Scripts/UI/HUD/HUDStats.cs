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

    public bool isShowingStats = false;
    [SerializeField] Color goodStatFlashColor = Color.green;
    [SerializeField] Color badStatFlashColor = Color.red;
    [SerializeField] float colorFlashTime = 0.25f;

    int initialPrimalLevel;
    int initialSpeedLevel;
    int initialSentienceLevel;
    int initialVitalityLevel;

    public void ShowStats()
    {
        CharacterStats characterStats = GameObject.FindWithTag("currentPlayer").GetComponent<CharacterStats>();

        initialPrimalLevel = characterStats.primalLevel;
        initialSpeedLevel = characterStats.speedLevel;
        initialSentienceLevel = characterStats.sentienceLevel;
        initialVitalityLevel = characterStats.vitalityLevel;

        RefreshStats();
        GetComponent<HUDController>().SlideHUDElement(statsHolder, statsInsideTarget);
    }

    public void HideStats(float delay = 0f)
    {
        StartCoroutine(HideStatsCoroutine(delay));
    }

    IEnumerator HideStatsCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        GetComponent<HUDController>().SlideHUDElement(statsHolder, statsOutsideTarget);
    }

    public void FlashHUDStats()
    {
        CharacterStats characterStats = GameObject.FindWithTag("currentPlayer").GetComponent<CharacterStats>();

        for (int i = 0; i < 4; i++)
        {   
            int statModifier = 0;
            TMP_Text statText = null;
            switch ((CharacterStats.Stats)i)
            {
                case CharacterStats.Stats.Primal:
                    statModifier = characterStats.primalLevel - initialPrimalLevel;
                    statText = primalText;
                    break; 
                case CharacterStats.Stats.Speed:
                    statModifier = characterStats.speedLevel - initialSpeedLevel;
                    statText = speedText;
                    break; 
                case CharacterStats.Stats.Sentience:
                    statModifier = characterStats.sentienceLevel - initialSentienceLevel;
                    statText = sentienceText;
                    break; 
                case CharacterStats.Stats.Vitality:
                    statModifier = characterStats.vitalityLevel - initialVitalityLevel;
                    statText = vitalityText;
                    break;
                default:
                    Debug.LogError("HUDStats: Invalid stat");
                    break;
            }

            StartCoroutine(FlashStatColor(statText, (CharacterStats.Stats)i, statModifier));
        }
    }

    IEnumerator FlashStatColor(TMP_Text statText, CharacterStats.Stats stat, int statModifier)
    {
        float flashElapsedTime = 0f;
        float t;
        Color originalColor = statText.color;
        Color statFlashColor;
        if (statModifier > 0)
        {
            statFlashColor = goodStatFlashColor;
        }
        else if (statModifier < 0)
        {
            statFlashColor = badStatFlashColor;
        }
        else
        {
            statFlashColor = originalColor;
        }

        while (flashElapsedTime < colorFlashTime)
        {
            t = DylanTree.EaseOutQuart(flashElapsedTime / colorFlashTime);

            statText.color = Color.Lerp(originalColor, statFlashColor, t);

            flashElapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        statText.color = statFlashColor;

        RefreshStats();

        float returnElapsedTime = 0f;
        while (returnElapsedTime < colorFlashTime)
        {
            t = returnElapsedTime / colorFlashTime;

            statText.color = Color.Lerp(statFlashColor, originalColor, t);

            returnElapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        statText.color = originalColor;
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
