using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stinky : AttributeBase
{
    private GameObject stinkCloudPrefab;
    private GameObject stinkCloud;

    public override void Initialize(){
        if(stats == null || hit == null){return;}
        attName = "Stinky";
        attDesc = "\nDamage overtime around spore";
        stats.wpnName = attName + " " + stats.wpnName;
        interact.attributeDescription = attDesc;
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
