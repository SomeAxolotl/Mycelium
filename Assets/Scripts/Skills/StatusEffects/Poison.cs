using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
    private EnemyHealth healthScript;
    //The path within the Resources folder to find the particle system
    private string particlePath = "Effects/PoisonParticles";
    [SerializeField] private GameObject poisonParticles;

    [SerializeField] private float maxPoisonTime = 5;
    [HideInInspector] public float O_maxPoisonTime{get{return maxPoisonTime;}}
    [SerializeField] private float currPoisonTime = 5;
    [HideInInspector] public float O_currPoisonTime{get{return currPoisonTime;}}
    //How many seconds pass for the poison to activate
    [SerializeField] private float tickTime = 0.5f;
    [HideInInspector] public float O_tickTime{get{return tickTime;}}
    [SerializeField] private float currTickTime;
    [HideInInspector] public float O_currTickTime{get{return currTickTime;}}
    [SerializeField] private float poisonDamage = 0.5f;
    [HideInInspector] public float O_poisonDamage{get{return poisonDamage;}}

    private IEnumerator timer;

    //Sets relevant information of the poison, refreshes its time to be its max
    //Will never go down in stats, only up when refreshing
    public void PoisonStats(float dmg = 0.5f, float tick = 0.5f, float maxTime = 5){
        if(dmg > poisonDamage){poisonDamage = dmg;}
        if(tick > tickTime){tickTime = tick;}
        if(maxTime > maxPoisonTime){maxPoisonTime = maxTime;}
        currPoisonTime = maxPoisonTime;
    }

    void Awake(){
        //Checks to see if the target has a particle saver, if it does not add it. 
        //Particle saver lets particles exist after target dies so they don't just dissapear instantly
        ParticleSaver saver = GetComponent<ParticleSaver>();
        if(saver == null){
            gameObject.AddComponent<ParticleSaver>();
        }
        //Checks to see if there are other instances of poison already on the target
        Poison[] poisonInstances = GetComponents<Poison>();
        if(poisonInstances.Length > 1){
            //Refreshes the poison already on the target
            foreach(Poison poisons in poisonInstances){
                poisons.PoisonStats(poisonDamage, tickTime, maxPoisonTime);
            }
            Destroy(this);
            return;
        }
        healthScript = this.GetComponent<EnemyHealth>();
        currPoisonTime = maxPoisonTime;
        //Start the timer that deals damage every tick
        timer = PoisonCoroutine();
        StartCoroutine(timer);
        poisonParticles = Resources.Load<GameObject>(particlePath);
    }

    private IEnumerator PoisonCoroutine(){
        while(currPoisonTime > 0){
            currPoisonTime -= Time.deltaTime;
            currTickTime += Time.deltaTime;
            //When the tick time is under 0, deal damage
            CheckPoison();
            yield return null;
        }
        //Removes this poison script
        Destroy(this);
    }

    private void CheckPoison(){
        //If the game lags while poison is applied, makes sure that extra damage is not applied
        if(currPoisonTime < 0){
            currTickTime += currPoisonTime;
        }
        if(currTickTime >= tickTime){
            DealPoisonDamage();
            currTickTime -= tickTime;
            //Checks that if the player did lag, the target takes their full poison damage
            if(currTickTime >= tickTime){
                CheckPoison();
            }
        }
    }

    private void DealPoisonDamage(){
        healthScript.EnemyTakeDamage(poisonDamage);
        //Tries to spawn poison particles
        if(poisonParticles != null){
            GameObject tempObj = Instantiate(poisonParticles) as GameObject;
            tempObj.transform.position = this.transform.position;
            tempObj.transform.parent = this.transform;
        }else{
            Debug.Log("Particles not set for poison effect");
        }
    }
}
