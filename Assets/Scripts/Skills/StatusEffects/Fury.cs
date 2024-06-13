using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fury : MonoBehaviour
{
    //Amount damage is increased by (0.1 = 10%)
    private float bonusDamagePer = 0.1f;
    //How many enemies have been killed (Starts at 1 though so they have something)
    private float currBonusNum = 0;
    //How many paticles should spawn per currBonusNumber
    private float particleRatePerNum = 5;
    //How long effect lasts
    private float currTimer = 8;
    //Amount the timer is decreased by each time (0.8 = 20% lost)
    private float decreaseMult = 0.8f;

    private string furyParticlePath = "Effects/FuryParticles";
    [SerializeField] private ParticleSystem furyParticles;

    [HideInInspector] public WeaponStats stats;
    [HideInInspector] public SwapWeapon swap;

    private void OnEnable(){
        swap = GetComponent<SwapWeapon>();
        if(swap != null){   
            stats = swap.O_curWeapon.GetComponent<WeaponStats>();
            swap.SwappedWeapon += NewWeapon;
            Actions.EnemyKilled += AddBonus;
            CreateParticles();
            AddBonus(null);
        }else{
            Debug.Log("Failed to apply Fury buff on: " + this.gameObject.name);
            Destroy(this);
        }
    }
    private void OnDisable(){
        if(swap != null){   
            swap.SwappedWeapon -= NewWeapon;
            Actions.EnemyKilled -= AddBonus;
            furyParticles.Stop();
        }
    }
    private void CreateParticles(){
        GameObject furyParticlesObj = Resources.Load<GameObject>(furyParticlePath);
        GameObject tempObj;
        tempObj = Instantiate(furyParticlesObj, transform.Find("Spore/SporeModel")) as GameObject;
        furyParticles = tempObj.GetComponent<ParticleSystem>();
    }
    //Makes sure buff is removed from old weapons and given to new ones
    private void NewWeapon(GameObject oldWeapon, GameObject newWeapon){
        Debug.Log("Switched weapon while fury is active");
        if(newWeapon != null){
            stats.ClearAllStatsFrom(this);
        }
        if(newWeapon != null){
            stats = newWeapon.GetComponent<WeaponStats>();
            stats.statNums.advDamage.AddModifier(new StatModifier(bonusDamagePer * currBonusNum, StatModType.PercentAdd, this));
        }
    }

    private void AddBonus(EnemyHealth health){
        stats.statNums.advDamage.AddModifier(new StatModifier(bonusDamagePer * currBonusNum, StatModType.PercentAdd, this));
        if(timer != null){StopCoroutine(timer);}
        timer = null;
        if(currBonusNum > 0){currTimer *= decreaseMult;}
        timer = Timer();
        StartCoroutine(timer);
        currBonusNum += 1;
        var newRate = furyParticles.emission;
        newRate.rateOverTime = currBonusNum * particleRatePerNum;
        Debug.Log("Bonus Added: " + (currBonusNum * bonusDamagePer) + ", " + currTimer + "     Applied to:" + stats.gameObject.name);
    }

    private IEnumerator timer;
    private IEnumerator Timer(){
        yield return new WaitForSeconds(currTimer);
        stats.ClearAllStatsFrom(this);
        furyParticles.Stop();
        Destroy(this);
    }
}
