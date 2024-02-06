using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseEnemyHealthBar : MonoBehaviour
{
    [SerializeField][Tooltip("Color of health bar when it's 66% to 100%")] public Color fullColor;
    [SerializeField][Tooltip("Color of health bar when it's 33% to 66%")] public Color halfColor;
    [SerializeField][Tooltip("Color of health bar when it's 0% to 33%")] public Color lowColor;

    [SerializeField] public Image enemyHealthBar;

    public virtual void UpdateEnemyHealthUI()
    {
        //Overridden by child
    }

    public virtual void DamageNumber(float damage)
    {
        //Overridden by child
    }

    public virtual void EncounterEnemy()
    {
        //Overridden by child
    }

    public virtual void DefeatEnemy()
    {
        //Overridden by child
    }
}
