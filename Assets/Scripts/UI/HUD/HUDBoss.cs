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

    [HideInInspector] public bool fightingBoss = false;

    void Start()
    {
        bossHealthHolder.transform.localScale = new Vector3(0f, bossHealthHolder.transform.localScale.y, bossHealthHolder.transform.localScale.z);
    }

    public void UpdateBossHealthUI(float currentHealth, float maxHealth)
    {
        float healthRatio = currentHealth / maxHealth;
        bossHealthBar.fillAmount = healthRatio;
        /*if (healthRatio > 0.66)
        {
            bossHealthBar.color = fullColor;
        }
        else if (healthRatio > 0.33)
        {
            bossHealthBar.color = halfColor;
        }
        else
        {
            bossHealthBar.color = lowColor;
        }*/

        bossHealthText.text = Mathf.FloorToInt(currentHealth) + "/" + Mathf.FloorToInt(maxHealth);
    }

    public void EncounterBoss(string bossName, float currentHealth, float maxHealth)
    {
        fightingBoss = true;

        bossNameText.text = bossName;
        UpdateBossHealthUI(currentHealth, maxHealth);
        StartCoroutine(EncounterBossCoroutine());
    }

    IEnumerator EncounterBossCoroutine()
    {
        float popCounter = 0f;
        while (popCounter < popDuration)
        {
            float popLerp = EaseOutQuart(popCounter / popDuration);
            bossHealthHolder.transform.localScale = new Vector3(Vector3.Lerp(Vector3.zero, Vector3.one, popLerp).x, bossHealthHolder.transform.localScale.y, bossHealthHolder.transform.localScale.z);

            popCounter += Time.deltaTime;
            yield return null;  
        }

        bossHealthHolder.transform.localScale = new Vector3(1f, bossHealthHolder.transform.localScale.y, bossHealthHolder.transform.localScale.z);
    }

    public override void DefeatEnemy()
    {
        if (fightingBoss)
        {
            StartCoroutine(DefeatingEnemyCoroutine());
            fightingBoss = false;
            //Debug.Log("DEFEATING ENEMY");
        }
    }

    IEnumerator DefeatingEnemyCoroutine()
    {
        float popCounter = 0f;
        while (popCounter < popDuration)
        {
            float popLerp = EaseOutQuart(popCounter / popDuration);
            bossHealthHolder.transform.localScale = new Vector3(Vector3.Lerp(Vector3.one, Vector3.zero, popLerp).x, bossHealthHolder.transform.localScale.y, bossHealthHolder.transform.localScale.z);

            popCounter += Time.deltaTime;
            yield return null;  
        }

        bossHealthHolder.transform.localScale = new Vector3(0f, bossHealthHolder.transform.localScale.y, bossHealthHolder.transform.localScale.z);
    }

    float EaseOutQuart(float x)
    {
        return 1f - Mathf.Pow(1f - x, 4);
    }
}
