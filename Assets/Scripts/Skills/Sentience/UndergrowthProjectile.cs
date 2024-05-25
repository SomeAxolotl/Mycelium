using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UndergrowthProjectile : MonoBehaviour
{
    void Start(){
        Debug.Log("UndergrowthProjectile is not used anymore, script is on: " + gameObject.name);
    }
    /*
    private float speed = 4f;
    private float lifetime = 5f;
    [SerializeField] private float enemyFreezeTime;
    Rigidbody rb;
    Undergrowth undergrowth;
    [SerializeField] private GameObject undergrowthParticles;
    [SerializeField] private GameObject undergrowthCaughtParticles;
    List<GameObject> hitEnemy = new List<GameObject>();

    void Start(){
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifetime);
        undergrowth = GameObject.FindWithTag("currentPlayer").GetComponentInChildren<Undergrowth>();
    }

    void Update(){
        //transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == 6)
        {
            StartCoroutine(UnFreeze());
            // Invoke("UnFreeze", 5);
            hitEnemy.Add(collision.gameObject);
            foreach(GameObject enemy in hitEnemy)
            {
                if (enemy.GetComponent<BossHealth>() == null)
                {
                    enemy.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    enemy.GetComponent<Animator>().SetBool("IsMoving", false);
                }
                Instantiate(undergrowthCaughtParticles, enemy.transform.position, transform.rotation);
            }
        }
    }

    IEnumerator UnFreeze()
    {
        yield return new WaitForSeconds(enemyFreezeTime);
        foreach(GameObject enemy in hitEnemy)
        {
            if (enemy.GetComponent<BossHealth>() == null)
            {
                enemy.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                enemy.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                enemy.GetComponent<Animator>().SetBool("IsMoving", true);
            }
        }
        hitEnemy.Clear();
    }
    */
}
