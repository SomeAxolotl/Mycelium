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

    [Header("Stat Skills")] 
    [SerializeField] private StatSkills skill1;
    [SerializeField] private StatSkills skill2;

    [Header("Species Skill")] 
    [SerializeField] private SpeciesSkills skill3;

    private GameObject player;
    private CharacterStats playerStats;
    private SkillManager skillManager;

    void Start()
    {
        player = GameObject.FindWithTag("currentPlayer");
        playerStats = player.GetComponent<CharacterStats>();
        //weapon script
        skillManager = player.transform.Find("SkillLoadout").gameObject.GetComponent<SkillManager>();

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
        skillManager.SetSkill(skill1.ToString(), 0);
        yield return null;
        skillManager.SetSkill(skill2.ToString(), 1);
        yield return null;
        skillManager.SetSkill(skill3.ToString(), 2);
    }
}
