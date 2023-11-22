using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempSpineshot : MonoBehaviour
{
    [SerializeField] private float speed = 4;
    [SerializeField] private float destroyTime = 1;
    Rigidbody rb;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GameObject.FindWithTag("spineshotProjectile").GetComponent<Rigidbody>();
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            NewEnemyHealth enemyHealth = collision.gameObject.GetComponent<NewEnemyHealth>();
            enemyHealth.EnemyTakeDamage(10);
            Destroy(gameObject);
        }
    }
}
