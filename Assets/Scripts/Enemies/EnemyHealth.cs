using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using System;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public int nutrientDrop;
    float deathTimer;
    Rigidbody rb;
    protected List<BaseEnemyHealthBar> enemyHealthBars = new List<BaseEnemyHealthBar>();
    public Transform centerPoint;
    protected bool hasTakenDamage = false;
    ReworkedEnemyNavigation reworkedEnemyNav;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
        this.transform.parent = null;
        reworkedEnemyNav = GetComponent<ReworkedEnemyNavigation>();
        foreach (BaseEnemyHealthBar enemyHealthBar in GetComponentsInChildren<BaseEnemyHealthBar>())
        {
            enemyHealthBars.Add(enemyHealthBar);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            deathTimer += Time.deltaTime;
            Death();
        }
    }
    void Death()
    {
        foreach (BaseEnemyHealthBar enemyHealthBar in enemyHealthBars)
        {
            enemyHealthBar.DefeatEnemy();
        }

        if(gameObject.GetComponent<MeleeEnemyAttack>() != null)
        {
            gameObject.GetComponent<MeleeEnemyAttack>().enabled = false;
        }
        else if (gameObject.GetComponent<RangedEnemyShoot>() != null)
        {
            gameObject.GetComponent<RangedEnemyShoot>().enabled = false;
        }
        gameObject.GetComponent<Collider>().enabled = false;
        reworkedEnemyNav.enabled = false;
        rb.velocity = Vector3.zero;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + 90f), Time.deltaTime);
        if(deathTimer >= 2f)
        {
            GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>().AddNutrients(nutrientDrop);
            ParticleManager.Instance.SpawnParticleFlurry("NutrientParticles", nutrientDrop / 20, 0.1f, this.gameObject.transform.position, Quaternion.Euler(-90f, 0f, 0f));

            if (gameObject.name == "Giga Beetle")
            {
                GameManager.Instance.OnExitToHub();
                PlayerPrefs.SetInt("IsTutorialFinished", Convert.ToInt32(true));
                GameObject.Find("SceneLoader").GetComponent<SceneLoader>().BeginLoadScene(2, false);
            }

            this.gameObject.SetActive(false);
        }
    }
    public virtual void EnemyTakeDamage(float dmgTaken)
    {
        currentHealth -= dmgTaken;
        ParticleManager.Instance.SpawnParticles("Blood", centerPoint.position, Quaternion.identity);

        foreach (BaseEnemyHealthBar enemyHealthBar in enemyHealthBars)
        {
            if(enemyHealthBar != null && currentHealth + dmgTaken > 0) 
            {
                enemyHealthBar.UpdateEnemyHealthUI();
                enemyHealthBar.DamageNumber(dmgTaken);
            }

        }

        hasTakenDamage = true;
    }

    public bool HasTakenDamage()
    {
        return hasTakenDamage;
    }
}
