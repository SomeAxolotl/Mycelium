using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairCurio : Curio
{
    [SerializeField] float minSitTime = 10f;
    [SerializeField] float maxSitTime = 20f;

    [SerializeField] float rotationOffset = 20f;

    public override IEnumerator DoEvent(WanderingSpore wanderingSpore)
    {
        wanderingSpore.lookTarget = Quaternion.Euler(0, rotationOffset, 0) * -(transform.position - wanderingSpore.transform.position);

        Animator animator = wanderingSpore.animator;

        yield return new WaitForSeconds(1f);

        animator.SetTrigger("SitFloor");
        animator.SetBool("InActionState", true);

        float randomSitTime = Random.Range(minSitTime, maxSitTime);
        yield return new WaitForSeconds(randomSitTime);

        animator.SetBool("InActionState", false);

        yield return new WaitForSeconds(2f);
    }
}
