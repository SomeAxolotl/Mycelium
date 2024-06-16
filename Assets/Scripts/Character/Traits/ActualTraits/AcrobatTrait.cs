using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcrobatTrait : TraitBase
{
    public override void Start(){
        base.Start();

        traitName = "Acrobat";
        traitDesc = "\nJump when dashing";


        Actions.ActivatedDodge += Jump;
    }
    void OnDisable(){
        Actions.ActivatedDodge -= Jump;
    }

    public void Jump(){
        Debug.Log("Jump!");
        rb.velocity = new Vector3(0f, 12f, 0f);
    }
}
