using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponCollision : MonoBehaviour
{
    PlayerAttack playerAttack;
    PlayerHealth playerHealth;
    GameObject player; 
    public float sentienceBonusDamage;
    public float reflectBonusDamage;
    List<GameObject> enemiesHit = new List<GameObject>();
    WeaponStats weaponStats;
    RelentlessFury relentlessFury;
    public bool hitStopping = false;
    public bool isCycloning = false;
    public float dmgDealt;
    
    public Action<GameObject, float> HitEnemy;

    void Start()
    {
        playerAttack = GameObject.Find("PlayerParent").GetComponent<PlayerAttack>();
        playerHealth = GameObject.Find("PlayerParent").GetComponent<PlayerHealth>();
        player = GameObject.FindWithTag("currentPlayer");
        weaponStats = GetComponent<WeaponStats>();
        Invoke("CheckForSkills", 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.gameObject.tag == "currentWeapon" && other.gameObject.tag == "Enemy" && other.GetType() != typeof(SphereCollider) && !enemiesHit.Contains(other.gameObject))
        {
            enemiesHit.Add(other.gameObject);
            dmgDealt = playerAttack.dmgDealt + sentienceBonusDamage + reflectBonusDamage;
            HitEnemy?.Invoke(other.gameObject, dmgDealt);
            //Debug.Log(other.gameObject);
            if (other.GetComponent<EnemyHealth>() != null)
            {
                other.GetComponent<EnemyHealth>().EnemyTakeDamage(dmgDealt);
            }
            else if(other.GetComponent<BossHealth>() != null)
            {
                other.GetComponent<BossHealth>().EnemyTakeDamage(dmgDealt);
            }
            if(relentlessFury != null)
            {
                if(relentlessFury.isFrenzied)
                {
                    playerHealth.PlayerHeal(dmgDealt / 4f);
                }

            }
            
            if (other.GetComponent<EnemyKnockback>() != null)
            {
                other.GetComponent<EnemyKnockback>().Knockback(weaponStats.statNums.advKnockback.Value, player.transform, other.transform, false);
            }

            SoundEffectManager.Instance.PlaySound("Impact", other.gameObject.transform);
            if(weaponStats.secondsTilHitstopSpeedup > 0){
                StartCoroutine(HitStop());
            }
            reflectBonusDamage = 0f;
        }
        /*if (this.gameObject.tag == "currentWeapon" && other.GetType() != typeof(SphereCollider) && other.gameObject.tag == "Boss")
        {
            float dmgDealt = playerAttack.dmgDealt + sentienceBonusDamage + reflectBonusDamage;
            other.GetComponentInParent<BossHealth>().EnemyTakeDamage(dmgDealt);
            if(relentlessFury != null)
            {
                if(relentlessFury.isFrenzied)
                {
                    playerHealth.PlayerHeal(dmgDealt / 4f);
                }

            }
            SoundEffectManager.Instance.PlaySound("Impact", other.gameObject.transform.position);
        }*/
    }

    IEnumerator HitStop()
    {
        hitStopping = true;

        GameObject currentPlayer = GameObject.FindWithTag("currentPlayer");

        Animator animator = currentPlayer.GetComponent<Animator>();

        animator.speed = 0f;

        //Speed Up
        float t = 0f;
        while (t < weaponStats.secondsTilHitstopSpeedup)
        {
            t += Time.unscaledDeltaTime;
            yield return null;
        }

        if (!isCycloning)
        {
            //Debug.Log("resetting animator");
            animator.speed = currentPlayer.GetComponent<CharacterStats>().animatorSpeed;
        }

        hitStopping = false;
    }

    public void ClearEnemyList()
    {
        enemiesHit.Clear();
    }
    private void CheckForSkills()
    {
        if (GameObject.Find("SkillLoadout").GetComponentInChildren<RelentlessFury>() != null)
        {
            relentlessFury = GameObject.Find("SkillLoadout").GetComponentInChildren<RelentlessFury>();
        }
    }
}
