using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusDamage : MonoBehaviour
{

    //ONLY WORKS ON PLAYER AND MELEE ENEMIES
    [HideInInspector] public WeaponCollision hit;
    [HideInInspector] public EnemyAttack enemyAttack;
    public float currBuffTime = 2;
    public float bonusDamage = 1;

    private void OnEnable(){
        hit = GetComponent<WeaponCollision>();
        if(hit != null){
            hit.HitEnemy += ApplyBonusDamage;
        }
        enemyAttack = GetComponent<EnemyAttack>();
        if(enemyAttack != null){
            enemyAttack.HitEnemy += ApplyBonusDamage;
        }
    }

    private void ApplyBonusDamage(GameObject target, float damage){
        if(hit != null){
            hit.dmgDealt += bonusDamage;
            hit.HitEnemy -= ApplyBonusDamage;
        }
        if(enemyAttack != null){
            enemyAttack.dmgDealt += bonusDamage;
            enemyAttack.HitEnemy -= ApplyBonusDamage;
        }
        Destroy(this);
    }

    public void StartSturdy(float duration = 2){
        StartCoroutine(SturdyCoroutine(duration));
    }

    private IEnumerator SturdyCoroutine(float duration){
        while(currBuffTime > 0){
            currBuffTime -= Time.deltaTime;
            yield return null;
        }
        Destroy(this);
    }
}
