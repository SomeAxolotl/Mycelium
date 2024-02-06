using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UndergrowthProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    public float destroyTime = 1.8f;
    [SerializeField] private GameObject undergrowthParticles;
    [SerializeField] private GameObject undergrowthCaughtParticles;
    Rigidbody rb;
    Undergrowth undergrowth;
    List<GameObject> hitEnemy = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, destroyTime);
        //StartCoroutine(Remove());
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
    // IEnumerator Remove()
    // {
    //     yield return new WaitForSeconds(destroyTime);
    //     gameObject.SetActive(false);
    // }
}
