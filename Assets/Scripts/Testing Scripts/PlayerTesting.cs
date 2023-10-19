using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTesting : MonoBehaviour
{
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
        LivingCycloneXXX,
        RelentlessFuryXXX,
        BlitzXXX,
        TrophicCascadeXXX,
        MycotoxinsXXX,
        SpineshotXXX,
        UnstablePuffballXXX,
        UndergrowthXXX,
        LeechingSporeXXX,
        SporeburstXXX,
        DefenseMechanismXXX
    }

    private enum SpeciesSkills
    {
        FungalMight,
        ZombifyXXX,
        DeathBlossomXXX,
        FairyRingXXX
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

    [Header("Prefabs")] 
    [SerializeField] private List<GameObject> skillPrefabs;

    private GameObject player;
    private GameObject skillLoadout;

    void Start()
    {
        player = GameObject.FindWithTag("currentPlayer");
        skillLoadout = player.transform.Find("SkillLoadout").gameObject;
    }
}
