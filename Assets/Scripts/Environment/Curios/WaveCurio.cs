using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveCurio : Curio
{
    float waveTime = 3f;
    float goadWaveBackTime = 0.75f;
    float maxRandomWaitTime = 0.5f;

    public override IEnumerator CurioEvent(WanderingSpore wanderingSpore)
    {
        currentUsers.Add(wanderingSpore);
        currentUserCount++;

        float randomWaitTime = Random.Range(0, maxRandomWaitTime);
        yield return new WaitForSeconds(randomWaitTime);

        wanderingSpore.GetComponent<Animator>().SetBool("Walk", false);

        if (wanderingSpore != null)
        {
            yield return wanderingSpore.StartCoroutine(DoEvent(wanderingSpore));
        }
    }

    public override IEnumerator DoEvent(WanderingSpore wanderingSpore)
    {
        wanderingSpore.animator.SetTrigger("Wave");

        StartCoroutine(LookAtSpore(wanderingSpore, gameObject));

        //if the target spore is a wandering spore (not the player), then goad a wave back
        WanderingSpore thisWanderingSpore = GetComponent<WanderingSpore>();
        if (thisWanderingSpore != null)
        {
            if (thisWanderingSpore.enabled == true)
            {
                StartCoroutine(GoadWaveBack(thisWanderingSpore));
            }
        }

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

    IEnumerator GoadWaveBack(WanderingSpore targetWanderingSpore)
    {
        yield return new WaitForSeconds(goadWaveBackTime);

        targetWanderingSpore.GoadedWaveBack(this);
    }
}
