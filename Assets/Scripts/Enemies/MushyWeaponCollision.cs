using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushyWeaponCollision : MonoBehaviour
{
    private float HitboxActivateDelay = .3f;
    [HideInInspector] public List<GameObject> playerHit = new List<GameObject>();
    [HideInInspector] public float knockbackForce;
    [HideInInspector] public float damage;
    [SerializeField] private EnemyHealth enemyHealth;
    // Start is called before the first frame update
    void Start()
    {

    }
    public IEnumerator ActivateHitbox()
    {
        yield return new WaitForSeconds(HitboxActivateDelay);
        gameObject.GetComponent<Collider>().enabled = true;
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<Collider>().enabled = false;
        playerHit.Clear();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "currentPlayer" && !other.gameObject.GetComponentInParent<PlayerController>().isInvincible && !playerHit.Contains(other.gameObject) && !enemyHealth.alreadyDead)
        {
            other.gameObject.GetComponentInParent<PlayerHealth>().PlayerTakeDamage(damage * GlobalData.currentLoop);
            other.gameObject.GetComponentInParent<PlayerController>().Knockback(this.gameObject, knockbackForce);
            playerHit.Add(other.gameObject);
        }
    }
}
