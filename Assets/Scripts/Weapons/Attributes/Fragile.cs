using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragile : AttributeBase
{
    public bool hitSomething = false;
    private Material whiteMat;

    public override void Initialize(){
        attName = "Fragile";
        attDesc = "+50% Damage, chance to break";
        if(stats == null || hit == null){return;}
        stats.wpnName = attName + " " + stats.wpnName;
        specialAttNum = Random.Range(30, 40);

        stats.wpnMult *= 1.5f;
        whiteMat = Instantiate(Resources.Load("m_white")) as Material;
    }

    public override void Hit(GameObject target, float damage){
        if(!hitSomething){
            specialAttNum -= 1;
            hitSomething = true;
            if(specialAttNum <= 0){
                TurnWhite();
            }
        }
    }

    public override void StopAttack(){
        hitSomething = false;
        if(specialAttNum <= 0){
            //Break weapon
            NewWeapon();
        }
    }
    public void OnDestroy(){
        if(attack != null){
            attack.StartedAttack -= StartAttack;
            attack.FinishedAttack -= StopAttack;
        }
    }

    private Renderer wepRen;
    private void TurnWhite(){
        wepRen = GetComponent<Renderer>();
        foreach(Material mat in wepRen.materials){
            Debug.Log("Change color white: " + whiteMat);
            //mat.Lerp(mat, whiteMat, 0);
            wepRen.materials = new Material[]{whiteMat};
        }
    }

    public void NewWeapon(){
        playerParent = player.transform.parent.gameObject;
        swap = playerParent.GetComponent<SwapWeapon>();
        //Weapon you want to give goes in the first part
        GameObject randomWeapon = Instantiate(Resources.Load("Slash/Stick"), GameObject.FindWithTag("WeaponSlot").transform) as GameObject;
        if(randomWeapon.GetComponent<WeaponStats>().wpnName != "Stick"){
            randomWeapon.transform.localScale = randomWeapon.transform.localScale / 0.03f / 100f / 1.2563f;
        }
        randomWeapon.GetComponent<WeaponStats>().acceptingAttribute = false;
        randomWeapon.layer = LayerMask.NameToLayer("currentWeapon");
        randomWeapon.GetComponent<Collider>().enabled = false;
        gameObject.tag = "Weapon";
        randomWeapon.tag = "currentWeapon";
        StartCoroutine(SetPreviousWeaponStats(randomWeapon));
    }

    //Has to wait a frame for things to initialize
    IEnumerator SetPreviousWeaponStats(GameObject randomWeapon){
        //Next frame do these things
        yield return 0;
        Destroy(gameObject);
        randomWeapon.GetComponent<WeaponInteraction>().ApplyWeaponPositionAndRotation();
    }
}
