using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineshotProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 4;
    public float damage;
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
            collision.GetComponent<NewEnemyHealth>().EnemyTakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
