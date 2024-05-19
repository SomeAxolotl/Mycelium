using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enigmatic : AttributeBase
{
    private bool hitSomething = false;

    public override void Initialize(){
        if(stats == null || hit == null){return;}
        attName = "Enigmatic";
        attDesc = "\n+50% Damage, changes on hit";
        stats.wpnName = attName + " " + stats.wpnName;
        interact.attributeDescription = attDesc;

        stats.wpnMult *= 1.5f;
    }

    public override void Hit(GameObject target, float damage){
        if(!hitSomething){
            hitSomething = true;
        }
    }

    public override void StopAttack(){
        if(hitSomething){
            playerParent = player.transform.parent.gameObject;
            swap = playerParent.GetComponent<SwapWeapon>();
            GameObject randomWeapon = Instantiate(Resources.Load(RandomWeapon()), GameObject.FindWithTag("WeaponSlot").transform) as GameObject;
            if(randomWeapon.GetComponent<WeaponStats>().wpnName != "Stick"){
                randomWeapon.transform.localScale = randomWeapon.transform.localScale / 0.03f / 100f / 1.2563f;
            }
            randomWeapon.GetComponent<WeaponStats>().acceptingAttribute = false;
            AttributeAssigner.Instance.PickAttFromString(randomWeapon, "Enigmatic");
            randomWeapon.layer = LayerMask.NameToLayer("currentWeapon");
            randomWeapon.GetComponent<Collider>().enabled = false;
            gameObject.tag = "Weapon";
            randomWeapon.tag = "currentWeapon";
            StartCoroutine(SetPreviousWeaponStats(randomWeapon));
        }
        hitSomething = false;
    }

    IEnumerator SetPreviousWeaponStats(GameObject randomWeapon){
        //Next frame do these things
        yield return 0;
        Destroy(gameObject);
        randomWeapon.GetComponent<WeaponInteraction>().ApplyWeaponPositionAndRotation();
        randomWeapon.GetComponent<Enigmatic>().Equipped();
    }

    public void OnDestroy(){
        if(attack != null){
            attack.StartedAttack -= StartAttack;
            attack.FinishedAttack -= StopAttack;
        }
    }

    private string RandomWeapon(){
        switch(Random.Range(0, 8)){
            case 0:
                return "Daybreak Arboretum/Slash/AvocadoFlamberge";
            case 1:
                return "Daybreak Arboretum/Slash/ObsidianScimitar";
            case 2:
                return "Daybreak Arboretum/Slash/MandibleSickle";
            case 3:
                return "Daybreak Arboretum/Smash/RoseMace";
            case 4:
                return "Daybreak Arboretum/Smash/GeodeHammer";
            case 5:
                return "Daybreak Arboretum/Smash/FemurClub";
            case 6:
                return "Daybreak Arboretum/Stab/BambooPartisan";
            case 7:
                return "Daybreak Arboretum/Stab/OpalRapier";
            case 8:
                return "Daybreak Arboretum/Stab/CarpalSais";
            default:
                return "Daybreak Arboretum/Slash/Stick";
        }
    }
}
