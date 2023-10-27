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

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        playerActionsAsset = new ThirdPersonActionsAsset();
        playerActionsAsset.Player.Enable();
        swapCharacter = GetComponent<SwapCharacter>();
        attack = playerActionsAsset.Player.Attack;

        animator = GetComponent<Animator>();
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
        canAttack = false;
        yield return new WaitForSeconds(atkCooldown);
        canAttack = true;
    }
    private IEnumerator Attack(GameObject curWeapon)
    {
        attacking = true;
        curWeapon.GetComponent<Collider>().enabled = true;

        if(gameObject.tag == "currentWeapon" && gameObject.name == "BoneSlash")
        {
            animator.Play("Slash");
            Debug.Log("Animation is playing");
        }

        yield return new WaitForSeconds(.6f); //THIS IS WHERE THE ANIMATION WILL GO
        attacking = false;
        curWeapon.GetComponent<Collider>().enabled = false;
    }
}
