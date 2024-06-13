using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fury : MonoBehaviour
{
    private float bonusDamagePer = 0.1f;
    private float currBonusNum = 0;

    [HideInInspector] public WeaponStats stats;
    [HideInInspector] public SwapWeapon swap;

    private void OnEnable(){
        swap = GetComponent<SwapWeapon>();
        if(swap != null){   
            stats = swap.O_curWeapon.GetComponent<WeaponStats>();
            swap.SwappedWeapon += NewWeapon;
            Actions.EnemyKilled += AddBonus;
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
        }
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
        if(currBonusNum > 0){
            if(timer != null){
                StopCoroutine(timer);
            }
            timer = null;
            currTimer *= 0.8f;
            timer = Timer();
            StartCoroutine(timer);
        }
        currBonusNum += 1;
        Debug.Log("Bonus Added: " + (currBonusNum * bonusDamagePer) + ", " + currTimer + "     Applied to:" + stats.gameObject.name);
    }

    private float currTimer = 10;
    private IEnumerator timer;
    private IEnumerator Timer(){
        yield return new WaitForSeconds(currTimer);
        stats.ClearAllStatsFrom(this);
        Destroy(this);
    }
}
