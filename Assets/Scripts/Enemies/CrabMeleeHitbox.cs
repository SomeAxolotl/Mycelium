using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabMeleeHitbox : MonoBehaviour
{
    private float HitboxActivateDelay = 2.25f;
    [HideInInspector] public float knockbackForce;
    [HideInInspector] public float damage;
    [HideInInspector] public List<GameObject> playerHit = new List<GameObject>();
    [SerializeField] private GameObject particles;
    Cinemachine.CinemachineImpulseSource impulseSource;
    private EnemyHealth enemyHealth;
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
        yield return new WaitForSeconds(HitboxActivateDelay);
        CameraShakeManager.instance.ShakeCamera(impulseSource);
        gameObject.GetComponent<Collider>().enabled = true;
        //gameObject.GetComponent<Renderer>().enabled = true;
        yield return new WaitForSeconds(0.3f);
        gameObject.GetComponent<Collider>().enabled = false;
        //gameObject.GetComponent<Renderer>().enabled = false;
        playerHit.Clear();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "currentPlayer" && !other.gameObject.GetComponentInParent<PlayerController>().isInvincible && !playerHit.Contains(other.gameObject))
        {
            other.gameObject.GetComponentInParent<PlayerHealth>().PlayerTakeDamage(damage);
            other.gameObject.GetComponentInParent<PlayerController>().Knockback(this.gameObject, knockbackForce);
            playerHit.Add(other.gameObject);
        }
    }
}
