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
    Rigidbody rb;
    protected List<BaseEnemyHealthBar> enemyHealthBars = new List<BaseEnemyHealthBar>();
    public Transform centerPoint;
    protected bool hasTakenDamage = false;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        this.transform.parent = null;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        foreach (BaseEnemyHealthBar enemyHealthBar in GetComponentsInChildren<BaseEnemyHealthBar>())
        {
            enemyHealthBars.Add(enemyHealthBar);
        }
    }

    // Update is called once per frame
    void Update()
    {

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
        if(currentHealth <= 0)
        {
            StartCoroutine(Death());
        }
        hasTakenDamage = true;
    }

    IEnumerator Death()
    {
        foreach (BaseEnemyHealthBar enemyHealthBar in enemyHealthBars)
        {
            enemyHealthBar.DefeatEnemy();
        }

        gameObject.GetComponent<EnemyAttack>().CancelAttack();
        gameObject.GetComponent<EnemyAttack>().enabled = false;
        gameObject.GetComponent<ReworkedEnemyNavigation>().enabled = false;
        rb.velocity = Vector3.zero;
        animator.Rebind();
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(2f);
        ParticleManager.Instance.SpawnParticleFlurry("NutrientParticles", nutrientDrop, 0.1f, this.gameObject.transform.position, Quaternion.Euler(-90f, 0f, 0f));
        if (gameObject.name == "Giga Beetle")
        {
            GameManager.Instance.OnExitToHub();
            PlayerPrefs.SetInt("IsTutorialFinished", Convert.ToInt32(true));
            GameObject.Find("SceneLoader").GetComponent<SceneLoader>().BeginLoadScene(2, false);
        }
        this.gameObject.SetActive(false);
    }

    public bool HasTakenDamage()
    {
        return hasTakenDamage;
    }
}
