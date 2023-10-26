using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewWeaponCollision : MonoBehaviour
{
    NewPlayerAttack newPlayerAttack;
    // Start is called before the first frame update
    void Start()
    {
        newPlayerAttack = GameObject.Find("PlayerParent").GetComponent<NewPlayerAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (this.gameObject.tag == "currentWeapon" && other.gameObject.tag == "Enemy" && newPlayerAttack.attacking)
        {
            float dmgDealt = newPlayerAttack.dmgDealt;
            other.GetComponent<NewEnemyHealth>().EnemyTakeDamage(dmgDealt);
        }
    }
}
