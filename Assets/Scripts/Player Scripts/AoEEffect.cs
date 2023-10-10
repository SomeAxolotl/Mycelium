using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class AoEEffect : MonoBehaviour
{
    public GameObject AoeAttack;
    public Transform Player;
    private Collider[] hitColliders;
    private Transform enemy;
    private bool canSkill = true;
    public AiController enemyController;
    // Start is called before the first frame update
    void Start()
    {
        enemyController = GameObject.FindGameObjectWithTag("Enemy").GetComponent<AiController>();
        



    }

    // Update is called once per frame
    void Update()
    {
        Vector3 origin = Player.position;
            float radius = 10.0f;
        if (Input.GetKeyDown("x") && canSkill) 
        {
            
            float delay = 3.0f;
            //GameObject newAoeAttack = Instantiate(AoeAttack);
            Instantiate(AoeAttack, Player.position, Player.rotation);
            //newAoeAttack.transform.SetParent(transform);    
            AoEDamage(origin, radius);
            //newAoeAttack.SetActive(true);
            StartCoroutine(OnCooldown(delay));
        }
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (GameObject.FindGameObjectWithTag("Enemy"))
        {
            enemyController.speedRun = 0;
            Debug.Log("Enemy Hit!");
        }
    }

    private void AoEDamage(Vector3 origin, float radius)
    {
        
        hitColliders = Physics.OverlapSphere(origin, radius);
        foreach (var hitCollider in hitColliders)
        {
            enemy = hitCollider.transform;
        }
    }

    private IEnumerator OnCooldown(float delay) 
    {
        canSkill = false;
        StartCoroutine(EnemyReturn(delay));
        yield return new WaitForSeconds(2f);
        GameObject.FindGameObjectWithTag("AOE").SetActive(false);
        
    yield return new WaitForSeconds(delay);
        canSkill = true;
    }

    private IEnumerator EnemyReturn(float delay) 
    {
        yield return new WaitForSeconds(3f);
        enemyController.speedRun = 5;
    }


}
