using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineshotProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 12;
    [SerializeField] private float destroyTime = 5;
    Rigidbody rb;
    Spineshot spineshot;
    CamTracker camTracker;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, destroyTime);
        spineshot = GameObject.FindWithTag("currentPlayer").GetComponentInChildren<Spineshot>();
        camTracker = GameObject.FindWithTag("Camtracker").GetComponentInChildren<CamTracker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!camTracker.isLockedOn)
            transform.position += transform.forward * speed * Time.deltaTime;
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, camTracker.currentTarget.position, speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Boss")
        {
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            enemyHealth.EnemyTakeDamage(10);
            //collision.GetComponent<EnemyHealth>().EnemyTakeDamage(finalSkillValue);
            if (collision.GetComponent<EnemyHealth>() != null)
            {
                collision.GetComponent<EnemyHealth>().EnemyTakeDamage(spineshot.finalSkillValue);
            }
            else if (collision.GetComponent<BossHealth>() != null)
            {
                collision.GetComponent<BossHealth>().EnemyTakeDamage(spineshot.finalSkillValue);
            }
            Destroy(gameObject);
        }
    }
}
