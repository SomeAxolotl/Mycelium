using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

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

    public void EncounterBoss(EnemyHealth enemyHealth)
    {
        string fullBossName = GetBossNameWithAttributes(enemyHealth);
        bossNameText.text = fullBossName;
        UpdateBossHealthUI(enemyHealth.currentHealth, enemyHealth.maxHealth);
        StartCoroutine(EncounterBossCoroutine());
    }

    public void EncounterBoss(string bossName, float currentHealth, float maxHealth)
    {
        string fullBossName = GetBossNameWithAttributes(bossName);
        bossNameText.text = fullBossName;
        UpdateBossHealthUI(currentHealth, maxHealth);
        StartCoroutine(EncounterBossCoroutine());
    }

    private string GetBossNameWithAttributes(EnemyHealth enemyHealth)
    {
        string attributes = enemyHealth.attributePrefix;
        return attributes + enemyHealth.miniBossName;
    }

    private string GetBossNameWithAttributes(string bossName)
    {
        // If the boss is not an EnemyHealth type, just return the name.
        return bossName;
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
