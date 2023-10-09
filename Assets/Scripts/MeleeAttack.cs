using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeAttack : MonoBehaviour
{
    private ThirdPersonActionsAsset playerActionsAsset;
    private InputAction attack;
    bool canAttack;
    bool attacking;
    float speedAtkCooldown;
    float weaponAtkCooldown;
    public float finalAtkCooldown;
    float primalDmg;
    float weaponDmg;
    public float finalDmg;
    bool swapping;
    bool fetchedStats = false;

    private HUDSkills hudSkills;

    // Start is called before the first frame update
    void Start()
    {
        playerActionsAsset = new ThirdPersonActionsAsset();
        playerActionsAsset.Player.Enable();
        attack = playerActionsAsset.Player.Attack;
        canAttack = true;
        attacking = false;

        hudSkills = GameObject.Find("HUD").GetComponent<HUDSkills>();
    }

    // Update is called once per frame
    void Update()
    {
        swapping = gameObject.GetComponent<Swapping>().swapping;
        
        //Gets the attack and cooldown values of the current player once when the scene first loads and whenever the player switches weapons.
        if(fetchedStats == false || swapping == true)
        {
            StartCoroutine("FetchStats");
        }
        
        if(attack.triggered && canAttack == true)
        {
            StartCoroutine("Attacking");
        }

        //The statement below makes it so that the weapon collider is only active when the attack animation is playing in order to damage the enemy.
        if(attacking == false)
        {
            GameObject.FindWithTag("currentWeapon").GetComponent<Collider>().enabled = false;
        }
        else
        {
            GameObject.FindWithTag("currentWeapon").GetComponent<Collider>().enabled = true;
        }
    }
    IEnumerator FetchStats()
    {
        yield return new WaitForEndOfFrame();

        //The info below gets the stats from the StatTracker script and makes a final damage/cooldown value depending on which weapon the player is holding.
        speedAtkCooldown = gameObject.GetComponent<StatTracker>().speedAtkCooldown;
        weaponAtkCooldown = GameObject.FindWithTag("currentWeapon").GetComponent<WeaponStats>().weaponAtkCooldown;
        finalAtkCooldown = speedAtkCooldown + weaponAtkCooldown;
        
        primalDmg = gameObject.GetComponent<StatTracker>().primalDmg;
        weaponDmg = GameObject.FindWithTag("currentWeapon").GetComponent<WeaponStats>().weaponDmg;
        finalDmg = primalDmg + weaponDmg;
        
        fetchedStats = true;
    }
    IEnumerator Attacking()
    {
        canAttack = false;
        StartCoroutine(Attack()); //Starts the coroutine which has the attack animation
        hudSkills.StartHitCooldownUI();
        yield return new WaitForSeconds(finalAtkCooldown); //The cooldown between attacks
        canAttack = true;
    }
    IEnumerator Attack()
    {
        attacking = true;
        yield return new WaitForSeconds(0.5f); //This is where the attack animation will go and replace the WaitForSeconds, yield return null will go at the end of this IEnumerator
        //Debug.Log("attacking");
        attacking = false;
    }
}
