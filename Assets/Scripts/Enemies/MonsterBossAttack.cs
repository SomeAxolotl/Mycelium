using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MonsterBossAttack : MonoBehaviour
{
    Transform player;
    Rigidbody rb;
    Animator animator;
    [HideInInspector] public List<GameObject> playerHit = new List<GameObject>();
    [SerializeField] private BossMeleeHitbox leftArmHitbox;
    [SerializeField] private BossMeleeHitbox rightArmHitbox;
    [SerializeField] private GameObject bossTail;
    [SerializeField] private float pullForce = 100f;
    [SerializeField] private float pullDuration = 5.0f;
    [SerializeField] private float pullCooldown = 5.0f;
    [SerializeField] private float tailCooldown = 5.0f;
    [SerializeField] private float cooldownAfterSlam = 6.0f;
    [SerializeField] private float cooldownAfterSwipe = 4.0f;
    [SerializeField] private float tailAttackDamage = 60f;
    [SerializeField] private float swipeAttackDamage = 85f;
    [SerializeField] private float slamAttackDamage = 70f;
    [HideInInspector] public bool isAttacking = false;
    private int numberofAttacks = 2;
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

    // Update is called once per frame
    void Update()
    {

    }
    public void DoRandomAttack() // Picks either slam or swipe attack
    {
        int randomAttack = Random.Range(0, numberofAttacks);

        if (randomAttack == 0)
        { 
            StartCoroutine(SwipeAttack());
        }
        else if (randomAttack == 1)
        {
            StartCoroutine(SlamAttack());
        }
    }
    private void PullPlayer()
    {
        StartCoroutine(ApplyPullForce());
    }
    private IEnumerator ApplyPullForce()
    {
        ParticleManager.Instance.SpawnParticles("BossSandPullParticles", gameObject.transform.position + new Vector3(0, 0.25f, 0), Quaternion.Euler(-90, 0, 0));
        float elapsedTime = 0.0f;
        while (elapsedTime < pullDuration)
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
        ParticleManager.Instance.SpawnParticles("TrophicCascadePoof", spawnPosition + new Vector3(0f, 2.75f, 0f), Quaternion.Euler(-90, 0, 0));
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
        Debug.Log("SWIPE ATTACK!");
        yield return new WaitForSeconds(swipeAttackAnimationDuration);
        isAttacking = false;
        playerHit.Clear();
        yield return new WaitForSeconds(cooldownAfterSwipe + swipeAttackAnimationDuration);
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
        Debug.Log("SLAM ATTACK!");
        yield return new WaitForSeconds(slamAttackAnimationDuration);
        isAttacking = false;
        playerHit.Clear();
        yield return new WaitForSeconds(cooldownAfterSlam + slamAttackAnimationDuration);
        DoRandomAttack();
    }
    public void OnDisable()
    {
        StopAllCoroutines();
        CancelInvoke("PullPlayer");
        CancelInvoke("DoTailAttack");
        isAttacking = false;
    }
}


