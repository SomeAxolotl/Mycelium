using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewPlayerAttack : MonoBehaviour
{
    private ThirdPersonActionsAsset playerActionsAsset;
    private InputAction attack;
    private SwapCharacter swapCharacter;
    public float dmgDealt;
    public float atkCooldown;
    private float fungalMightBonus = 1f;
    private float lungeDuration = 0.4f;
    [SerializeField] private float lungeForce = 20f;
    [SerializeField][Tooltip("Is multiplied by the attack animation speed")] private float lungeDurationScalar = 0.25f;
    private HUDSkills hudSkills;
    Animator animator;
    GameObject player;
    PlayerController playerController;

    public string attackAnimationName = "Slash";

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
        playerController = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (attack.triggered && playerController.canAct == true)
        {
            StartAttack();
        }
    }

    private void StartAttack()
    {
<<<<<<< Updated upstream
        if (!attacking)
=======
        if (playerController.canAct == true)
>>>>>>> Stashed changes
        {
            GameObject curWeapon = GameObject.FindWithTag("currentWeapon");
            NewWeaponCollision weaponCollision = curWeapon.GetComponent<NewWeaponCollision>();
            weaponCollision.ClearEnemyList();
            player = GameObject.FindWithTag("currentPlayer");
            atkCooldown = curWeapon.GetComponent<NewWeaponStats>().wpnCooldown - swapCharacter.currentCharacterStats.atkCooldownBuff;
            dmgDealt = (swapCharacter.currentCharacterStats.primalDmg + curWeapon.GetComponent<NewWeaponStats>().wpnDamage) * fungalMightBonus;
            animator = GetComponentInChildren<Animator>();
            //StartCoroutine(AttackCooldown());
            StartCoroutine(Attack(curWeapon));
            StartCoroutine(Lunge());
        }
<<<<<<< Updated upstream
=======
        
>>>>>>> Stashed changes
    }

    /*private IEnumerator AttackCooldown()
    {
        hudSkills.StartCooldownUI(3, atkCooldown);

        yield return new WaitForSeconds(atkCooldown);
<<<<<<< Updated upstream
        canAttack = true;
=======
        canAct = true;
>>>>>>> Stashed changes
    }*/

    private IEnumerator Attack(GameObject curWeapon)
    {   
        playerController.canAct = false;
        playerController.DisableController();
        curWeapon.GetComponent<Collider>().enabled = true;

        animator.Play(attackAnimationName);
        SoundEffectManager.Instance.PlaySound(attackAnimationName, curWeapon.transform.position);

<<<<<<< Updated upstream

        // play smash animation
        // animator.Play("Smash");

        // play stab animation
        // animator.Play("Stab");
=======
        lungeDuration = (animator.GetCurrentAnimatorStateInfo(0).length * animator.speed) * lungeDurationScalar;
>>>>>>> Stashed changes

        lungeDuration = (animator.GetCurrentAnimatorStateInfo(0).length * animator.speed) * lungeDurationScalar;

        yield return new WaitForEndOfFrame();
        yield return new WaitUntil (() => !animator.GetCurrentAnimatorStateInfo(0).IsName(attackAnimationName));
        playerController.canAct = true;
        playerController.EnableController();
        curWeapon.GetComponent<Collider>().enabled = false;
        ClearAllFungalMights();

        //Clear the list of enemies struck

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
