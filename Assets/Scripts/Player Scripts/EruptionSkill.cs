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
    private InputAction statSkill1;
    private InputAction statSkill2;
    //private float dstToEnemy;
    bool fetchedStats = false;
    float sentienceSkillDmg;
    float eruptionDmg = 25f;
    public float finalEruptionDmg;
    float eruptionCooldown = 5f;
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

        if (skillLoadout.transform.GetChild(0).name.Contains("EruptionSkill"))
        {
            if (statSkill1.triggered && canSkill == true) 
            {
                player = GameObject.FindWithTag("currentPlayer");
                StartCoroutine("Eruption");
            }
        }

        /*if (skillLoadout.transform.GetChild(1).name == "EruptionSkill")
        {
            if (statSkill2.triggered && canSkill == true) 
            {
                player = GameObject.FindWithTag("currentPlayer");
                StartCoroutine("Eruption");
            }
        }*/
    }
    IEnumerator FetchStats()
    {
        yield return new WaitForEndOfFrame();

        //The info below gets the stats from the StatTracker script and makes a final damage/cooldown value depending on which skill the player is using.
        //sentienceSkillDmg = player.GetComponent<CharacterStats>().sentienceSkillDmg;
        finalEruptionDmg = sentienceSkillDmg + eruptionDmg;

        //skillCooldownBuff = player.GetComponent<CharacterStats>().skillCooldownBuff;
        finalSkillCooldown = eruptionCooldown - skillCooldownBuff;
        
        fetchedStats = true;
    }

    IEnumerator Eruption()
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

        GameObject tempHitbox = Instantiate(aoeHitbox, player.transform.position, player.transform.rotation) as GameObject;
        yield return new WaitForSeconds(activeHitboxTime); //Animation will go here
        Destroy(tempHitbox);
        //hudSkills.StartSkill1CooldownUI(finalSkillCooldown);
        yield return new WaitForSeconds(finalSkillCooldown - activeHitboxTime);
        canSkill = true;
    }
}
