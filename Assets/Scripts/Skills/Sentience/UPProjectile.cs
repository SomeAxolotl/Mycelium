using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UPProjectile : MonoBehaviour
{
    Rigidbody rb;
    public float damage;
    [SerializeField] private float range = 10;
    [SerializeField] private float AoERange = 2;
    [SerializeField] private float speed = 3;
    [SerializeField] private int particleSpacing = 36;
    [SerializeField] private float particleHeight = 0f;
    [SerializeField] private GameObject UPParticlePrefab;    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Boss")
        {
            speed = 0;
            DamageEnemies();
            UnstablePuffballParticles();
        }

        int enviornmentLayer = 8;
        if (collision.gameObject.layer == enviornmentLayer)
        {
            speed = 0;
            DamageEnemies();
            UnstablePuffballParticles();
        }
    }

    void DamageEnemies()
    {
        rb.velocity = Vector3.zero;

        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");
        int bossLayerMask = 1 << LayerMask.NameToLayer("Boss");

        Collider[] colliders = Physics.OverlapSphere(transform.position, AoERange, enemyLayerMask | bossLayerMask);
        
        // float damage = finalSkillValue;
        foreach (Collider collider in colliders)
        {
            NewEnemyHealth enemyHealth = collider.gameObject.GetComponent<NewEnemyHealth>();
            enemyHealth.EnemyTakeDamage(damage);
        }
        Destroy(gameObject);
    }

    void UnstablePuffballParticles()
    {
        int particlesPerCircle = 360 / particleSpacing;
        
        int currentSmalLSpacing = 0;
        for (int i = 0; i < particlesPerCircle; i++)
        {
            float smallX = Mathf.Cos(Mathf.Deg2Rad * currentSmalLSpacing) * AoERange;
            float smallZ = Mathf.Sin(Mathf.Deg2Rad * currentSmalLSpacing) * AoERange;

            InstantiateParticles(smallX, smallZ);
            currentSmalLSpacing += particleSpacing;
        }
    }

    void InstantiateParticles(float x, float z)
    {
        Vector3 circlePosition = new Vector3(x, particleHeight, z);
        Vector3 spawnPosition = transform.position + circlePosition;
        ParticleManager.Instance.SpawnParticles("EruptionParticles", spawnPosition, Quaternion.LookRotation(Vector3.up, Vector3.up));
    }
}
