using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollision : MonoBehaviour
{
    PlayerAttack playerAttack;
    public float sentienceBonusDamage = 0f;
    List<GameObject> enemiesHit = new List<GameObject>();
    WeaponStats newWeaponStats;
    void Start()
    {
        playerAttack = GameObject.Find("PlayerParent").GetComponent<PlayerAttack>();
        newWeaponStats = GetComponent<WeaponStats>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.gameObject.tag == "currentWeapon" && other.gameObject.tag == "Enemy" && !enemiesHit.Contains(other.gameObject))
        {
            enemiesHit.Add(other.gameObject);
            float dmgDealt = playerAttack.dmgDealt + sentienceBonusDamage;
            other.GetComponent<EnemyHealth>().EnemyTakeDamage(dmgDealt);
            other.GetComponent<EnemyKnockback>().Knockback(newWeaponStats.wpnKnockback);
            SoundEffectManager.Instance.PlaySound("Impact", other.gameObject.transform.position);
            HitStopManager.Instance.HitStop(dmgDealt);
        }
        if (this.gameObject.tag == "currentWeapon" && other.gameObject.tag == "Boss")
        {
            float dmgDealt = playerAttack.dmgDealt + sentienceBonusDamage;
            other.GetComponentInParent<BossHealth>().EnemyTakeDamage(dmgDealt);
            SoundEffectManager.Instance.PlaySound("Impact", other.gameObject.transform.position);
        }
    }

    public void ClearEnemyList()
    {
        enemiesHit.Clear();
    }
}
