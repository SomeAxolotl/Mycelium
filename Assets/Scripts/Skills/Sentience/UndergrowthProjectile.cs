using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndergrowthProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    public float destroyTime = 1.8f;
    [SerializeField] private GameObject undergrowthParticles;
    Rigidbody rb;
    Undergrowth undergrowth;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, destroyTime);
        Destroy(undergrowthParticles, destroyTime);
        undergrowth = GameObject.FindWithTag("currentPlayer").GetComponentInChildren<Undergrowth>();
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
            if (collision.GetComponent<EnemyHealth>() != null)
            {
                collision.GetComponent<EnemyHealth>().EnemyTakeDamage(undergrowth.finalSkillValue);
            }
            else if (collision.GetComponent<BossHealth>() != null)
            {
                collision.GetComponent<BossHealth>().EnemyTakeDamage(undergrowth.finalSkillValue);
            }
        }
    }
}
