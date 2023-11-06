using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingManager : MonoBehaviour
{
    [SerializeField] private bool testingEnabled = false;

    private enum WeaponTiers
    {
        Wood,
        Stone,
        Bone,
    }

    private enum WeaponTypes
    {
        Slash,
        Smash,
        Stab
    }

    private enum StatSkills
    {
        Eruption,
        LivingCyclone,
        RelentlessFury,
        Blitz,
        TrophicCascade,
        Mycotoxins,
        Spineshot,
        UnstablePuffball,
        Undergrowth,
        LeechingSpore,
        Sporeburst,
        DefenseMechanism
    }

    private enum SpeciesSkills
    {
        FungalMight,
        Zombify,
        DeathBlossom,
        FairyRing
    }

    [Header("Stats")]
    [SerializeField] private int primalLevel;
    [SerializeField] private int sentienceLevel;
    [SerializeField] private int speedLevel;
    [SerializeField] private int vitalityLevel;

    [Header("Weapon")]
    [SerializeField] private WeaponTiers weaponTier;
    [SerializeField] private WeaponTypes weaponType;

    [Header("Species Skill")] 
    [SerializeField] private SpeciesSkills speciesSkill;

    [Header("Stat Skills")] 
    [SerializeField] private StatSkills skill1;
    [SerializeField] private StatSkills skill2;

    private GameObject playerParent;
    private GameObject player;
    private CharacterStats playerStats;
    private SkillManager skillManager;

    void Start()
    {
        playerParent = GameObject.Find("PlayerParent");
        player = GameObject.FindWithTag("currentPlayer");
        playerStats = player.GetComponent<CharacterStats>();
        //weapon script
        skillManager = playerParent.GetComponent<SkillManager>();

        if (testingEnabled)
        {
            StartCoroutine(SetPlayerStuff());
        }
    }

    IEnumerator SetPlayerStuff()
    {
        //Set Stats
        playerStats.primalLevel = primalLevel;
        playerStats.sentienceLevel = sentienceLevel;
        playerStats.speedLevel = speedLevel;
        playerStats.vitalityLevel = vitalityLevel;

        //Set Weapon (WIP)

        //Set Skills
        yield return null;
        skillManager.SetSkill(speciesSkill.ToString(), 0, player);
        yield return null;
        skillManager.SetSkill(skill1.ToString(), 1, player);
        yield return null;
        skillManager.SetSkill(skill2.ToString(), 2, player);
    }
}
