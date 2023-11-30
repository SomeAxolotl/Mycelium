using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempSpineshot : MonoBehaviour
{
    [SerializeField] private float speed = 4;
    [SerializeField] private float destroyTime = 5;
    Rigidbody rb;
    Spineshot spineshot;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, destroyTime);
        spineshot = GameObject.FindWithTag("currentPlayer").GetComponentInChildren<Spineshot>();
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
            if(collision.GetComponent<NewEnemyHealth>() != null)
            {
                collision.GetComponent<NewEnemyHealth>().EnemyTakeDamage(spineshot.finalSkillValue);
            }
            else if(collision.GetComponent<BossHealth>() != null)
            {
                collision.GetComponent<BossHealth>().EnemyTakeDamage(spineshot.finalSkillValue);
            }
            Destroy(gameObject);
        }
    }
}
