using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UPProjectile : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] private float AOERange = 2;
    [SerializeField] private float speed = 12f;
    [SerializeField] private int particleSpacing = 36;
    [SerializeField] private float particleHeight = 0f;
    UnstablePuffball unstablePuffball;
    CamTracker camTracker;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        unstablePuffball = GameObject.FindWithTag("currentPlayer").GetComponentInChildren<UnstablePuffball>();
        camTracker = GameObject.FindWithTag("Camtracker").GetComponentInChildren<CamTracker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!camTracker.isLockedOn)
        {
            Vector3 launchDirection = (transform.up * 0.2f + transform.forward).normalized;
            transform.position += launchDirection * speed * Time.deltaTime;
        }
        else
        {
            Vector3 targetDir = camTracker.currentTarget.position - transform.position;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, speed * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);
            Vector3 launchDirection = (transform.up * 0.2f + transform.forward).normalized;
            transform.position += launchDirection * speed * Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Boss")
        {
            ParticleManager.Instance.SpawnParticles("PuffballParticles", transform.position, Quaternion.identity);
            DamageEnemies();
            //UnstablePuffballParticles();
        }

        int enviornmentLayer = 8;
        int wallLayer = 12;
        if (collision.gameObject.layer == enviornmentLayer || collision.gameObject.layer == wallLayer)
        {
            ParticleManager.Instance.SpawnParticles("PuffballParticles", transform.position, Quaternion.identity);
            DamageEnemies();
            //UnstablePuffballParticles();
        }
    }

    void DamageEnemies()
    {
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

    /*void UnstablePuffballParticles()
    {
        int particlesPerCircle = 360 / particleSpacing;
        
        int currentSmallSpacing = 0;
        for (int i = 0; i < particlesPerCircle; i++)
        {
            float smallX = Mathf.Cos(Mathf.Deg2Rad * currentSmallSpacing) * AOERange;
            float smallZ = Mathf.Sin(Mathf.Deg2Rad * currentSmallSpacing) * AOERange;

            InstantiateParticles(smallX, smallZ);
            currentSmallSpacing += particleSpacing;
        }
    }

    void InstantiateParticles(float x, float z)
    {
        Vector3 circlePosition = new Vector3(x, particleHeight, z);
        Vector3 spawnPosition = transform.position + circlePosition;
        ParticleManager.Instance.SpawnParticles("EruptionParticles", spawnPosition, Quaternion.LookRotation(Vector3.up, Vector3.up));
    }*/
}
