using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHealthBar : MonoBehaviour
{
    private NewEnemyHealth newEnemyHealth;

    [SerializeField][Tooltip("Color of health bar when it's 66% to 100%")] private Color fullColor;
    [SerializeField][Tooltip("Color of health bar when it's 33% to 66%")] private Color halfColor;
    [SerializeField][Tooltip("Color of health bar when it's 0% to 33%")] private Color lowColor;

    private float currentHealth;
    private float maxHealth;

    private Image enemyHealthBar;

    [SerializeField] private Canvas enemyHealthCanvas;
    [SerializeField] private GameObject damageTextObject;
    private TMP_Text damageText;
    [SerializeField] private RectTransform damageTextAnchorRectTransform;
    [SerializeField] [Tooltip("How high the damage text goes during the animation")] private float damageTextHeightScalar = 1.0f;
    [SerializeField] [Tooltip("Duration of damage number floating")] private float floatingTime = 1.0f;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        enemyHealthCanvas.GetComponent<Canvas>().worldCamera = mainCamera;
        newEnemyHealth = GetComponentInParent<NewEnemyHealth>();
        enemyHealthBar = GetComponent<Image>();
    }

    public void UpdateEnemyHealth()
    {
        currentHealth = newEnemyHealth.currentHealth;
        maxHealth = newEnemyHealth.maxHealth;

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
    }

    public void DamageNumber(float damage)
    {
        StartCoroutine("DamageNumberAnimation", damage);
    }

    IEnumerator DamageNumberAnimation(float damage)
    {
        Transform parentTransform = this.gameObject.transform.parent;
        GameObject damageTextInstance = Instantiate(damageTextObject, damageTextAnchorRectTransform.position, this.gameObject.transform.rotation, parentTransform);
        TMP_Text damageText = damageTextInstance.GetComponent<TMP_Text>();

        damageText.text = damage.ToString();
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

    void Update()
    {
        enemyHealthCanvas.transform.rotation = Quaternion.LookRotation(enemyHealthCanvas.transform.position - mainCamera.transform.position);
    }
}
