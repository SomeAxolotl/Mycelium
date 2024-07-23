using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboTrait : TraitBase
{
    private GameObject parent;
    private PlayerAttack attack;
    private WeaponStats savedWeapon;

    public override void Start(){
        base.Start();

        traitName = "Skilled";
        traitDesc = "Two hit combo attacks";
    }

    public override void SporeSelected(){
        parent = transform.parent.gameObject;
        attack = parent.GetComponent<PlayerAttack>();

        attack.StartedAttack += CheckCombo;
        attack.FinishedAttack += StartCombo;
    }
    public override void SporeUnselected(){
        attack.StartedAttack -= CheckCombo;
        attack.FinishedAttack -= StartCombo;
    }

    private IEnumerator combo;
    private IEnumerator ComboTimer(){
        //Debug.Log("Start combo");
        ComboOne();
        yield return new WaitForSeconds(1.2f);
        if(savedWeapon.weaponType == WeaponStats.WeaponTypes.Smash){
            yield return new WaitForSeconds(0.8f);
        }
        EndCombo();
    }
    //Animation times if I want to link the waitforseconds (NOT USED)
    //Slash = 0.666666
    //Smash = 1
    //Stab = 1

    private float savedAttackSpeed;
    private void StartCombo(){
        if(combo == null && !inCombo){
            combo = ComboTimer();
            StartCoroutine(combo);
        }
        if(combo != null && inCombo){
            EndCombo();
        }
    }

    private bool inCombo = false;
    private void CheckCombo(){
        if(savedWeapon != attack.curWeapon.GetComponent<WeaponStats>()){
            inCombo = false;
            EndCombo();
            savedWeapon = attack.curWeapon.GetComponent<WeaponStats>();
            return;
        }
        inCombo = (savedAttackSpeed != savedWeapon.wpnAttackSpeedModifier);
    }

    private void ComboOne(){
        if(savedWeapon != attack.curWeapon.GetComponent<WeaponStats>()){
            EndCombo();
            return;
        }
        savedAttackSpeed = savedWeapon.wpnAttackSpeedModifier;

        savedWeapon.wpnAttackSpeedModifier *= 1.5f;
    }

    private void EndCombo(){
        //Debug.Log("End combo");
        if(savedWeapon != null){
            savedWeapon.wpnAttackSpeedModifier = savedAttackSpeed;
        }
        if(combo != null){
            StopCoroutine(combo);
        }
        combo = null;
    }

    /*
    Dead Code

    public override void SporeSelected(){
        parent = transform.parent.gameObject;
        attack = parent.GetComponent<PlayerAttack>();

        attack.StartedAttack += CheckCombo;
        attack.FinishedAttack += StartCombo;
    }
    public override void SporeUnselected(){
        attack.StartedAttack -= CheckCombo;
        attack.FinishedAttack -= StartCombo;
    }

    private IEnumerator combo;
    private IEnumerator ComboTimer(){
        //Debug.Log("Start combo");
        ComboOne();
        yield return new WaitForSeconds(1.2f);
        if(savedWeapon.weaponType == WeaponStats.WeaponTypes.Smash){
            yield return new WaitForSeconds(0.8f);
        }
        EndCombo();
    }
    //Animation times if I want to link the waitforseconds (NOT USED)
    //Slash = 0.666666
    //Smash = 1
    //Stab = 1

    private WeaponStats.WeaponTypes savedType;
    private float savedAttackSpeed;
    private void StartCombo(){
        if(combo == null && !inCombo){
            combo = ComboTimer();
            StartCoroutine(combo);
        }
        if(combo != null && inCombo){
            EndCombo();
        }
    }

    private bool inCombo = false;
    private void CheckCombo(){
        if(savedWeapon != attack.curWeapon.GetComponent<WeaponStats>()){
            inCombo = false;
            EndCombo();
            savedWeapon = attack.curWeapon.GetComponent<WeaponStats>();
            return;
        }
        inCombo = (savedType != savedWeapon.weaponType);
    }

    private void ComboOne(){
        if(savedWeapon != attack.curWeapon.GetComponent<WeaponStats>()){
            EndCombo();
            return;
        }
        savedType = savedWeapon.weaponType;
        savedAttackSpeed = savedWeapon.wpnAttackSpeedModifier;

        if(savedWeapon.weaponType == WeaponStats.WeaponTypes.Slash){
            savedWeapon.weaponType = WeaponStats.WeaponTypes.Stab;
            savedWeapon.wpnAttackSpeedModifier *= 1.25f;
        }else if(savedWeapon.weaponType == WeaponStats.WeaponTypes.Smash){
            savedWeapon.weaponType = WeaponStats.WeaponTypes.Stab;
            savedWeapon.wpnAttackSpeedModifier *= 0.85f;
        }else if(savedWeapon.weaponType == WeaponStats.WeaponTypes.Stab){
            savedWeapon.weaponType = WeaponStats.WeaponTypes.Smash;
            savedWeapon.wpnAttackSpeedModifier *= 2.2f;
        }
    }

    private void EndCombo(){
        //Debug.Log("End combo");
        if(savedWeapon != null){
            savedWeapon.weaponType = savedType;
            savedWeapon.wpnAttackSpeedModifier = savedAttackSpeed;
        }
        if(combo != null){
            StopCoroutine(combo);
        }
        combo = null;
    }

    */
}
