using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using System;
//using UnityEditor.Rendering.Universal.ShaderGUI;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public int nutrientDrop;
    Rigidbody rb;
    protected List<BaseEnemyHealthBar> enemyHealthBars = new List<BaseEnemyHealthBar>();
    public Transform centerPoint;
    protected bool hasTakenDamage = false;
    [HideInInspector] public bool alreadyDead = false;
    Animator animator;

    private ProfileManager profileManagerScript;

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

        profileManagerScript = GameObject.Find("ProfileManager").GetComponent<ProfileManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public virtual void EnemyTakeDamage(float dmgTaken)
    {
        currentHealth -= dmgTaken;

        foreach (BaseEnemyHealthBar enemyHealthBar in enemyHealthBars)
        {
            if(enemyHealthBar != null && currentHealth + dmgTaken > 0) 
            {
                enemyHealthBar.UpdateEnemyHealthUI();
                enemyHealthBar.DamageNumber(dmgTaken);
                ParticleManager.Instance.SpawnParticles("Blood", centerPoint.position, Quaternion.identity);
            }
        }
        if(currentHealth <= 0 && !alreadyDead)
        {
            StartCoroutine(Death());
        }
        hasTakenDamage = true;
    }

    protected IEnumerator Death()
    {
        if (gameObject.name != "Rival Colony Leader")
        {
            gameObject.GetComponent<EnemyAttack>().CancelAttack();
            gameObject.GetComponent<EnemyAttack>().enabled = false;
            gameObject.GetComponent<ReworkedEnemyNavigation>().enabled = false;
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
        }
        else
        {
            GameObject.Find("CreditsPlayer").GetComponent<CreditsPlayer>().StartPlayCredits();
        }


        alreadyDead = true;
        
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezePosition;
        
        animator.Rebind();
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(1.25f);
        float elapsedTime = 0f;
        float shrinkDuration = 1f;
        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = Vector3.zero;
        while (elapsedTime < shrinkDuration)
        {

            elapsedTime += Time.deltaTime;
            float t = elapsedTime / shrinkDuration;
            transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            yield return null;
        }
        ParticleManager.Instance.SpawnParticleFlurry("NutrientParticles", nutrientDrop, 0.1f, this.gameObject.transform.position, Quaternion.Euler(-90f, 0f, 0f));
        if (gameObject.name == "Giga Beetle")
        {
            //GameManager.Instance.OnExitToHub();
            profileManagerScript.tutorialIsDone[GlobalData.profileNumber] = true;
            GameObject.Find("SceneLoader").GetComponent<SceneLoader>().BeginLoadScene(2, false);
        }

        this.gameObject.SetActive(false);
    }
    protected IEnumerator BossDeath()
    {
        GameObject boss = GameObject.Find("Rival Colony Leader");

        Collider[] bossCollider = boss.GetComponents<Collider>();
        foreach (Collider collider in bossCollider)
        {
            collider.enabled = false;
        }
        Collider[] childColliders = boss.GetComponentsInChildren<Collider>();
        foreach (Collider collider in childColliders)
        {
            collider.enabled = false;
        }

        boss.GetComponent<MonsterBossAttack>().enabled = false;
        boss.GetComponent<TempMovement>().enabled = false;
        boss.GetComponent<Animator>().SetTrigger("Death");

        GameObject player = GameObject.FindWithTag("PlayerParent");
        player.GetComponent<PlayerController>().moveSpeed = 0;
        yield return null;
        // if (bossDead == true)
        // {
        //     Debug.Log("true");
        //     yield return null;
        // }
        // GameObject.Find("CreditsPlayer").GetComponent<CreditsPlayer>().StartPlayCredits();
        // alreadyDead = true;
        // ParticleManager.Instance.SpawnParticleFlurry("NutrientParticles", nutrientDrop, 0.1f, this.gameObject.transform.position, Quaternion.Euler(-90f, 0f, 0f));
        // this.gameObject.SetActive(false);
    }

    public bool HasTakenDamage()
    {
        return hasTakenDamage;
    }
}
