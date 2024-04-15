using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyProjectile : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private GameObject ExplosionVFX;
    float gravityForce = 9.3f;
    Vector3 gravity;
    Rigidbody rb;

    private void Start()
    {
        damage = damage * GlobalData.currentLoop;
        Destroy(gameObject, 5f);
        gravity = new Vector3(0f, gravityForce, 0f);
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        rb.AddForce(gravity, ForceMode.Acceleration);
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "currentPlayer" && collision.GetComponentInParent<PlayerController>().isInvincible == false)
        {
            collision.GetComponentInParent<PlayerHealth>().PlayerTakeDamage(damage);
            Instantiate(ExplosionVFX, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == 8 && !collision.isTrigger || collision.gameObject.layer == 12 && !collision.isTrigger || collision.gameObject.layer == 0 && !collision.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}
