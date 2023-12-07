using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
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
    [SerializeField][Tooltip("Controls what percent of the attack animation the weapon collider enables at")] float percentUntilWindupDone = 0.5f;
    [SerializeField][Tooltip("Controls what percent of the attack animation the weapon collider disables at")] float percentUntilSwingDone = 0.9f;

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
        playerController = GetComponent<PlayerController>();
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
        if (playerController.canAct == true)
        {
            GameObject curWeapon = GameObject.FindWithTag("currentWeapon");
            WeaponCollision weaponCollision = curWeapon.GetComponent<WeaponCollision>();
            weaponCollision.ClearEnemyList();
            player = GameObject.FindWithTag("currentPlayer");
            //atkCooldown = curWeapon.GetComponent<WeaponStats>().wpnCooldown - swapCharacter.currentCharacterStats.atkCooldownBuff;
            dmgDealt = (swapCharacter.currentCharacterStats.primalDmg + curWeapon.GetComponent<WeaponStats>().wpnDamage) * fungalMightBonus;
            animator = GetComponentInChildren<Animator>();
            //StartCoroutine(AttackCooldown());
            StartCoroutine(Attack(curWeapon));
            StartCoroutine(Lunge());
        }
    }

    /*private IEnumerator AttackCooldown()
    {
        hudSkills.StartCooldownUI(3, atkCooldown);

        yield return new WaitForSeconds(atkCooldown);
        canAttack = true;
    }*/

    private IEnumerator Attack(GameObject curWeapon)
    {
        playerController.DisableController();

        animator.Play(attackAnimationName);
        SoundEffectManager.Instance.PlaySound(attackAnimationName, curWeapon.transform.position);


        // play smash animation
        // animator.Play("Smash");

        // play stab animation
        // animator.Play("Stab");

        lungeDuration = (animator.GetCurrentAnimatorStateInfo(0).length * animator.speed) * lungeDurationScalar;
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil (() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > percentUntilWindupDone);
        curWeapon.GetComponent<Collider>().enabled = true;
        yield return new WaitUntil (() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > percentUntilSwingDone);
        curWeapon.GetComponent<Collider>().enabled = false;

        /*yield return new WaitForEndOfFrame();
        yield return new WaitUntil (() => !animator.GetCurrentAnimatorStateInfo(0).IsName("Slash"));
        yield return new WaitUntil(() => !animator.GetCurrentAnimatorStateInfo(0).IsName("Smash"));
        yield return new WaitUntil(() => !animator.GetCurrentAnimatorStateInfo(0).IsName("Stab"));*/
        playerController.EnableController();
        ClearAllFungalMights();

    }
    private IEnumerator Lunge()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > percentUntilWindupDone);
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
