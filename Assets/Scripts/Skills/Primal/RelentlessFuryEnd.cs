using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RelentlessFuryEnd : MonoBehaviour
{
    [HideInInspector] public ThirdPersonActionsAsset playerActionsAsset;
    private InputAction stat_skill_1;
    private InputAction stat_skill_2;
    public PlayerController playerController;
    public RelentlessFury relentlessFury;
    public PlayerHealth playerHealth;
    [SerializeField] private float skillDuration;

    // Start is called before the first frame update
    void Start()
    {
        playerActionsAsset = new ThirdPersonActionsAsset();
        playerActionsAsset.Player.Enable();
        stat_skill_1 = playerActionsAsset.Player.Stat_Skill_1;
        stat_skill_2 = playerActionsAsset.Player.Stat_Skill_2;
        playerController = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>();
        playerHealth = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (relentlessFury.isFrenzied == true)
        {
            StartCoroutine(DisableStuff());
            StartCoroutine(DisableAfter());
        }
    }

    IEnumerator DisableStuff()
    {
        yield return new WaitForSeconds(2);
        if (stat_skill_1.triggered || stat_skill_2.triggered)
        {
            if (GameObject.FindWithTag("RelentlessFury").activeSelf)
            {
                playerController.canUseDodge = true;
                relentlessFury.characterStats.moveSpeed = relentlessFury.originalSpeed;
                Destroy(GameObject.FindWithTag("RelentlessFuryParticles"));
                relentlessFury.isFrenzied = false;
                relentlessFury.durationTime = 0;
                relentlessFury.EndSkill();
            }
        }
    }

    IEnumerator DisableAfter()
    {
        yield return new WaitForSeconds(skillDuration);
        if (GameObject.FindWithTag("RelentlessFury").activeSelf)
        {
            playerController.canUseDodge = true;
            relentlessFury.characterStats.moveSpeed = relentlessFury.originalSpeed;
            Destroy(GameObject.FindWithTag("RelentlessFuryParticles"));
            relentlessFury.isFrenzied = false;
            relentlessFury.durationTime = 0;
        }
    }
}
