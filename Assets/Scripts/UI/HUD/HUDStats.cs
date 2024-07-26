using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class HUDStats : MonoBehaviour
{    
    [SerializeField] RectTransform sporeName;
    [SerializeField] RectTransform sporeTrait;
    [SerializeField] RectTransform sporeTraitOutsideTarget;
    [SerializeField] RectTransform sporeAndNameTarget;
    [SerializeField] RectTransform sporeNameInsideTarget;

    [SerializeField] RectTransform statsHolder;
    [SerializeField] RectTransform statsOutsideTarget;
    [SerializeField] RectTransform statsInsideTarget;

    [SerializeField] TMP_Text primalText;
    [SerializeField] TMP_Text sentienceText;
    [SerializeField] TMP_Text speedText;
    [SerializeField] TMP_Text vitalityText;
    [SerializeField] TMP_Text primalDamageText;
    [SerializeField] TMP_Text baseHealthText;

    public bool isShowingStats = false;
    [SerializeField] Color defaultStatColor = Color.white;
    [SerializeField] Color goodStatFlashColor = Color.green;
    [SerializeField] Color badStatFlashColor = Color.red;
    [SerializeField] float colorFlashTime = 0.25f;

    int initialPrimalLevel;
    int initialSpeedLevel;
    int initialSentienceLevel;
    int initialVitalityLevel;
    float initialPrimalDamage;
    float initialBaseHealth;

    void Start()
    {
        primalText.color = defaultStatColor;
        sentienceText.color = defaultStatColor;
        speedText.color = defaultStatColor;
        vitalityText.color = defaultStatColor;
    }

    public void UpdateSporeTraitName(CharacterStats characterStats)
    {
        TraitBase trait = characterStats.GetComponent<TraitBase>();
        string traitName = "";
        if (trait != null)
        {
            traitName = characterStats.GetComponent<TraitBase>().traitName;
        }
        sporeTrait.GetComponent<TMP_Text>().text = traitName;
    }

    public void ShowStats()
    {
        GameObject currentPlayer = GameObject.FindWithTag("currentPlayer");
        CharacterStats characterStats = currentPlayer.GetComponent<CharacterStats>();

        UpdateSporeTraitName(characterStats);

        initialPrimalLevel = characterStats.primalLevel;
        initialSpeedLevel = characterStats.speedLevel;
        initialSentienceLevel = characterStats.sentienceLevel;
        initialVitalityLevel = characterStats.vitalityLevel;

        initialPrimalDamage = characterStats.primalDmg;
        initialBaseHealth = characterStats.baseHealth;

        RefreshStats();

        HUDController hudController = GetComponent<HUDController>();
        hudController.SlideHUDElement("Stats", statsHolder, statsInsideTarget);

        sporeTrait.pivot = new Vector2(0f, 0.5f);
        sporeNameInsideTarget.localPosition = new Vector2(sporeAndNameTarget.localPosition.x + sporeTrait.rect.width + 5, sporeAndNameTarget.localPosition.y);

        hudController.SlideHUDElement("Trait", sporeTrait, sporeAndNameTarget);
        hudController.SlideHUDElement("Name", sporeName, sporeNameInsideTarget);
    }

    public void HideStats(float delay = 0f)
    {
        StartCoroutine(HideStatsCoroutine(delay));
    }

    IEnumerator HideStatsCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        HUDController hudController = GetComponent<HUDController>();
        hudController.SlideHUDElement("Stats",statsHolder, statsOutsideTarget);
        sporeTrait.pivot = new Vector2(1f, 0.5f);
        hudController.SlideHUDElement("Trait", sporeTrait, sporeTraitOutsideTarget);
        hudController.SlideHUDElement("Name", sporeName, sporeAndNameTarget);
    }

    public void FlashHUDStats()
    {
        CharacterStats characterStats = GameObject.FindWithTag("currentPlayer").GetComponent<CharacterStats>();

        //Flash each stat
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

            StartCoroutine(FlashStatColor(statText, statModifier));
        }

        //Flash damage and health
        StartCoroutine(FlashStatColor(primalDamageText, characterStats.primalDmg - initialPrimalDamage));
        StartCoroutine(FlashStatColor(baseHealthText, characterStats.baseHealth - initialBaseHealth));
    }

    IEnumerator FlashStatColor(TMP_Text statText, float statModifier)
    {
        float flashElapsedTime = 0f;
        float t;
        Color originalColor = defaultStatColor;
        Color statFlashColor = defaultStatColor;
        if (statModifier > 0)
        {
            statFlashColor = goodStatFlashColor;
        }
        else if (statModifier < 0)
        {
            statFlashColor = badStatFlashColor;
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

        primalDamageText.text = characterStats.primalDmg.ToString();
        baseHealthText.text = Mathf.Ceil(GameObject.FindWithTag("PlayerParent").GetComponent<PlayerHealth>().currentHealth).ToString() + "/" + Mathf.Ceil(characterStats.baseHealth).ToString();
    }
}
