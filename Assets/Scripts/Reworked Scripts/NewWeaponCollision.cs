using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewWeaponCollision : MonoBehaviour
{
    NewPlayerAttack newPlayerAttack;

    List<GameObject> enemiesHit = new List<GameObject>();

    void Start()
    {
        newPlayerAttack = GameObject.Find("PlayerParent").GetComponent<NewPlayerAttack>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.gameObject.tag == "currentWeapon" && other.gameObject.tag == "Enemy" && newPlayerAttack.attacking && !enemiesHit.Contains(other.gameObject))
        {
            float dmgDealt = newPlayerAttack.dmgDealt;
            other.GetComponent<NewEnemyHealth>().EnemyTakeDamage(dmgDealt);
            HitStopManager.Instance.HitStop(dmgDealt);
            enemiesHit.Add(other.gameObject);
        }
        if (this.gameObject.tag == "currentWeapon" && other.gameObject.tag == "Boss" && newPlayerAttack.attacking && !enemiesHit.Contains(other.gameObject))
        {
            float dmgDealt = newPlayerAttack.dmgDealt;
            other.GetComponent<BossHealth>().BossTakeDamage(dmgDealt);
            HitStopManager.Instance.HitStop(dmgDealt);
            enemiesHit.Add(other.gameObject);
        }
    }

    public void ClearEnemyList()
    {
        enemiesHit.Clear();
    }
}
