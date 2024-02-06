using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthBar : BaseEnemyHealthBar
{
    [SerializeField] private TMP_Text bossNameText;
    [SerializeField] private TMP_Text bossHealthText;
    [SerializeField] private GameObject bossHealthHolder;
    [SerializeField] private float popDuration = 0.5f;

    [HideInInspector] public bool hasPopped = false;

    void Start()
    {
        bossNameText.text = transform.parent.gameObject.name;
        bossHealthHolder.transform.localScale = new Vector3(0f, bossHealthHolder.transform.localScale.y, bossHealthHolder.transform.localScale.z);
    }

    public override void UpdateEnemyHealthUI()
    {
        float currentHealth = transform.parent.gameObject.GetComponent<EnemyHealth>().currentHealth;
        float maxHealth = transform.parent.gameObject.GetComponent<EnemyHealth>().maxHealth;
        float healthRatio = currentHealth / maxHealth;
        enemyHealthBar.fillAmount = healthRatio;
        if (healthRatio > 0.66)
        {
            enemyHealthBar.color = fullColor;
        }
        else if (healthRatio > 0.33)
        {
            enemyHealthBar.color = halfColor;
        }
        else
        {
            enemyHealthBar.color = lowColor;
        }

        bossHealthText.text = Mathf.FloorToInt(currentHealth) + "/" + Mathf.FloorToInt(maxHealth);
    }

    public override void EncounterEnemy()
    {
        hasPopped = true;

        UpdateEnemyHealthUI();
        StartCoroutine(EncounterEnemyCoroutine());
    }

    IEnumerator EncounterEnemyCoroutine()
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
        StartCoroutine(DefeatingEnemyCoroutine());
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
