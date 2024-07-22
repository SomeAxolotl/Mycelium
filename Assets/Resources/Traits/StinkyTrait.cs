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
        traitDesc = "Nearby enemies take damage over time";

        stinkCloudPrefab = Resources.Load("Attributes/StinkCloud", typeof(GameObject)) as GameObject;
        stinkCloud = Instantiate(stinkCloudPrefab, player.transform);
        stinkCloud.GetComponent<StinkCloud>().stats = GetComponent<CharacterStats>();
    }

    public void OnDestroy(){
        Destroy(stinkCloud);
    }
}
