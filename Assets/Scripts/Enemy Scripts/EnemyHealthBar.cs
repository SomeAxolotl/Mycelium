using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private EnemyHealth enemyHealth;

    [SerializeField][Tooltip("Color of health bar when it's 66% to 100%")] private Color fullColor;
    [SerializeField][Tooltip("Color of health bar when it's 33% to 66%")] private Color halfColor;
    [SerializeField][Tooltip("Color of health bar when it's 0% to 33%")] private Color lowColor;

    private float currentHealth;
    private float maxHealth;

    private Image enemyHealthBar;

    [SerializeField] private Canvas enemyHealthCanvas;
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private RectTransform damageTextAnchorRectTransform;
    [SerializeField] [Tooltip("How high the damage text goes during the animation")] private float damageTextHeightScalar = 1.0f;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        enemyHealthCanvas.GetComponent<Canvas>().worldCamera = mainCamera;

        enemyHealthBar = GetComponent<Image>();
        Debug.Log(enemyHealthBar.fillAmount);
    }

    public void UpdateEnemyHealth()
    {
        currentHealth = enemyHealth.currentHealth;
        maxHealth = enemyHealth.maxHealth;

        float healthRatio = currentHealth / maxHealth;
        Debug.Log("HealthBarRatio: " + healthRatio);

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
    }

    public void DamageNumber(float damage)
    {
        StartCoroutine("DamageNumberAnimation", damage);
    }

    IEnumerator DamageNumberAnimation(float damage)
    {
        damageText.text = damage.ToString();
        yield return null;

        float t = 0;
        Color startColor = new Color(255,255,255,1);
        Color endColor = new Color(255,255,255,0);
 
        damageText.color = startColor;
        RectTransform damageTextRectTransform = damageText.GetComponent<RectTransform>();
        Vector3 startingPosition = damageTextRectTransform.position;
        Vector3 targetPosition = startingPosition + (Vector3.up * damageTextHeightScalar);
        while (t < 1)
        {
            damageText.color = Color.Lerp(startColor, endColor, t);
            damageTextRectTransform.position = Vector3.Lerp(startingPosition, targetPosition, t);
            t += Time.deltaTime;
            yield return null;
        }
        damageTextRectTransform.position = damageTextAnchorRectTransform.position;
    }

    void Update()
    {
        enemyHealthCanvas.transform.rotation = Quaternion.LookRotation(enemyHealthCanvas.transform.position - mainCamera.transform.position);
    }
}
