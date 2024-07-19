using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Adaptive;

public class RangedEnemyProjectile : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private GameObject ExplosionVFX;
    [SerializeField] private Material negativeVelocityMaterial;
    [SerializeField] private LayerMask collidableLayers;
    private EnemyHealth instantiatorEnemyHealth; // Reference to the instantiator's EnemyHealth
    private RangedEnemyShoot instantiatorShoot; // Reference to the instantiator's RangedEnemyShoot
    float gravityForce = 9.3f;
    Vector3 gravity;
    Rigidbody rb;
    Renderer rend;
    bool enemyCollisionOccurred = false;
    public CharacterStats primalLevel;
    Spineshot spineshot;
    private float damageBuffMultiplier = 1f;

    private void Start()
    {
        primalLevel = GetComponent<CharacterStats>();
        //Instead of destroy does some other things to be a bit more consistant 
        StartCoroutine(DelayedHide());
        gravity = new Vector3(0f, gravityForce, 0f);
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        spineshot = GameObject.FindWithTag("currentPlayer").GetComponentInChildren<Spineshot>();
    }
    private void FixedUpdate()
    {
        rb.AddForce(gravity, ForceMode.Acceleration);
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "currentPlayer" && collision.GetComponentInParent<PlayerController>().isInvincible == false)
        {
            float finalDamage = damage * GlobalData.currentLoop * damageBuffMultiplier;
            Debug.Log("Dmg Multi: " + damageBuffMultiplier);
            collision.GetComponentInParent<PlayerHealth>().PlayerTakeDamage(finalDamage);
            if (instantiatorEnemyHealth != null)
            {
                instantiatorEnemyHealth.OnDamageDealt(finalDamage); // Heal the instantiator enemy
            }
            StartCoroutine(DelayedHide(0));
        }
        else if (collision.gameObject.layer == 8 && !collision.isTrigger || collision.gameObject.layer == 12 && !collision.isTrigger || collision.gameObject.layer == 0 && !collision.isTrigger)
        {
            StartCoroutine(DelayedHide(0));
        }
        else if (collision.gameObject.layer == 10)
        {
            ReverseProjectile();
        }
        else if (collision.gameObject.name == "Council")
        {
            ScaleProjectile();
        }
        if (collision.gameObject.tag == "Enemy" && this.gameObject.tag == "ReversedProjectile")
        {
            enemyCollisionOccurred = true;
            if (collision.GetComponent<EnemyHealth>() != null)
            {

                collision.GetComponentInParent<EnemyHealth>().EnemyTakeDamage(damage * GlobalData.currentLoop);
            }
            StartCoroutine(DelayedHide(0));

        }
    }

    public void ReverseProjectile(){
        gameObject.tag = "ReversedProjectile";
        enemyCollisionOccurred = false;
        rb.velocity = -rb.velocity * 2.5f;

        rend.material = negativeVelocityMaterial;
    }
    private void ScaleProjectile()
    {
        transform.localScale *= 2; // Scale the projectile to twice its size, adjust the multiplier as needed
    }
    public void SetInstantiatorEnemyHealth(EnemyHealth enemyHealth)
    {
        instantiatorEnemyHealth = enemyHealth;
    }

    private void OnDestroy()
    {
        if (instantiatorShoot != null)
        {
            instantiatorShoot.RemoveProjectile(this); // Pass the RangedEnemyProjectile instance
        }
    }
        public void SetInstantiatorShoot(RangedEnemyShoot shoot)
    {
        instantiatorShoot = shoot; // Set the reference to the RangedEnemyShoot script
    }



    public void SetDamageBuffMultiplier(float multiplier)
    {
        damageBuffMultiplier = multiplier;
    }

    public void RemoveDamageBuff()
    {
        damageBuffMultiplier = 1f;
    }

    public IEnumerator DelayedHide(float time = 6){
        yield return new WaitForSeconds(time);
        Instantiate(ExplosionVFX, transform.position, transform.rotation);
        TrailRenderer trail = GetComponentInChildren<TrailRenderer>();
        if(trail != null){
            trail.time = 0.425f;
        }
        rb.isKinematic = true;
        rend.enabled = false;
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 2);
    }
}
