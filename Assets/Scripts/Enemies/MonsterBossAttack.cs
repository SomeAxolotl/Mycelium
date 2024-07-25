using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBossAttack : MonoBehaviour
{
    Transform player;
    Rigidbody rb;
    Animator animator;
    [HideInInspector] public List<GameObject> playerHit = new List<GameObject>();

    [SerializeField] private BossMeleeHitbox leftArmHitbox;
    [SerializeField] private BossMeleeHitbox rightArmHitbox;
    [SerializeField] private BossMeleeHitbox spineHitbox;

    [SerializeField] private GameObject bossTail;
    [Space(10)]
    [SerializeField] private float pullForce;
    [SerializeField] private float pullDuration;
    [SerializeField] private float pullCooldown;
    [Space(10)]
    [SerializeField] private float tailCooldown;
    [SerializeField] private float spinCooldown;
    [Space(10)]
    [SerializeField] private float cooldownAfterSlam;
    [SerializeField] private float cooldownAfterSwipe;
    [SerializeField] private float cooldownAfterSpin;
    [Space(10)]
    [SerializeField] private float tailAttackDamage;
    [SerializeField] private float swipeAttackDamage;
    [SerializeField] private float slamAttackDamage;
    [SerializeField] private float spinAttackDamage;

    [HideInInspector] public bool isAttacking = false;
    private bool isSpinning = false;
    private bool canDoSpin = true;
    private float spinSpeed;

    private int numberofAttacks = 3;

    private float swipeHitboxActivationDelay = 2.0f;
    private float slamHitboxActivationDelay = 1.1f;
    private float swipeAttackAnimationDuration = 2.7f;
    private float slamAttackAnimationDuration = 1.7f;

    // Start is called before the first frame update
    void OnEnable()
    {
        if (GameObject.FindWithTag("currentPlayer") != null )
        {
            player = GameObject.FindWithTag("currentPlayer").transform;
        }
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        InvokeRepeating("PullPlayer", 5f, (pullCooldown + pullDuration));
        InvokeRepeating("DoTailAttack", 8f, (tailCooldown + 5.0f)); // 5 sec buffer for when tail attack is actually happening
    }

    private void FixedUpdate()
    {
        if (isSpinning == true)
        {
            transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime);
        }
    }

    public void DoRandomAttack() // Picks either slam or swipe attack
    {
        int randomAttack = Random.Range(0, numberofAttacks);

        switch(randomAttack)
        {
            case 0:
                StartCoroutine(SwipeAttack());
                break;

            case 1:
                StartCoroutine(SlamAttack());
                break;

            case 2:
                if(canDoSpin == true)
                {
                    StartCoroutine(SpinAttack());
                }
                else
                {
                    DoRandomAttack();
                }
                break;
        }
    }

    public IEnumerator WaitForIntroToEnd()
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Intro") == false);

        yield return new WaitForSeconds(2f);

        DoRandomAttack();
    }

    private void PullPlayer()
    {
        StartCoroutine(ApplyPullForce());
    }

    private IEnumerator ApplyPullForce()
    {
        ParticleManager.Instance.SpawnParticles("BossSandPullParticles", gameObject.transform.position + new Vector3(0, 0.25f, 0), Quaternion.Euler(-90, 0, 0));
        float elapsedTime = 0.0f;
        while (elapsedTime < pullDuration && !GlobalData.isGamePaused)
        {
            Vector3 direction = (new Vector3(transform.position.x, player.position.y - 2f, transform.position.z) - player.position).normalized;
            float distance = direction.magnitude;

            float strength = (1.0f - distance / 20f) * this.pullForce;
            Vector3 pullForce = (direction * strength);
            player.GetComponent<Rigidbody>().AddForce(pullForce, ForceMode.Force); // Pulls player towards the center of the boss

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void DoTailAttack()
    {
        StartCoroutine(TailAttack());
    }

    private IEnumerator TailAttack()
    {
        Vector3 spawnPosition = player.position + new Vector3(0f, -4.5f, 0f);
        Quaternion tailRotation = Quaternion.Euler(-21.7f, 0f, 0f);
        GameObject bossTailInstance = Instantiate(bossTail, spawnPosition, tailRotation, null); //Spawns tail under player
        bossTailInstance.GetComponentInChildren<TailCollision>().damage = tailAttackDamage * GlobalData.currentLoop;
        ParticleSystem tailRisePart = ParticleManager.Instance.SpawnParticlesAndGetParticleSystem("TailRisingParticle", spawnPosition + new Vector3(0f, 5f, 0f), Quaternion.Euler(-90, 0, 0));

        yield return new WaitForSeconds(1.75f);

        float timeElapsed = 0f;
        float duration = 0.5f;
        Vector3 startPosition = bossTailInstance.transform.position;
        Vector3 endPosition = bossTailInstance.transform.position + new Vector3(0f, 4f, 0f);

        while (timeElapsed < duration)
        {
            bossTailInstance.transform.position = Vector3.Lerp(startPosition, endPosition, timeElapsed / duration); // Tail shoots up
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        tailRisePart.Stop();
        yield return new WaitForSeconds(1f);

        float timeElapsed_02 = 0f;
        float duration_02 = 1f;
        while (timeElapsed_02 < duration_02)
        {
            bossTailInstance.transform.position = Vector3.Lerp(endPosition, startPosition + new Vector3(0f, -3f, 0f), timeElapsed_02 / duration_02); // Tail retracts back down
            timeElapsed_02 += Time.deltaTime;
            yield return null;
        }

        Destroy(bossTailInstance);
    }

    private IEnumerator SwipeAttack()
    {
        isAttacking = true;
        animator.SetBool("IsAttacking", true);

        int randomSwipeAttack = Random.Range(0, 2);

        if (randomSwipeAttack == 0)
        {
            animator.SetTrigger("AttackLeft");
            leftArmHitbox.hitboxActivateDelay = swipeHitboxActivationDelay;
            leftArmHitbox.damage = swipeAttackDamage * GlobalData.currentLoop;
            leftArmHitbox.StartCoroutine(leftArmHitbox.ActivateHitbox());
        }
        else if (randomSwipeAttack == 1)
        {
            animator.SetTrigger("AttackRight");
            rightArmHitbox.hitboxActivateDelay = swipeHitboxActivationDelay;
            rightArmHitbox.damage = swipeAttackDamage * GlobalData.currentLoop;
            rightArmHitbox.StartCoroutine(rightArmHitbox.ActivateHitbox());
        }
        yield return new WaitForEndOfFrame();
        animator.SetBool("IsAttacking", false);
        //Debug.Log("SWIPE ATTACK!");

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f);

        isAttacking = false;
        playerHit.Clear();

        yield return new WaitForSeconds(cooldownAfterSwipe);
        DoRandomAttack();
    }

    private IEnumerator SlamAttack()
    {
        isAttacking = true;
        animator.SetBool("IsAttacking", true);
        animator.SetTrigger("Smash");
        leftArmHitbox.hitboxActivateDelay = slamHitboxActivationDelay;
        leftArmHitbox.damage = slamAttackDamage * GlobalData.currentLoop;
        rightArmHitbox.hitboxActivateDelay = slamHitboxActivationDelay;
        rightArmHitbox.damage = slamAttackDamage * GlobalData.currentLoop;
        leftArmHitbox.StartCoroutine(leftArmHitbox.ActivateHitbox());
        rightArmHitbox.StartCoroutine(rightArmHitbox.ActivateHitbox());
        yield return new WaitForEndOfFrame();
        animator.SetBool("IsAttacking", false);
        //Debug.Log("SLAM ATTACK!");

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f);

        isAttacking = false;
        playerHit.Clear();

        yield return new WaitForSeconds(cooldownAfterSlam);
        DoRandomAttack();
    }

    private IEnumerator SpinAttack()
    {
        isAttacking = true;
        animator.SetBool("IsAttacking", true);
        animator.SetTrigger("Spin");
        yield return new WaitForEndOfFrame();
        animator.SetBool("IsAttacking", false);

        spinSpeed = 0f;
        isSpinning = true;
        yield return StartCoroutine(ChangeSpinSpeed(-400, 1f));
        StartCoroutine(ChangeSpinSpeed(-800, 2f));

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.43f);

        leftArmHitbox.InstantHitboxToggle(true);
        leftArmHitbox.damage = spinAttackDamage * GlobalData.currentLoop;
        rightArmHitbox.InstantHitboxToggle(true);
        rightArmHitbox.damage = spinAttackDamage * GlobalData.currentLoop;
        spineHitbox.InstantHitboxToggle(true);
        spineHitbox.damage = spinAttackDamage * GlobalData.currentLoop;

        ParticleSystem spinPartL = ParticleManager.Instance.SpawnParticlesAndGetParticleSystem("SpinAttack", leftArmHitbox.transform.position, Quaternion.Euler(0, 0, 0), leftArmHitbox.gameObject);
        ParticleSystem spinPartR = ParticleManager.Instance.SpawnParticlesAndGetParticleSystem("SpinAttack", rightArmHitbox.transform.position, Quaternion.Euler(0, 0, 0), rightArmHitbox.gameObject);

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.87f);

        leftArmHitbox.InstantHitboxToggle(false);
        rightArmHitbox.InstantHitboxToggle(false);
        spineHitbox.InstantHitboxToggle(false);
        StartCoroutine(ChangeSpinSpeed(0, 1f));

        spinPartL.Stop();
        spinPartR.Stop();

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f);

        spinSpeed = 0f;
        isAttacking = false;
        isSpinning = false;
        canDoSpin = false;
        playerHit.Clear();

        yield return new WaitForSeconds(cooldownAfterSpin);
        DoRandomAttack();

        yield return new WaitForSeconds(spinCooldown);
        canDoSpin = true;
    }

    private IEnumerator ChangeSpinSpeed(float targetSpeed, float timeToChange)
    {
        float elapsedTime = 0f;
        float t;
        float initialSpinSpeed = spinSpeed;

        while (elapsedTime < timeToChange)
        {
            t = elapsedTime / timeToChange;

            spinSpeed = Mathf.Lerp(initialSpinSpeed, targetSpeed, t);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        spinSpeed = targetSpeed;
    }

    public void OnDisable()
    {
        StopAllCoroutines();
        CancelInvoke("PullPlayer");
        CancelInvoke("DoTailAttack");
        isAttacking = false;
    }
}


