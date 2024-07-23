using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flourishing : AttributeBase
{
    private bool canRoot = true;

    public override void Initialize(){
        attName = "Flourishing";
        attDesc = "Every 12 seconds, root enemies hit for 4 seconds";
    }

    private bool isSwinging;
    public override void StartAttack(){
        isSwinging = true;
    }

    private bool hitSomething = false;
    public override void Hit(GameObject target, float damage){
        if(canRoot){
            RootTarget(target);
        }
    }

    public override void StopAttack(){
        isSwinging = false;
        //If you hit something, remove root
        if(hitSomething && canRoot){
            canRoot = false;
            if(cooldown == null){
                cooldown = RootCooldown();
                StartCoroutine(cooldown);
            }
        }
        hitSomething = false;
    }

    //For some reason I have to have a delay only when applied through melee attacks
    private void RootTarget(GameObject target){
        StartCoroutine(ApplyVulnerable(target));
    }

    private IEnumerator ApplyVulnerable(GameObject target){
        yield return new WaitForSeconds(0.5f);
        if(target.GetComponent<EnemyHealth>().currentHealth > 0){
            Root rootEffect = target.AddComponent<Root>();
            rootEffect.duration = 4;
        }
    }

    private IEnumerator cooldown;
    IEnumerator RootCooldown(){
        yield return new WaitForSeconds(12);
        if(isSwinging){
            yield return null;
        }
        canRoot = true;
        cooldown = null;
    }
}
