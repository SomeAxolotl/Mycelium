using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Adaptive;

public class CrabMeleeHitbox : MonoBehaviour, IDamageBuffable
{
    [HideInInspector] public float knockbackForce;
    [HideInInspector] public float damage;
    [HideInInspector] public List<GameObject> playerHit = new List<GameObject>();
    [SerializeField] private GameObject particles;
    [SerializeField] private Transform particleHolder;
    Cinemachine.CinemachineImpulseSource impulseSource;
    private EnemyHealth enemyHealth;
    private float damageBuffMultiplier = 1f;
    // Start is called before the first frame update
    void Start()
    {
        impulseSource = GetComponentInParent<Cinemachine.CinemachineImpulseSource>();
        enemyHealth = GetComponentInParent<EnemyHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyHealth.currentHealth <= 0f)
        {
            StopAllCoroutines(); //TEMPORARY FIX
        }
    }
    public IEnumerator ActivateHitbox()
    {
        CameraShakeManager.instance.ShakeCamera(impulseSource);
        ParticleManager.Instance.SpawnParticles("SmashParticle", particleHolder.position, Quaternion.Euler(-90, 0, 0));
        SoundEffectManager.Instance.PlaySound("Explosion", transform);
        gameObject.GetComponent<Collider>().enabled = true;
        //gameObject.GetComponent<Renderer>().enabled = true;
        yield return new WaitForSeconds(0.3f);
        gameObject.GetComponent<Collider>().enabled = false;
        //gameObject.GetComponent<Renderer>().enabled = false;
        playerHit.Clear();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "currentPlayer" && !other.gameObject.GetComponentInParent<PlayerController>().isInvincible && !playerHit.Contains(other.gameObject) && !enemyHealth.alreadyDead)
        {
            float dmgDealt = damage * GlobalData.currentLoop * damageBuffMultiplier;
            other.gameObject.GetComponentInParent<PlayerHealth>().PlayerTakeDamage(dmgDealt);
            other.gameObject.GetComponentInParent<PlayerController>().Knockback(this.gameObject, knockbackForce);
            playerHit.Add(other.gameObject);
            if (enemyHealth != null)
            {
                enemyHealth.OnDamageDealt(dmgDealt);
            }
            else
            {
                Debug.LogError("EnemyHealth component is not assigned on " + gameObject.name);
            }
        }
    }
    public void ApplyDamageBuff(float multiplier, float duration)
    {
        StartCoroutine(DamageBuffCoroutine(multiplier, duration));
    }

    private IEnumerator DamageBuffCoroutine(float multiplier, float duration)
    {
        damageBuffMultiplier = multiplier;
        yield return new WaitForSeconds(duration);
        RemoveDamageBuff();
    }

    public void RemoveDamageBuff()
    {
        damageBuffMultiplier = 1f;
    }
}
