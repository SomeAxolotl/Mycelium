using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyProjectile : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private GameObject ExplosionVFX;
    [SerializeField] private Material negativeVelocityMaterial;
    [SerializeField] private LayerMask collidableLayers;
    float gravityForce = 9.3f;
    Vector3 gravity;
    Rigidbody rb;
    Renderer rend;
    bool enemyCollisionOccurred = false;
    public CharacterStats primalLevel;
    Spineshot spineshot;

    private void Start()
    {
        primalLevel = GetComponent<CharacterStats>();
        Destroy(gameObject, 5f);
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
            collision.GetComponentInParent<PlayerHealth>().PlayerTakeDamage(damage * GlobalData.currentLoop);
            Instantiate(ExplosionVFX, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == 8 && !collision.isTrigger || collision.gameObject.layer == 12 && !collision.isTrigger || collision.gameObject.layer == 0 && !collision.isTrigger)
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == 10)
        {
            gameObject.tag = "ReversedProjectile";
            enemyCollisionOccurred = false;
            rb.velocity = -rb.velocity * 2.5f;

            rend.material = negativeVelocityMaterial;
        }
        if (collision.gameObject.tag == "Enemy" && this.gameObject.tag == "ReversedProjectile")
        {
            enemyCollisionOccurred = true;
            if (collision.GetComponent<EnemyHealth>() != null)
            {

                collision.GetComponentInParent<EnemyHealth>().EnemyTakeDamage(damage * GlobalData.currentLoop);
            }
            Instantiate(ExplosionVFX, transform.position, transform.rotation);
            Destroy(gameObject);

        }

    }
}
