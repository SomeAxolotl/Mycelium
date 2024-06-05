using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Massive : AttributeBase
{
    private float newSize = 1.5f;

    public override void Initialize(){
        attName = "Massive";
        attDesc = "50% larger";
        if(stats == null || hit == null){return;}
        stats.wpnName = attName + " " + stats.wpnName;

        Vector3 newScale = new Vector3(transform.localScale.x * newSize, transform.localScale.y * newSize, transform.localScale.z * newSize);
        transform.localScale = newScale;
    }
}
