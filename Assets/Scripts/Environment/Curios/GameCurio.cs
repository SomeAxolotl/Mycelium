using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCurio : Curio
{
    [SerializeField] float minSitTime = 10f;
    [SerializeField] float maxSitTime = 20f;

    public override IEnumerator DoEvent(WanderingSpore wanderingSpore)
    {
        wanderingSpore.lookTarget = transform.position - wanderingSpore.transform.position;
        
        Animator animator = wanderingSpore.GetComponent<Animator>();

        animator.SetBool("Sitting", true);
        animator.SetTrigger("SitFloor");

        float randomSitTime = Random.Range(minSitTime, maxSitTime);
        yield return new WaitForSeconds(randomSitTime);

        animator.SetBool("Sitting", false);
    }
}
