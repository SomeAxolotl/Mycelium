using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AlliedSporeHealthBar : MonoBehaviour
{
    [SerializeField][Tooltip("Color of health bar when it's 66% to 100%")] public Color fullColor;
    [SerializeField][Tooltip("Color of health bar when it's 33% to 66%")] public Color halfColor;
    [SerializeField][Tooltip("Color of health bar when it's 0% to 33%")] public Color lowColor;

    [SerializeField] public Image alliedSporeHealthBar;
    [SerializeField] private Canvas alliedSporeHealthCanvas;
    [SerializeField] private Image alliedSporeHealthPanel;
    //[SerializeField] private Color enemyHealthPanelDeathColor;
    public TMP_Text alliedSporeHealthName;
    [SerializeField] private GameObject damageTextObject;
    private TMP_Text damageText;
    [SerializeField] private RectTransform damageTextAnchorRectTransform;
    [SerializeField][Tooltip("How high the damage text goes during the animation")] private float damageTextHeightScalar = 1.0f;
    [SerializeField][Tooltip("Duration of damage number floating")] private float floatingTime = 1.0f;
    [SerializeField][Tooltip("Duration of fade out on death")] private float fadeOutDuration = 0.75f;
    private Camera mainCamera;
    void Start()
    {
        alliedSporeHealthPanel.GetComponent<CanvasGroup>().alpha = 0f;

        alliedSporeHealthName.text = transform.parent.gameObject.GetComponent<CharacterStats>().sporeName;

        mainCamera = Camera.main;
        alliedSporeHealthCanvas.GetComponent<Canvas>().worldCamera = mainCamera;
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        float currentHealth = transform.parent.gameObject.GetComponent<AlliedSporeHealth>().currentHealth;
        float maxHealth = transform.parent.gameObject.GetComponent<AlliedSporeHealth>().maxHealth;
        float healthRatio = currentHealth / maxHealth;

        if (healthRatio < 1.0)
        {
            alliedSporeHealthPanel.GetComponent<CanvasGroup>().alpha = 1f;
        }

        StartCoroutine(AnimateHealthChange(healthRatio));
    }

    IEnumerator AnimateHealthChange(float targetRatio)
    {
        float duration = 0.5f; // Duration of the animation in seconds
        float elapsed = 0f;
        float startingRatio = alliedSporeHealthBar.fillAmount;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / duration;

            float newT = DylanTree.EaseOutQuart(t);

            alliedSporeHealthBar.fillAmount = Mathf.Lerp(startingRatio, targetRatio, newT);
            yield return null;
        }
        alliedSporeHealthBar.fillAmount = targetRatio;

        // Update health bar color based on health ratio
        if (targetRatio > 0.66)
        {
            alliedSporeHealthBar.color = fullColor;
        }
        else if (targetRatio > 0.33)
        {
            alliedSporeHealthBar.color = halfColor;
        }
        else
        {
            alliedSporeHealthBar.color = lowColor;
        }

        if (targetRatio <= 0)
        {
            StartCoroutine(LerpPanelSize());
        }
    }

    public void DamageNumber(float damage)
    {
        StartCoroutine("DamageNumberAnimation", damage);
    }

    IEnumerator DamageNumberAnimation(float damage)
    {
        if (Mathf.RoundToInt(damage) > 0)
        {
            //Debug.Log("damage number");
            Transform parentTransform = transform.GetChild(2);
            GameObject damageTextInstance = Instantiate(damageTextObject, damageTextAnchorRectTransform.position, transform.GetChild(0).rotation, parentTransform);
            TMP_Text damageText = damageTextInstance.GetComponent<TMP_Text>();

            damageText.text = Mathf.RoundToInt(damage).ToString();
            yield return null;

            float t = 0;
            Color startColor = new Color(255, 255, 255, 1);
            Color endColor = new Color(255, 255, 255, 0);

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
    }

    IEnumerator LerpPanelSize()
    {
        Vector3 startingScale = alliedSporeHealthPanel.rectTransform.localScale;

        float timer = 0f;
        while (timer < fadeOutDuration)
        {
            float t = DylanTree.EaseOutQuart(timer / fadeOutDuration);

            alliedSporeHealthPanel.rectTransform.localScale = Vector3.Lerp(new Vector3(startingScale.x, startingScale.y, startingScale.z), new Vector3(0f, startingScale.y, startingScale.z), t);

            timer += Time.deltaTime;

            yield return null;
        }

        alliedSporeHealthPanel.rectTransform.localScale = Vector3.zero;
    }

    void Update()
    {
        alliedSporeHealthCanvas.transform.rotation = Quaternion.LookRotation(alliedSporeHealthCanvas.transform.position - mainCamera.transform.position);
    }
}
