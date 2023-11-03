using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDHealth : MonoBehaviour
{
    [SerializeField][Tooltip("Color of health bar when it's 66% to 100%")] private Color fullColor;
    [SerializeField][Tooltip("Color of health bar when it's 33% to 66%")] private Color halfColor;
    [SerializeField][Tooltip("Color of health bar when it's 0% to 33%")] private Color lowColor;

    private Image healthBar;
    private TMP_Text healthNumberText;

    void Start()
    {
        healthBar = GameObject.Find("HealthBar").GetComponent<Image>();
        healthNumberText = GameObject.Find("HPNumber").GetComponent<TMP_Text>();
    }

    public void UpdateHealthUI(float currentHealth, float maxHealth)
    {
        float healthRatio = currentHealth / maxHealth;
        healthBar.fillAmount = healthRatio;
        if (healthRatio > 0.66)
        {
            healthBar.color = fullColor;
        }
        else if (healthRatio > 0.33)
        {
            healthBar.color = halfColor;
        }
        else
        {
            healthBar.color = lowColor;
        }
        
        healthNumberText.text = Mathf.FloorToInt(currentHealth) + "/" + Mathf.FloorToInt(maxHealth);
    }
}
