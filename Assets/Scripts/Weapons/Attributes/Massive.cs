using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Massive : AttributeBase
{
    private float newSize = 1.5f;

    public override void Initialize(){
        if(stats == null || hit == null){return;}
        attName = "Massive";
        attDesc = "\n50% larger";
        stats.wpnName = attName + " " + stats.wpnName;
        interact.attributeDescription = attDesc;

        Vector3 newScale = new Vector3(transform.localScale.x * newSize, transform.localScale.y * newSize, transform.localScale.z * newSize);
        transform.localScale = newScale;
    }
}
