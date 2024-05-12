using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
    private EnemyHealth healthScript;
    private string particlePath = "Effects/PoisonParticles";
    [SerializeField] private GameObject poisonParticles;

    public float maxPoisonTime = 5;
    public float currPoisonTime = 5;
    //How many seconds pass for the poison to activate
    public float tickTime = 0.5f;
    private float currTickTime;
    public float poisonDamage = 0.5f;

    public void PoisonStats(float dmg = 0.5f, float tick = 0.5f, float maxTime = 5){
        poisonDamage = dmg;
        tickTime = tick;
        maxPoisonTime = maxTime;
    }

    void Awake(){
        healthScript = this.GetComponent<EnemyHealth>();
        currPoisonTime = maxPoisonTime;
        //Start the timer that deals damage every tick
        StartCoroutine(PoisonCoroutine());
        poisonParticles = Resources.Load<GameObject>(particlePath);
    }

    public void RefreshPoison(float dmg = 0.5f, float tick = 0.5f, float maxTime = 5){
        if(dmg > poisonDamage){poisonDamage = dmg;}
        if(tick > tickTime){tickTime = tick;}
        if(maxTime > maxPoisonTime){maxPoisonTime = maxTime;}
        currPoisonTime = maxPoisonTime;
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
        }else{
            Debug.Log("Particles not set for poison effect");
        }
    }
}
