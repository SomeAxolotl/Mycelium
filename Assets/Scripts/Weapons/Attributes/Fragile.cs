using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragile : AttributeBase
{
    public bool hitSomething = false;
    private Material whiteMat;

    public override void Initialize(){
        if(stats == null || hit == null){return;}
        attName = "Fragile";
        attDesc = "\n+50% Damage, chance to break";
        stats.wpnName = attName + " " + stats.wpnName;
        interact.attributeDescription = attDesc;
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
            playerParent = player.transform.parent.gameObject;
            swap = playerParent.GetComponent<SwapWeapon>();
            GameObject startingWeapon = Instantiate(Resources.Load("Daybreak Arboretum/Slash/Stick"), GameObject.FindWithTag("WeaponSlot").transform) as GameObject;
            startingWeapon.layer = LayerMask.NameToLayer("currentWeapon");
            startingWeapon.GetComponent<Collider>().enabled = false;
            swap.curWeapon = startingWeapon;
            Destroy(gameObject);
            //Particles?
            //Play sound?
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
}
