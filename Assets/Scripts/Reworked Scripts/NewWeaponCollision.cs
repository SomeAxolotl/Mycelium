using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewWeaponCollision : MonoBehaviour
{
    NewPlayerAttack newPlayerAttack;
    public float sentienceBonusDamage = 0f;
    List<GameObject> enemiesHit = new List<GameObject>();
    NewWeaponStats newWeaponStats;
    void Start()
    {
        newPlayerAttack = GameObject.Find("PlayerParent").GetComponent<NewPlayerAttack>();
        newWeaponStats = GetComponent<NewWeaponStats>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.gameObject.tag == "currentWeapon" && other.gameObject.tag == "Enemy" && !enemiesHit.Contains(other.gameObject))
        {
            enemiesHit.Add(other.gameObject);
            float dmgDealt = newPlayerAttack.dmgDealt + sentienceBonusDamage;
            other.GetComponent<NewEnemyHealth>().EnemyTakeDamage(dmgDealt);
            other.GetComponent<EnemyKnockback>().Knockback(newWeaponStats.knockback);
            HitStopManager.Instance.HitStop(dmgDealt);
            //Sound effect
            SoundEffectManager.Instance.PlaySound("Impact", transform.position);
        }
    }

    public void ClearEnemyList()
    {
        enemiesHit.Clear();
    }
}
