using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BonusDamage : MonoBehaviour
{
    //Amount damage is increased by (0.1 = 10%)
    private float bonusDamage = 0.1f;
    //How long effect lasts
    public float currTimer = 8;

    private string furyParticlePath = "Effects/FuryParticles";
    [SerializeField] private ParticleSystem furyParticles;

    [HideInInspector] public WeaponStats stats;
    [HideInInspector] public SwapWeapon swap;
    public Action EffectEnd;
    public Action EffectRefresh;

    public void InitializeDamageChange(float duration = 5, float changeAmount = 0.1f){
        bonusDamage = changeAmount;
        currTimer = duration;
        swap = GetComponent<SwapWeapon>();
        if(swap != null){   
            stats = swap.O_curWeapon.GetComponent<WeaponStats>();
            swap.SwappedWeapon += NewWeapon;
            CreateParticles();
            AddBonus(null);
        }else{
            Debug.Log("Failed to apply Daamage buff on: " + this.gameObject.name);
            Destroy(this);
        }
    }
    private void OnDisable(){
        if(swap != null){   
            swap.SwappedWeapon -= NewWeapon;
            furyParticles.Stop();
        }
    }
    private void CreateParticles(){
        GameObject furyParticlesObj = Resources.Load<GameObject>(furyParticlePath);
        GameObject tempObj;
        tempObj = Instantiate(furyParticlesObj, transform.Find("Spore/SporeModel")) as GameObject;
        furyParticles = tempObj.GetComponent<ParticleSystem>();
        var newRate = furyParticles.emission;
        newRate.rateOverTime = 10f;
    }
    //Makes sure buff is removed from old weapons and given to new ones
    private void NewWeapon(GameObject oldWeapon, GameObject newWeapon){
        Debug.Log("Switched weapon while Damage buff is active");
        if(newWeapon != null){
            stats.ClearAllStatsFrom(this);
        }
        if(newWeapon != null){
            stats = newWeapon.GetComponent<WeaponStats>();
            stats.statNums.advDamage.AddModifier(new StatModifier(bonusDamage, StatModType.PercentAdd, this));
        }
    }

    private void AddBonus(EnemyHealth health){
        stats.statNums.advDamage.AddModifier(new StatModifier(bonusDamage, StatModType.PercentAdd, this));
        if(timer != null){StopCoroutine(timer);}
        timer = null;
        EffectRefresh?.Invoke();
        timer = Timer();
        StartCoroutine(timer);
        Debug.Log("Bonus Added: " + (bonusDamage) + ", " + currTimer + "     Applied to:" + stats.gameObject.name);
    }

    private IEnumerator timer;
    private IEnumerator Timer(){
        yield return new WaitForSeconds(currTimer);
        stats.ClearAllStatsFrom(this);
        furyParticles.Stop();
        EffectEnd?.Invoke();
        Destroy(this);
    }
}
