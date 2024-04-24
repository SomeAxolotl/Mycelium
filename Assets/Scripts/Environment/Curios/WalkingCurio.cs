using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingCurio : Curio
{
    public override IEnumerator DoEvent(WanderingSpore wanderingSpore)
    {
        SoundEffectManager.Instance.PlaySound("Walking", transform.position);

        yield return null;
    }
}
