using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanoProjectile : MonoBehaviour
{
    private Rigidbody rb;
    private Transform player;
    [SerializeField] private float damage = 50f;
    [SerializeField] private GameObject particles;
    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("currentPlayer").transform;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "currentPlayer" && collision.GetComponentInParent<PlayerController>().isInvincible == false)
        {
            collision.GetComponentInParent<PlayerHealth>().PlayerTakeDamage(damage * GlobalData.currentLoop);
            ParticleManager.Instance.SpawnParticles("SmashParticle", transform.position, Quaternion.Euler(-90, 0, 0));
            if (this.gameObject.tag == "Enemy")
            {
                return;
            }
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == 8 || collision.gameObject.layer == 12)
        {
            ParticleManager.Instance.SpawnParticles("SmashParticle", transform.position, Quaternion.Euler(-90, 0, 0));
            SoundEffectManager.Instance.PlaySound("Explosion", transform);
            if (this.gameObject.tag == "Enemy")
            {
                return;
            }
            Destroy(gameObject);
        }

    }
}
