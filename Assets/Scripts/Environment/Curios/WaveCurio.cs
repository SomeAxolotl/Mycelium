using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveCurio : Curio
{
    public override IEnumerator DoEvent(WanderingSpore wanderingSpore)
    {
        wanderingSpore.animator.SetTrigger("Wave");

        StartCoroutine(LookAtSpore(wanderingSpore));

        yield return new WaitUntil(() => !wanderingSpore.animator.GetCurrentAnimatorStateInfo(0).IsName("HelloAnim"));
    }

    IEnumerator LookAtSpore(WanderingSpore wanderingSpore)
    {     
        while (wanderingSpore.animator.GetCurrentAnimatorStateInfo(0).IsName("HelloAnim"))
        {
            wanderingSpore.lookTarget = transform.position - wanderingSpore.transform.position;

            yield return null;
        }
    }
}
