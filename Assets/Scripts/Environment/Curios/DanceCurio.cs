using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceCurio : Curio
{
    bool doneDancing = false;

    public override IEnumerator Start()
    {
        currentUserCount = maxUserCount;

        StartCoroutine(base.Start());

        yield return null;
    }

    public override IEnumerator DoEvent(WanderingSpore wanderingSpore)
    {
        DrumCurio drumCurio = transform.parent.GetComponent<DrumCurio>();

        wanderingSpore.lookTarget = transform.parent.position - wanderingSpore.transform.position;

        bool isMetal = (Random.Range(0, 2) == 0) ? true : false;
        if (isMetal)
        {
            wanderingSpore.animator.SetTrigger("BustMetalMoves");
        }
        else
        {
            wanderingSpore.animator.SetTrigger("BustMoves");
        }

        wanderingSpore.animator.SetBool("InActionState", true);

        yield return new WaitUntil(() => doneDancing == true);

        doneDancing = false;

        wanderingSpore.animator.SetBool("InActionState", false);

        wanderingSpore.animator.SetTrigger("Wave");
    }

    public void OpenToDancing()
    {
        currentUserCount -= maxUserCount;
    }

    public void CloseToDancing()
    {
        doneDancing = true;
        currentUserCount += maxUserCount;
    }
}
