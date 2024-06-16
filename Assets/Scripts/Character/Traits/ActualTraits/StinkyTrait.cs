using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StinkyTrait : TraitBase
{
    private GameObject stinkCloudPrefab;
    private GameObject stinkCloud;

    public override void Start(){
        base.Start();

        traitName = "Stinky";
        traitDesc = "\nDamage overtime around spore";

        stinkCloudPrefab = Resources.Load("Attributes/StinkCloud", typeof(GameObject)) as GameObject;
        stinkCloud = Instantiate(stinkCloudPrefab, player.transform);
    }

    public void OnDestroy(){
        Destroy(stinkCloud);
    }
}
