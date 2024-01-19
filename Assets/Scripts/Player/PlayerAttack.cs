using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public GameObject curWeapon;
    private ThirdPersonActionsAsset playerActionsAsset;
    private InputAction attack;
    private SwapCharacter swapCharacter;
    public float dmgDealt;
    private float fungalMightBonus = 1f;
    private float lungeDuration = 0.4f;
    [SerializeField] private float lungeForce = 20f;
    [SerializeField][Tooltip("Is multiplied by the attack animation speed")] private float lungeDurationScalar = 0.25f;
    private HUDSkills hudSkills;
    public Animator animator;
    GameObject player;
    PlayerController playerController;
    [SerializeField][Tooltip("Controls what percent of the attack animation the weapon collider enables at")] float percentUntilWindupDone = 0.5f;
    [SerializeField][Tooltip("Controls what percent of the attack animation the weapon collider disables at")] float percentUntilSwingDone = 0.9f;

    public string attackAnimationName = "Slash";

    public IEnumerator attackstart;
    public IEnumerator lunge;

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
        attackstart = this.Attack(curWeapon);
        lunge = this.Lunge();
    }

    // Update is called once per frame
    void Update()
    {
        if (attack.triggered && playerController.canUseAttack == true)
        {
            StartAttack();
        }
    }

    private void StartAttack()
    {
        if (playerController.canUseAttack == true)
        {
            curWeapon = GameObject.FindWithTag("currentWeapon");
            WeaponCollision weaponCollision = curWeapon.GetComponent<WeaponCollision>();
            weaponCollision.ClearEnemyList();
            player = GameObject.FindWithTag("currentPlayer");
            dmgDealt = (swapCharacter.currentCharacterStats.primalDmg + curWeapon.GetComponent<WeaponStats>().wpnDamage) * fungalMightBonus;
            animator = GetComponentInChildren<Animator>();
            StartCoroutine(Attack(curWeapon));
            StartCoroutine(Lunge());
        }
    }
    private IEnumerator Attack(GameObject curWeapon)
    {
        playerController.canUseSkill = false;
        playerController.canUseAttack = false;
        playerController.moveSpeed = 2f;
        animator.Play(attackAnimationName);
        yield return null;
        SoundEffectManager.Instance.PlaySound(attackAnimationName, curWeapon.transform.position);


        // play smash animation
        // animator.Play("Smash");

        // play stab animation
        // animator.Play("Stab");

        lungeDuration = (animator.GetCurrentAnimatorStateInfo(0).length * animator.speed) * lungeDurationScalar;
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil (() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > percentUntilWindupDone);
        curWeapon.GetComponent<Collider>().enabled = true;
        playerController.moveSpeed = swapCharacter.currentCharacterStats.moveSpeed;
        yield return new WaitUntil (() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > percentUntilSwingDone);
        curWeapon.GetComponent<Collider>().enabled = false;

        /*yield return new WaitForEndOfFrame();
        yield return new WaitUntil (() => !animator.GetCurrentAnimatorStateInfo(0).IsName("Slash"));
        yield return new WaitUntil(() => !animator.GetCurrentAnimatorStateInfo(0).IsName("Smash"));
        yield return new WaitUntil(() => !animator.GetCurrentAnimatorStateInfo(0).IsName("Stab"));*/

        playerController.canUseSkill = true;
        playerController.canUseAttack = true;
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
            yield return null;
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
