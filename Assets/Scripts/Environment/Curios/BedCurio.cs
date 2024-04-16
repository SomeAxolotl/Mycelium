using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedCurio : Curio
{
    [SerializeField] float minLayTime = 10f;
    [SerializeField] float maxLayTime = 20f;

    public override IEnumerator DoEvent(WanderingSpore wanderingSpore)
    {
        bool isSleeping = (Random.Range(0, 2) == 0) ? true : false;

        if (isSleeping)
        {
            wanderingSpore.lookTarget = -(transform.position - wanderingSpore.transform.position);

            Animator animator = wanderingSpore.animator;

            yield return new WaitForSeconds(1f);

            animator.SetBool("InActionState", true);
            animator.SetTrigger("Sleep");

            float randomSitTime = Random.Range(minLayTime, maxLayTime);
            yield return new WaitForSeconds(randomSitTime);

            animator.SetBool("InActionState", false);

            yield return new WaitForSeconds(2f);
        }
        else
        {
            wanderingSpore.lookTarget = transform.position - wanderingSpore.transform.position;

            Animator animator = wanderingSpore.animator;

            yield return new WaitForSeconds(1f);

            animator.SetBool("InActionState", true);
            animator.SetTrigger("Lay");

            float randomSitTime = Random.Range(minLayTime, maxLayTime);
            yield return new WaitForSeconds(randomSitTime);

            animator.SetBool("InActionState", false);

            yield return new WaitForSeconds(2f);
        }
    }
}
