using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewWeaponCollision : MonoBehaviour
{
    NewPlayerAttack newPlayerAttack;
    public float sentienceBonusDamage = 0f;

    List<GameObject> enemiesHit = new List<GameObject>();

    void Start()
    {
        newPlayerAttack = GameObject.Find("PlayerParent").GetComponent<NewPlayerAttack>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.gameObject.tag == "currentWeapon" && other.gameObject.tag == "Enemy" && !enemiesHit.Contains(other.gameObject))
        {
            enemiesHit.Add(other.gameObject);
            float dmgDealt = newPlayerAttack.dmgDealt + sentienceBonusDamage;
            other.GetComponent<NewEnemyHealth>().EnemyTakeDamage(dmgDealt);
            SoundEffectManager.Instance.PlaySound("Impact", other.gameObject.transform.position);
            HitStopManager.Instance.HitStop(dmgDealt);
        }
    }

    public void ClearEnemyList()
    {
        enemiesHit.Clear();
    }
}
