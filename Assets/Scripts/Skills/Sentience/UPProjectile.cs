using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UPProjectile : MonoBehaviour
{
    [SerializeField] private float AOERange;
    [SerializeField] private float speed;
    [SerializeField] private LayerMask collidableLayers;
    Rigidbody rb;
    UnstablePuffball unstablePuffball;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        unstablePuffball = GameObject.FindWithTag("currentPlayer").GetComponentInChildren<UnstablePuffball>();

        //Launches the puffball with an initial force so it doesn't start falling right away
        Vector3 knockbackForce = transform.forward * 5f;
        knockbackForce += Vector3.up * 2f;
        rb.AddForce(knockbackForce, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "DangerZone") return; //Hard-Coded fix for ignoring boss DangerZone

        //Kind of scuffed but this ignores the big trigger colliders on the bouncy shrooms and spawners
        if (((1 << collision.gameObject.layer) & collidableLayers.value) != 0 
            && collision.gameObject.GetComponent<SmackableGlowShroomController>() == null 
            && collision.gameObject.GetComponent<SmackableShroomController>() == null
            && collision.gameObject.GetComponent<MushroomPlayerSensorController>() == null
            && collision.gameObject.GetComponent<EnemySpawner>() == null)
        {
            ParticleManager.Instance.SpawnParticles("PuffballParticles", transform.position, Quaternion.identity);
            SoundEffectManager.Instance.PlaySound("Explosion", transform.position, pitchMultiplier: 1.1f);
            DamageEnemies();
        }
    }

    void DamageEnemies()
    {
        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");

        Collider[] colliders = Physics.OverlapSphere(transform.position, AOERange, enemyLayerMask);
        
        foreach (Collider collider in colliders)
        {
            if (collider.GetComponent<EnemyHealth>() != null)
            {
                collider.GetComponent<EnemyHealth>().EnemyTakeDamage(unstablePuffball.finalSkillValue);
            }
        }
        Destroy(gameObject);
    }
}
