using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedCurio : Curio
{
    public override IEnumerator DoEvent(WanderingSpore wanderingSpore)
    {
        wanderingSpore.lookTarget = transform.position - wanderingSpore.transform.position;

        yield return new WaitForSeconds(1f);

        wanderingSpore.GetComponent<Animator>().SetBool("Lay(Beg)", true);

        yield return new WaitForSeconds(7f);

        wanderingSpore.GetComponent<Animator>().SetBool("Lay(Beg)", false);
        wanderingSpore.GetComponent<Animator>().SetBool("Lay(End)", true);


        yield return new WaitForSeconds(2f);
        wanderingSpore.GetComponent<Animator>().SetBool("Lay(End)", false);
    }
}
