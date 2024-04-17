using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    public virtual IEnumerator Attack()
    {
        yield return null;
    }
    public virtual void CancelAttack()
    {

    }
}
