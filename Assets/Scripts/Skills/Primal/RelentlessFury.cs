using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RelentlessFury : Skill
{
    //Skill specific fields
    public float durationTime = 5f;
    [SerializeField] private float loseHP;
    [SerializeField] private float gainHP;
    [SerializeField] private float increaseSpeedPercentage;
    public GameObject relentlessFuryParticles;
    public bool isFrenzied;

    public override void DoSkill()
    {
        //Skill specific stuff
        isFrenzied = true;
        InstantiateParticles();
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
        float increaseSpeed = characterStats.moveSpeed * increaseSpeedPercentage;
        characterStats.moveSpeed += increaseSpeed;
        StartCoroutine(HurtPlayer());
        // float heal = player.GetComponent<PlayerAttack>().curWeapon.GetComponent<WeaponStats>().wpnDamage;
        // playerHealth.PlayerHeal(heal * gainHP);
        playerController.canUseDodge = false;
    }

    IEnumerator HurtPlayer()
    {
        float timer = 0f;
        while (timer < durationTime)
        {  
            playerHealth.PlayerTakeDamage(loseHP);
            yield return new WaitForSeconds(1);
            timer++;
        }
    }
}
