using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMeleeHitbox : MonoBehaviour
{
    [SerializeField] private MonsterBossAttack monsterBossAttack;
    [HideInInspector] public float hitboxActivateDelay;
    [HideInInspector] public float knockbackForce;
    [HideInInspector] public float damage;
    Cinemachine.CinemachineImpulseSource impulseSource;
    private EnemyHealth bossHealth;
    // Start is called before the first frame update
    void Start()
    {
        impulseSource = GetComponentInParent<Cinemachine.CinemachineImpulseSource>();
        bossHealth = GetComponentInParent<BossHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bossHealth.currentHealth <= 0f)
        {
            StopAllCoroutines(); //TEMPORARY FIX
        }
    }

    public void InstantHitboxToggle(bool setActive)
    {
        gameObject.GetComponent<Collider>().enabled = setActive;
        //gameObject.GetComponent<Renderer>().enabled = setActive;
    }

    public IEnumerator ActivateHitbox()
    {
        yield return new WaitForSeconds(hitboxActivateDelay);
        gameObject.GetComponent<Collider>().enabled = true;
        //gameObject.GetComponent<Renderer>().enabled = true;
        yield return new WaitForSeconds(0.3f);
        gameObject.GetComponent<Collider>().enabled = false;
        //gameObject.GetComponent<Renderer>().enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "currentPlayer" && !other.gameObject.GetComponentInParent<PlayerController>().isInvincible && !monsterBossAttack.playerHit.Contains(other.gameObject))
        {
            other.gameObject.GetComponentInParent<PlayerHealth>().PlayerTakeDamage(damage);
            other.gameObject.GetComponentInParent<PlayerController>().Knockback(this.gameObject, knockbackForce);
            monsterBossAttack.playerHit.Add(other.gameObject);
        }
    }
    public void OnDisable()
    {
        StopAllCoroutines();
    }
}
