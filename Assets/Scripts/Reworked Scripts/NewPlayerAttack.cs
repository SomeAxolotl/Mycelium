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
    public float atkCooldown;
    private float fungalMightBonus = 1f;
    private float lungeDuration = 0.4f;
    private float lungeForce = 20f;
    private HUDSkills hudSkills;
    Animator animator;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        playerActionsAsset = new ThirdPersonActionsAsset();
        playerActionsAsset.Player.Enable();
        swapCharacter = GetComponent<SwapCharacter>();
        attack = playerActionsAsset.Player.Attack;
        hudSkills = GameObject.Find("HUD").GetComponent<HUDSkills>();
        animator = GetComponentInChildren<Animator>();
        player = GameObject.FindWithTag("currentPlayer");
    }

    // Update is called once per frame
    void Update()
    {

        if (attack.triggered && canAttack)
        {
            canAttack = false;
            StartAttack();
        }
        Debug.Log("lungeForce: " + lungeForce);

    }

    private void StartAttack()
    {
        GameObject curWeapon = GameObject.FindWithTag("currentWeapon");
        player = GameObject.FindWithTag("currentPlayer");
        atkCooldown = curWeapon.GetComponent<NewWeaponStats>().wpnCooldown - swapCharacter.currentCharacterStats.atkCooldownBuff;
        dmgDealt = (swapCharacter.currentCharacterStats.primalDmg + curWeapon.GetComponent<NewWeaponStats>().wpnDamage) * fungalMightBonus;
        animator = GetComponentInChildren<Animator>();
        StartCoroutine(AttackCooldown());
        StartCoroutine(Attack(curWeapon));
        StartCoroutine(Lunge());
    }

    private IEnumerator AttackCooldown()
    {
        hudSkills.StartCooldownUI(3, atkCooldown);

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

        yield return new WaitForEndOfFrame();
        yield return new WaitUntil (() => !animator.GetCurrentAnimatorStateInfo(0).IsName("Slash"));
        yield return new WaitUntil(() => !animator.GetCurrentAnimatorStateInfo(0).IsName("Smash"));
        yield return new WaitUntil(() => !animator.GetCurrentAnimatorStateInfo(0).IsName("Stab"));
        attacking = false;
        curWeapon.GetComponent<Collider>().enabled = false;
        ClearAllFungalMights();
    }
    private IEnumerator Lunge()
    {
        Vector3 lungeDirection = player.transform.forward;
        float forcePerSecond = lungeForce / lungeDuration;
        float elapsedTime = 0f;
        while (elapsedTime < lungeDuration)
        {
            elapsedTime += Time.deltaTime;
            player.GetComponent<Rigidbody>().AddForce(lungeDirection * forcePerSecond * Time.deltaTime, ForceMode.Impulse);
            yield return new WaitForFixedUpdate();
        }
    }

    //Fungal Might for Attacking
    public void ActivateFungalMight(float fungalMightValue)
    {   
        fungalMightBonus = fungalMightValue;
    }
    public void DeactivateFungalMight()
    {
        fungalMightBonus = 1f;
    }

    //Clears Fungal Might for attacking and skills
    public void ClearAllFungalMights()
    {
        GameObject skillLoadout = GameObject.FindWithTag("currentPlayer").transform.Find("SkillLoadout").gameObject;
        foreach (Transform child in skillLoadout.transform)
        {
            Skill skill = child.gameObject.GetComponent<Skill>();
            skill.DeactivateFungalMight();
        }

        DeactivateFungalMight();

        GameObject[] fungalMightParticles = GameObject.FindGameObjectsWithTag("FungalMightParticles");
        foreach (GameObject particle in fungalMightParticles)
        {
            particle.GetComponent<ParticleSystem>().Stop();
        }
    }
}
