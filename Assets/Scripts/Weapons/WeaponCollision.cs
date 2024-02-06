using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollision : MonoBehaviour
{
    PlayerAttack playerAttack;
    public float sentienceBonusDamage = 0f;
    List<GameObject> enemiesHit = new List<GameObject>();
    WeaponStats newWeaponStats;

    [SerializeField] private float secondsTilHitstopSpeedup = 0.25f;

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
            //HitStopManager.Instance.HitStop();
            StartCoroutine(HitStop());
        }
        if (this.gameObject.tag == "currentWeapon" && other.gameObject.tag == "Boss")
        {
            float dmgDealt = playerAttack.dmgDealt + sentienceBonusDamage;
            other.GetComponentInParent<BossHealth>().EnemyTakeDamage(dmgDealt);
            SoundEffectManager.Instance.PlaySound("Impact", other.gameObject.transform.position);
        }
    }

    IEnumerator HitStop()
    {
        Animator animator = GameObject.FindWithTag("currentPlayer").GetComponent<Animator>();

        animator.speed = 0f;

        //Speed Up
        float t = 0f;
        while (t < secondsTilHitstopSpeedup)
        {
            t += Time.unscaledDeltaTime;
            yield return null;
        }

        animator.speed = 1f;
    }

    public void ClearEnemyList()
    {
        enemiesHit.Clear();
    }
}
