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
<<<<<<< Updated upstream
        if (this.gameObject.tag == "currentWeapon" && other.gameObject.tag == "Enemy" && newPlayerAttack.attacking && !enemiesHit.Contains(other.gameObject))
=======
        if (this.gameObject.tag == "currentWeapon" && other.gameObject.tag == "Enemy" && !enemiesHit.Contains(other.gameObject))
>>>>>>> Stashed changes
        {
            enemiesHit.Add(other.gameObject);
            float dmgDealt = newPlayerAttack.dmgDealt;
            other.GetComponent<NewEnemyHealth>().EnemyTakeDamage(dmgDealt);
            HitStopManager.Instance.HitStop(dmgDealt);
<<<<<<< Updated upstream
            enemiesHit.Add(other.gameObject);
=======
>>>>>>> Stashed changes
        }
    }

    public void ClearEnemyList()
    {
        enemiesHit.Clear();
    }
}
