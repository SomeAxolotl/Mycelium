using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stinky : AttributeBase
{
    private GameObject stinkCloudPrefab;
    private GameObject stinkCloud;

    public override void Initialize(){
        attName = "Stinky";
        attDesc = "Damage overtime around spore";
        stinkCloudPrefab = Resources.Load("Attributes/StinkCloud", typeof(GameObject)) as GameObject;
    }

    public override void Equipped(){
        base.Equipped();
        stinkCloud = Instantiate(stinkCloudPrefab, player.transform);
    }

    public override void Unequipped(){
        base.Unequipped();
        Destroy(stinkCloud);
    }
}
