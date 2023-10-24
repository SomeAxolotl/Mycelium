using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewPlayerAttack : MonoBehaviour
{
    private ThirdPersonActionsAsset playerActionsAsset;
    private InputAction attack;
    private SwapCharacter swapCharacter;
    bool canAttack = true;
    public bool attacking = false;
    public float dmgDealt;
    float atkCooldown;
    // Start is called before the first frame update
    void Start()
    {
        playerActionsAsset = new ThirdPersonActionsAsset();
        playerActionsAsset.Player.Enable();
        swapCharacter = GetComponent<SwapCharacter>();
        attack = playerActionsAsset.Player.Attack;
    }

    // Update is called once per frame
    void Update()
    {

        if (attack.triggered && canAttack)
        {
            GameObject curWeapon = GameObject.FindWithTag("currentWeapon");
            //Attack(curWeapon);
            atkCooldown = curWeapon.GetComponent<WeaponStats>().weaponAtkCooldown - swapCharacter.currentCharacterStats.atkCooldownBuff;
            dmgDealt = swapCharacter.currentCharacterStats.primalDmg + curWeapon.GetComponent<WeaponStats>().weaponDmg;
            StartCoroutine(AttackCooldown());
            StartCoroutine(ActiveAttack(curWeapon));
        }
    }
    void Attack(GameObject curWeapon)
    {  
        //atkCooldown = curWeapon.GetComponent<WeaponStats>().weaponAtkCooldown - swapCharacter.currentCharacterStats.atkCooldownBuff;
        //dmgDealt = swapCharacter.currentCharacterStats.primalDmg + curWeapon.GetComponent<WeaponStats>().weaponDmg;
    }
    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(atkCooldown);
        canAttack = true;
    }
    private IEnumerator ActiveAttack(GameObject curWeapon)
    {
        attacking = true;
        curWeapon.GetComponent<Collider>().enabled = true;
        yield return new WaitForSeconds(.6f); //How long the hitbox is active
        attacking = false;
        curWeapon.GetComponent<Collider>().enabled = false;
    }
}
