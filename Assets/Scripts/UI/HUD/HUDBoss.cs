using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDBoss : BaseEnemyHealthBar
{
    [SerializeField] private Image bossHealthBar;
    [SerializeField] private TMP_Text bossNameText;
    [SerializeField] private TMP_Text bossHealthText;
    [SerializeField] private GameObject bossHealthHolder;
    [SerializeField] private float popDuration = 0.5f;
    void Start()
    {
        bossHealthHolder.transform.localScale = new Vector3(0f, bossHealthHolder.transform.localScale.y, bossHealthHolder.transform.localScale.z);
    }

    public void UpdateBossHealthUI(float currentHealth, float maxHealth)
    {
        float healthRatio = currentHealth / maxHealth;
        bossHealthBar.fillAmount = healthRatio;
        bossHealthText.text = Mathf.FloorToInt(currentHealth) + "/" + Mathf.FloorToInt(maxHealth);

        if (currentHealth <= 0)
        {
            StartCoroutine(DefeatingEnemyCoroutine());
        }
    }

    public void EncounterBoss(string bossName, float currentHealth, float maxHealth)
    {
        bossNameText.text = bossName;
        UpdateBossHealthUI(currentHealth, maxHealth);
        StartCoroutine(EncounterBossCoroutine());
    }

    IEnumerator EncounterBossCoroutine()
    {
        float popCounter = 0f;
        while (popCounter < popDuration)
        {
            float popLerp = DylanTree.EaseOutQuart(popCounter / popDuration);
            bossHealthHolder.transform.localScale = new Vector3(Vector3.Lerp(Vector3.zero, Vector3.one, popLerp).x, bossHealthHolder.transform.localScale.y, bossHealthHolder.transform.localScale.z);

            popCounter += Time.deltaTime;
            yield return null;  
        }

        bossHealthHolder.transform.localScale = new Vector3(1f, bossHealthHolder.transform.localScale.y, bossHealthHolder.transform.localScale.z);
    }
    IEnumerator DefeatingEnemyCoroutine()
    {
        float popCounter = 0f;
        while (popCounter < popDuration)
        {
            float popLerp = DylanTree.EaseOutQuart(popCounter / popDuration);
            bossHealthHolder.transform.localScale = new Vector3(Vector3.Lerp(Vector3.one, Vector3.zero, popLerp).x, bossHealthHolder.transform.localScale.y, bossHealthHolder.transform.localScale.z);

            popCounter += Time.deltaTime;
            yield return null;  
        }

        bossHealthHolder.transform.localScale = new Vector3(0f, bossHealthHolder.transform.localScale.y, bossHealthHolder.transform.localScale.z);
    }
}
