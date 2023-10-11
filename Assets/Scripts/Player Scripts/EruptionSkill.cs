using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EruptionSkill : MonoBehaviour
{
    [SerializeField] private GameObject aoeHitbox;
    //private Collider[] enemyColliders;
    //public LayerMask enemyLayer;
    //private Transform enemy;
    //private float aoeRadius = 4f;
    public bool canSkill = true;
    float activeHitboxTime = .1f;
    private ThirdPersonActionsAsset playerActionsAsset;
    private InputAction statSkill2;
    //private float dstToEnemy;
    bool fetchedStats = false;
    float sentienceSkillDmg;
    float eruptionDmg = 25f;
    public float finalEruptionDmg;
    float eruptionCooldown = 5f;
    float skillCooldownBuff;
    public float finalEruptionCooldown;

    // Start is called before the first frame update
    void Start()
    {
        playerActionsAsset = new ThirdPersonActionsAsset();
        playerActionsAsset.Player.Enable();
        statSkill2 = playerActionsAsset.Player.Stat_Skill_2;
    }

    // Update is called once per frame
    void Update()
    {
        if(fetchedStats == false)
        {
            StartCoroutine("FetchStats");
        }
        if (statSkill2.triggered && canSkill == true) 
        {
            StartCoroutine("Eruption");
        }
    }
    IEnumerator FetchStats()
    {
        yield return new WaitForEndOfFrame();

        //The info below gets the stats from the StatTracker script and makes a final damage/cooldown value depending on which skill the player is using.
        sentienceSkillDmg = gameObject.GetComponent<StatTracker>().sentienceSkillDmg;
        finalEruptionDmg = sentienceSkillDmg + eruptionDmg;

        skillCooldownBuff = gameObject.GetComponent<StatTracker>().skillCooldownBuff;
        finalEruptionCooldown = eruptionCooldown - skillCooldownBuff;
        
        fetchedStats = true;
    }

    IEnumerator Eruption()
    {
        canSkill = false;
        
        //Damage based on distance is not implemented yet

        /*enemyColliders = Physics.OverlapSphere(transform.position, aoeRadius, enemyLayer);
        foreach (var enemyCollider in enemyColliders)
        {
            enemy = enemyCollider.transform;
            dstToEnemy = Vector3.Distance(transform.position, enemy.position);
        }*/

        GameObject tempHitbox = Instantiate(aoeHitbox, transform.position, transform.rotation) as GameObject;
        yield return new WaitForSeconds(activeHitboxTime); //Animation will go here
        Destroy(tempHitbox);
        yield return new WaitForSeconds(finalEruptionCooldown - activeHitboxTime);
        canSkill = true;
    }
}
