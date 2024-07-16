using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Adaptive;

public class MushyWeaponCollision : MonoBehaviour, IDamageBuffable
{
    [HideInInspector] public List<GameObject> playerHit = new List<GameObject>();
    [HideInInspector] public float knockbackForce;
    [HideInInspector] public float damage;
    [SerializeField] private EnemyHealth enemyHealth;
    private EnemyAttributeManager attributeManager; // Reference to the attribute manager
    private float damageBuffMultiplier = 1f;
    // Start is called before the first frame update
    void Start()
    {
        attributeManager = GetComponentInParent<EnemyAttributeManager>(); // Get the attribute manager
    }
    public IEnumerator ActivateHitbox()
    {
        gameObject.GetComponent<Collider>().enabled = true;
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<Collider>().enabled = false;
        playerHit.Clear();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "currentPlayer" && !other.gameObject.GetComponentInParent<PlayerController>().isInvincible && !playerHit.Contains(other.gameObject) && !enemyHealth.alreadyDead)
        {
            float totalDamage = damage * GlobalData.currentLoop * damageBuffMultiplier;
            other.gameObject.GetComponentInParent<PlayerHealth>().PlayerTakeDamage(totalDamage);
            other.gameObject.GetComponentInParent<PlayerController>().Knockback(this.gameObject, knockbackForce);
            playerHit.Add(other.gameObject);
            // Handle healing logic for Parasitic attribute
            enemyHealth.OnDamageDealt(totalDamage);
            
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
