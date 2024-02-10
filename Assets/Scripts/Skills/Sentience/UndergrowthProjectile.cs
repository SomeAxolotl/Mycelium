using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UndergrowthProjectile : MonoBehaviour
{
    private float speed = 12f;
    private float lifetime = 5f;
    Rigidbody rb;
    Undergrowth undergrowth;
    [SerializeField] private GameObject undergrowthParticles;
    [SerializeField] private GameObject undergrowthCaughtParticles;
    List<GameObject> hitEnemy = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifetime);
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
            Invoke("UnFreeze", 15);
            hitEnemy.Add(collision.gameObject);
            foreach(GameObject enemy in hitEnemy)
            {
                enemy.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                enemy.GetComponent<Animator>().SetBool("IsMoving", false);
                enemy.GetComponent<NavMeshAgent>().enabled = false;
                enemy.GetComponent<EnemyNavigation>().enabled = false;
                Instantiate(undergrowthCaughtParticles, enemy.transform.position, transform.rotation);
                enemy.GetComponent<MeleeEnemyAttack>().enabled = false;
                enemy.GetComponent<RangedEnemyShoot>().enabled = false;
                enemy.GetComponent<EnemyHealth>().EnemyTakeDamage(undergrowth.finalSkillValue);
                enemy.GetComponent<BossHealth>().EnemyTakeDamage(undergrowth.finalSkillValue);
            }
        }
    }

    void UnFreeze()
    {
        foreach(GameObject enemy in hitEnemy)
        {
            enemy.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            enemy.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            enemy.GetComponent<Animator>().SetBool("IsMoving", true);
            enemy.GetComponent<NavMeshAgent>().enabled = true;
            enemy.GetComponent<EnemyNavigation>().enabled = true;
            enemy.GetComponent<MeleeEnemyAttack>().enabled = true;
            enemy.GetComponent<RangedEnemyShoot>().enabled = true;
        }
        hitEnemy.Clear();
    }
}
