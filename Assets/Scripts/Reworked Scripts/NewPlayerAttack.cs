using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
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
    public float atkCooldown;
    Animator animator;
    public GameObject characterPrefab;

    private HUDSkills hudSkills;

    // Start is called before the first frame update
    void Start()
    {
        playerActionsAsset = new ThirdPersonActionsAsset();
        playerActionsAsset.Player.Enable();
        swapCharacter = GetComponent<SwapCharacter>();
        attack = playerActionsAsset.Player.Attack;

        hudSkills = GameObject.Find("HUD").GetComponent<HUDSkills>();

        animator = characterPrefab.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (attack.triggered && canAttack)
        {
            GameObject curWeapon = GameObject.FindWithTag("currentWeapon");

            atkCooldown = curWeapon.GetComponent<NewWeaponStats>().wpnCooldown - swapCharacter.currentCharacterStats.atkCooldownBuff;
            dmgDealt = swapCharacter.currentCharacterStats.primalDmg + curWeapon.GetComponent<NewWeaponStats>().wpnDamage;

            StartCoroutine(AttackCooldown());
            StartCoroutine(Attack(curWeapon));            
        }
    }
    private IEnumerator AttackCooldown()
    {
        hudSkills.StartHitCooldownUI(atkCooldown);

        canAttack = false;
        yield return new WaitForSeconds(atkCooldown);
        canAttack = true;
    }
    private IEnumerator Attack(GameObject curWeapon)
    {
        attacking = true;
        curWeapon.GetComponent<Collider>().enabled = true;

        // play slash animation
        animator.Play("Slash");

        // play smash animation
        // animator.Play("Smash");

        // play stab animation
        // animator.Play("Stab");
        yield return new WaitForSeconds(.6f); //THIS IS WHERE THE ANIMATION WILL GO
        attacking = false;
        curWeapon.GetComponent<Collider>().enabled = false;
    }
}
