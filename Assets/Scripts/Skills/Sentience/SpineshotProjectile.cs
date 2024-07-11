using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineshotProjectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject ExplosionVFX;
    [SerializeField] private LayerMask collidableLayers;
    Rigidbody rb;
    Spineshot spineshot;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spineshot = GameObject.FindWithTag("currentPlayer").GetComponentInChildren<Spineshot>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "DangerZone") return; //Hard-Coded fix for ignoring boss DangerZone

        int enemyLayer = LayerMask.NameToLayer("Enemy");
        if (collision.gameObject.layer == enemyLayer)
        {
            if (collision.GetComponent<EnemyHealth>() != null)
            {
                collision.GetComponent<EnemyHealth>().EnemyTakeDamage(spineshot.finalSkillValue);
            }
            Instantiate(ExplosionVFX, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else if (((1 << collision.gameObject.layer) & collidableLayers.value) != 0
            && collision.gameObject.GetComponent<SmackableGlowShroomController>() == null
            && collision.gameObject.GetComponent<SmackableShroomController>() == null
            && collision.gameObject.GetComponent<MushroomPlayerSensorController>() == null)
            //Kind of scuffed but this ignores the big trigger colliders on the bouncy shrooms
        {
            Instantiate(ExplosionVFX, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
