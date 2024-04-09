using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UPProjectile : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] private float AOERange = 2;
    [SerializeField] private float speed = 12f;
    [SerializeField] private float upRange = 2f;
    UnstablePuffball unstablePuffball;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        unstablePuffball = GameObject.FindWithTag("currentPlayer").GetComponentInChildren<UnstablePuffball>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 launchDirection = (transform.up * upRange + transform.forward).normalized;
        transform.position += launchDirection * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Boss")
        {
            ParticleManager.Instance.SpawnParticles("PuffballParticles", transform.position, Quaternion.identity);
            DamageEnemies();
        }

        int enviornmentLayer = 8;
        int wallLayer = 12;
        if (collision.gameObject.layer == enviornmentLayer || collision.gameObject.layer == wallLayer)
        {
            ParticleManager.Instance.SpawnParticles("PuffballParticles", transform.position, Quaternion.identity);
            DamageEnemies();
        }
    }

    void DamageEnemies()
    {
        SoundEffectManager.Instance.PlaySound("Explosion", transform.position);

        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");
        int bossLayerMask = 1 << LayerMask.NameToLayer("Boss");

        Collider[] colliders = Physics.OverlapSphere(transform.position, AOERange, enemyLayerMask | bossLayerMask);
        
        foreach (Collider collider in colliders)
        {
            if (collider.GetComponent<EnemyHealth>() != null)
            {
                collider.GetComponent<EnemyHealth>().EnemyTakeDamage(unstablePuffball.finalSkillValue);
            }
            else if (collider.GetComponent<BossHealth>() != null)
            {
                collider.GetComponent<BossHealth>().EnemyTakeDamage(unstablePuffball.finalSkillValue);
            }
        }
        Destroy(gameObject);
    }
}
