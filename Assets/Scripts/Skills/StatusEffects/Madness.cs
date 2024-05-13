using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Madness : MonoBehaviour
{
    private GameObject target;
    private EnemyAttack enemyAttack;
    private ReworkedEnemyNavigation enemyNav;
    private ZombifiedMovement zombieMove;
    
    [SerializeField] private float currDuration;
    [HideInInspector] public float O_currDuration{
        get{return currDuration;}
        set{
            if(value == Mathf.Infinity){
                if(timer != null){
                    StopCoroutine(timer);
                    timer = null;
                }
            }
            currDuration = value;
        }
    }

    private IEnumerator timer;

    private bool accepting = false;
    Madness[] madnessInstances;
    private void Awake(){
        madnessInstances = GetComponents<Madness>();
        if(madnessInstances.Length == 1){
            accepting = true;
        }
        target = gameObject;
        enemyAttack = GetComponent<EnemyAttack>();
        if(enemyAttack != null){
            enemyAttack.CancelAttack();
            enemyAttack.enabled = false;
        }
        enemyNav = GetComponent<ReworkedEnemyNavigation>();
        if(enemyNav != null){
            enemyNav.enabled = false;
        }
        if(GetComponent<CrabAttack>() != null){
            GetComponent<CrabAttack>().StopAttack();
        }
        if(GetComponent<MushyAttack>() != null){
            GetComponent<MushyAttack>().StopAttack();
        }
        zombieMove = GetComponent<ZombifiedMovement>();
    }

    public void ApplyMadness(float damage = 5, float duration = Mathf.Infinity){
        if(madnessInstances.Length > 1 && accepting == false){
            //Combines madness
            foreach(Madness madness in madnessInstances){
                if(madness != this && madness.accepting == true){
                    madness.ApplyMadness(damage, duration);
                }
            }
            Destroy(this);
            return;
        }
        if(zombieMove != null){
            zombieMove.enabled = true;
            if(damage > zombieMove.explosionDamage){
                zombieMove.explosionDamage = damage;
            }
        }
        if(duration > currDuration){
            O_currDuration = duration;
            if(timer == null && duration != Mathf.Infinity){
                timer = MadnessTimer();
                StartCoroutine(timer);
            }
        }
    }

    private void ReturnSanity(){
        if(zombieMove != null){
            zombieMove.enabled = false;
        }
        if(enemyAttack != null){
            enemyAttack.enabled = true;
        }
        if(enemyNav != null){
            enemyNav.enabled = true;
        }
        Destroy(this);
    }

    IEnumerator MadnessTimer(){
        //Logic to spawn particles and change appearance goes here
        while(currDuration > 0){
            currDuration -= Time.deltaTime;
            yield return null;
        }
        ReturnSanity();
    }
}
