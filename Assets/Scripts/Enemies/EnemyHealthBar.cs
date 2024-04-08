using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHealthBar : BaseEnemyHealthBar
{
    [SerializeField] private Canvas enemyHealthCanvas;
    [SerializeField] private Image enemyHealthPanel;
    [SerializeField] private Color enemyHealthPanelDeathColor;
    [SerializeField] private GameObject damageTextObject;
    private TMP_Text damageText;
    [SerializeField] private RectTransform damageTextAnchorRectTransform;
    [SerializeField] [Tooltip("How high the damage text goes during the animation")] private float damageTextHeightScalar = 1.0f;
    [SerializeField] [Tooltip("Duration of damage number floating")] private float floatingTime = 1.0f;
    [SerializeField] [Tooltip("Duration of fade out on death")] private float fadeOutDuration = 0.75f;
    private Camera mainCamera;

    void Start()
    {
        GetComponent<CanvasGroup>().alpha = 0f;

        mainCamera = Camera.main;
        enemyHealthCanvas.GetComponent<Canvas>().worldCamera = mainCamera;
    }

    public override void UpdateEnemyHealthUI()
    {
        float currentHealth = transform.parent.gameObject.GetComponent<EnemyHealth>().currentHealth;
        float maxHealth = transform.parent.gameObject.GetComponent<EnemyHealth>().maxHealth;
        float healthRatio = currentHealth / maxHealth;

        if (healthRatio < 1.0)
        {
            GetComponent<CanvasGroup>().alpha = 1f;
        }

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

        if (currentHealth <= 0)
        {
            StartCoroutine(LerpPanelColor());
            //enemyHealthPanel.color = enemyHealthPanelDeathColor;
        }
    }

    public override void DamageNumber(float damage)
    {
        StartCoroutine("DamageNumberAnimation", damage);
    }

    IEnumerator DamageNumberAnimation(float damage)
    {
        //Debug.Log("damage number");
        Transform parentTransform = transform.GetChild(0);
        GameObject damageTextInstance = Instantiate(damageTextObject, damageTextAnchorRectTransform.position, transform.GetChild(0).rotation, parentTransform);
        TMP_Text damageText = damageTextInstance.GetComponent<TMP_Text>();

        damageText.text = Mathf.RoundToInt(damage).ToString();
        yield return null;

        float t = 0;
        Color startColor = new Color(255,255,255,1);
        Color endColor = new Color(255,255,255,0);
 
        damageText.color = startColor;
        RectTransform damageTextRectTransform = damageText.GetComponent<RectTransform>();
        Vector3 startingPosition = damageTextRectTransform.position;
        Vector3 targetPosition = startingPosition + (Vector3.up * damageTextHeightScalar);
        while (t < floatingTime)
        {
            float normalizedTime = t / floatingTime;
            float logT = Mathf.Lerp(0, 1, Mathf.Log(1 + normalizedTime * 9) / Mathf.Log(10));

            damageText.color = Color.Lerp(startColor, endColor, t);
            damageTextRectTransform.position = Vector3.Lerp(startingPosition, targetPosition, logT);
            t += Time.deltaTime;
            yield return null;
        }
        Destroy(damageTextInstance);
    }

    IEnumerator LerpPanelColor()
    {
        Color startingColor = enemyHealthPanel.color;

        float timer = 0f;
        while (timer < fadeOutDuration)
        {
            float t = EaseOutQuart(timer / fadeOutDuration);

            enemyHealthPanel.color = Color.Lerp(startingColor, enemyHealthPanelDeathColor, t);

            timer += Time.deltaTime;

            yield return null;
        }

        enemyHealthPanel.color = enemyHealthPanelDeathColor;
    }

    void Update()
    {
        enemyHealthCanvas.transform.rotation = Quaternion.LookRotation(enemyHealthCanvas.transform.position - mainCamera.transform.position);
    }

    float EaseOutQuart(float x)
    {
        return 1f - Mathf.Pow(1f - x, 4);
    }
}
