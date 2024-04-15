using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveCurio : Curio
{
    public override IEnumerator DoEvent(WanderingSpore wanderingSpore)
    {
        wanderingSpore.lookTarget = transform.position - wanderingSpore.transform.position;

        Animator animator = wanderingSpore.animator;
        animator.SetTrigger("Wave");

        yield return new WaitForSeconds(2f);
    }
}
