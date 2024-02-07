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

    // Start is called before the first frame update
    void Start()
    {
        playerActionsAsset = new ThirdPersonActionsAsset();
        playerActionsAsset.Player.Enable();
        stat_skill_1 = playerActionsAsset.Player.Stat_Skill_1;
        stat_skill_2 = playerActionsAsset.Player.Stat_Skill_2;
        playerController = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        Invoke("DisableStuff", 5);
    }

    void DisableStuff()
    {
        if (relentlessFury.isFrenzied == true && (stat_skill_1.triggered || stat_skill_2.triggered))
        {
            if (GameObject.FindWithTag("RelentlessFury").activeSelf)
            {
                playerController.canUseDodge = true;
                Destroy(GameObject.FindWithTag("RelentlessFuryParticles"));
                relentlessFury.isFrenzied = false;
                relentlessFury.durationTime = 0;
                relentlessFury.EndSkill();
            }
        }
    }
}
