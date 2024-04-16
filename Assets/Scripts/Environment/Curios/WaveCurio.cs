using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveCurio : Curio
{
    float waveTime = 3f;
    float goadWaveBackTime = 0.75f;

    public override IEnumerator DoEvent(WanderingSpore wanderingSpore)
    {
        wanderingSpore.animator.SetTrigger("Wave");

        GameObject randomSpore = wanderingSpore.GetRandomNearbySpore();

        StartCoroutine(LookAtSpore(wanderingSpore, randomSpore));

        yield return new WaitForSeconds(waveTime);
    }

    IEnumerator LookAtSpore(WanderingSpore wanderingSpore, GameObject randomSpore)
    {     
        float timer = 0f;

        while(timer < waveTime)
        {
            wanderingSpore.lookTarget = randomSpore.transform.position - wanderingSpore.transform.position;

            timer += Time.deltaTime;

            yield return null;
        }
    }

    /*IEnumerator GoadWaveBack()
    {
        yield return new WaitForSeconds(goadWaveBackTime);

        WanderingSpore goadedWanderingSpore = GetComponent<WanderingSpore>();

        if (goadedWanderingSpore.gameObject.tag != "currentPlayer" && (int) goadedWanderingSpore.currentState != 5)
        {
            goadedWanderingSpore.StopAllCoroutines();

            goadedWanderingSpore.currentState = (WanderingSpore.WanderingStates) 5;

            StartCoroutine(goadedWanderingSpore.InteractWithCurio(this));
        }
    }*/
}
