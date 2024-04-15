using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedCurio : Curio
{
    [SerializeField] float minLayTime = 10f;
    [SerializeField] float maxLayTime = 20f;

    public override IEnumerator DoEvent(WanderingSpore wanderingSpore)
    {
        wanderingSpore.lookTarget = transform.position - wanderingSpore.transform.position;

        Animator animator = wanderingSpore.animator;

        yield return new WaitForSeconds(1f);

        animator.SetBool("Lay(Beg)", true);

        float randomSitTime = Random.Range(minLayTime, maxLayTime);
        yield return new WaitForSeconds(randomSitTime);

        animator.SetBool("Lay(Beg)", false);
        animator.SetBool("Lay(End)", true);


        yield return new WaitForSeconds(2f);
        animator.SetBool("Lay(End)", false);
    }
}
