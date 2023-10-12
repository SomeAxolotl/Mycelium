using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FungalMightSkill : MonoBehaviour
{
    public bool canSkill = true;
    private ThirdPersonActionsAsset playerActionsAsset;
    private InputAction statSkill1;
    private InputAction statSkill2;
    //private float dstToEnemy;
    bool fetchedStats = false;
    float sentienceSkillDmg;
    public float finalEruptionDmg;
    float fungalMightCooldown = 7f;
    float skillCooldownBuff;
    public float finalSkillCooldown;

    private GameObject skillLoadout;
    private GameObject player;

    private HUDSkills hudSkills;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("currentPlayer");
        playerActionsAsset = new ThirdPersonActionsAsset();
        playerActionsAsset.Player.Enable();
        statSkill1 = playerActionsAsset.Player.Stat_Skill_1;
        statSkill2 = playerActionsAsset.Player.Stat_Skill_2;

        skillLoadout = transform.parent.gameObject;

        hudSkills = GameObject.Find("HUD").GetComponent<HUDSkills>();
    }

    // Update is called once per frame
    void Update()
    {
        if(fetchedStats == false)
        {
            StartCoroutine("FetchStats");
        }

        if(fetchedStats == false)
        {
            StartCoroutine("FetchStats");
        }

        if (skillLoadout.transform.GetChild(0).name.Contains("FungalMightSkill"))
        {
            if (statSkill1.triggered && canSkill == true) 
            {
                player = GameObject.FindWithTag("currentPlayer");
                StartCoroutine("FungalMight");
            }
        }

        /*if (skillLoadout.transform.GetChild(1).name == "FungalMightSkill")
        {
            if (statSkill2.triggered && canSkill == true) 
            {
                player = GameObject.FindWithTag("currentPlayer");
                StartCoroutine("FungalMight");
            }
        }*/
    }


    IEnumerator FetchStats()
    {
        yield return new WaitForEndOfFrame();

        skillCooldownBuff = player.GetComponent<StatTracker>().skillCooldownBuff;
        finalSkillCooldown = fungalMightCooldown - skillCooldownBuff;
        
        fetchedStats = true;
    }

    IEnumerator FungalMight()
    {
        StartCoroutine("FetchStats");
        canSkill = false;
        
        //Damage based on distance is not implemented yet

        /*enemyColliders = Physics.OverlapSphere(transform.position, aoeRadius, enemyLayer);
        foreach (var enemyCollider in enemyColliders)
        {
            enemy = enemyCollider.transform;
            dstToEnemy = Vector3.Distance(transform.position, enemy.position);
        }*/

        player.GetComponent<MeleeAttack>().ActivateFungalMight();
        hudSkills.StartSkill1CooldownUI(finalSkillCooldown);
        yield return new WaitForSeconds(finalSkillCooldown);
        canSkill = true;
    }
}