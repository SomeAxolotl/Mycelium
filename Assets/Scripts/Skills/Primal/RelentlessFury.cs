using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RelentlessFury : Skill
{
    //Skill specific fields
    [HideInInspector] public float durationTime = 500f;
    [SerializeField] private float healPercentage = 0.25f;
    [SerializeField] private float loseHP;
    [SerializeField] private float increaseSpeedPercentage;
    public GameObject relentlessFuryParticles;
    public bool isFrenzied;
    public float originalSpeed;

    public override void DoSkill()
    {
        //Skill specific stuff
        Frenzied();
        EndSkill();
    }

    void InstantiateParticles()
    {
        GameObject player = GameObject.FindWithTag("currentPlayer");
        Instantiate(relentlessFuryParticles, new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z), transform.rotation = Quaternion.Euler(-90f, 0f, 0f), player.transform);
    }

    void Frenzied()
    {
        InstantiateParticles();
        isFrenzied = true;
        originalSpeed = characterStats.moveSpeed;
        float increaseSpeed = characterStats.moveSpeed * increaseSpeedPercentage;
        characterStats.moveSpeed += increaseSpeed;
        StartCoroutine(HurtPlayer());
        playerController.canUseDodge = false;
    }


    void Heal()
    {
        if (GameObject.FindWithTag("Enemy") != null)
        {
            GameObject enemyObject = GameObject.FindWithTag("Enemy");
            EnemyHealth enemyHealth = enemyObject.GetComponent<EnemyHealth>();

            if (enemyHealth != null && enemyHealth.HasTakenDamage())
            {
                Debug.Log("Enemy has taken damage. Attempting to heal the player.");

                GameObject playerObj = GameObject.FindWithTag("PlayerParent");

                if (playerObj != null)
                {
                    PlayerAttack playerAttack = playerObj.GetComponent<PlayerAttack>();

                    if (playerAttack != null)
                    {
                        WeaponStats weaponStats = playerAttack.curWeapon.GetComponent<WeaponStats>();

                        if (weaponStats != null)
                        {
                            float weaponDmg = weaponStats.wpnDamage;
                            float heal = weaponDmg * healPercentage;

                            playerHealth.PlayerHeal(heal);
                            Debug.Log("Player healed successfully.");
                        }
                        else
                        {
                            Debug.Log("WeaponStats component not found on the player's weapon.");
                        }
                    }
                    else
                    {
                        Debug.Log("PlayerAttack component not found on the player.");
                    }
                }
                else
                {
                    Debug.Log("Player GameObject not found with tag 'player'");
                }
            }
            else
            {
                Debug.Log("Enemy has not taken damage, so the player is not healed.");
            }
        }
        else
        {
            Debug.Log("Enemy GameObject not found with tag 'Enemy'.");
        }
    }

    IEnumerator HurtPlayer()
    {
        float timer = 0f;
        while (timer < durationTime)
        {
            float damageToTake = loseHP * Time.deltaTime;

            playerHealth.PlayerTakeDamage(damageToTake);

            if (IsPlayerTakingDamage())
            {
                Heal();
            }

            yield return null;
            timer += Time.deltaTime;
        }
    }
    
    bool IsPlayerTakingDamage()
    {
        return playerHealth.currentHealth < playerHealth.maxHealth;
    }
}
