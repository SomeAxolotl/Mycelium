using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class EnemyAttack : MonoBehaviour
{
    public float dmgDealt;
    public Action<GameObject, float> HitEnemy;

    public virtual IEnumerator Attack()
    {
        yield return null;
    }
    public virtual void CancelAttack()
    {

    }
}
