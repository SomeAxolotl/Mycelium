using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class BossHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public int nutrientDrop;
    float deathTimer;
    List<BaseEnemyHealthBar> enemyHealthBars = new List<BaseEnemyHealthBar>();
    Transform player;
    EnemyNavigation enemyNavigation;
    CamTracker camTracker;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindWithTag("currentPlayer").transform;
        enemyNavigation = GetComponent<EnemyNavigation>();
        camTracker = GameObject.FindWithTag("Camtracker").GetComponent<CamTracker>();
    }
    void Update()
    {
        if (currentHealth <= 0)
        {
            Death();
        }
    }
    void Death()
    {

        gameObject.GetComponentInChildren<BossMeleeAttack>().enabled = false;
        deathTimer += Time.deltaTime;
        gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
        enemyNavigation.enabled = false;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + 90f), Time.deltaTime);
        if (deathTimer >= 2f)
        {
            GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>().AddNutrients(nutrientDrop);
            camTracker.ToggleLockOn();
            this.gameObject.SetActive(false);
        }
    }
    public void EnemyTakeDamage(float dmgTaken)
    {
        currentHealth -= dmgTaken;

        //Particle effect for blood
        ParticleManager.Instance.SpawnParticles("Blood", transform.position, Quaternion.identity);
    }
}
