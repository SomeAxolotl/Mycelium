using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempSpineshot : Spineshot
{
    [SerializeField] private float speed = 4;
    [SerializeField] private float destroyTime = 5;
    Rigidbody rb;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Boss")
        {
            NewEnemyHealth enemyHealth = collision.gameObject.GetComponent<NewEnemyHealth>();
            enemyHealth.EnemyTakeDamage(10);
            //collision.GetComponent<NewEnemyHealth>().EnemyTakeDamage(finalSkillValue);
            Destroy(gameObject);
        }
    }
}
