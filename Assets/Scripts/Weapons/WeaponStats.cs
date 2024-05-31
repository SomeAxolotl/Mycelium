using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    public enum WeaponTypes
    {
        Slash,
        Smash,
        Stab
    }

    public WeaponTypes weaponType;

    [SerializeField] public string wpnName = "Wooden Sword";

    [HideInInspector][SerializeField] private float baseDmg = 10;
    [HideInInspector] public float wpnBaseDmg{get{return baseDmg;}set{baseDmg = value;}}
    [SerializeField] private float mult = 1;
    [HideInInspector] public float wpnMult{get{return mult;}set{mult = value;}}
    [SerializeField] private float knockback;
    [HideInInspector] public float wpnKnockback{get{return knockback;}set{knockback = value;}}

    public float wpnAttackSpeedModifier = 1.0f;

    float rotationSpeed = 30f;
    float tiltAngle = 45f;

    private Quaternion initialRotation;

    public Vector3 holdPositionOffset = new Vector3();
    public Vector3 holdRotationOffset = new Vector3();

    [Tooltip("Controls what percent of the attack animation the weapon collider enables at")] public float percentUntilWindupDone = 0.3f;
    [Tooltip("Controls what percent of the attack animation the weapon collider disables at")] public float percentUntilSwingDone = 0.55f;

    public float secondsTilHitstopSpeedup = 0.25f;

    public bool acceptingAttribute = true;

    void Start()
    {
        if(acceptingAttribute){
            AttributeAssigner.Instance.AddRandomAttribute(gameObject);
        }

        if (gameObject.tag == "currentWeapon")
        {
            Vector3 positionOffset = holdPositionOffset;
            transform.localPosition = positionOffset;
            Vector3 rotationOffset = holdRotationOffset;
            transform.parent.transform.localEulerAngles = rotationOffset;
        }

        //InvokeRepeating("SayStats", 1f, 1f);
    }

    private void Awake(){
        ScaleWeaponStats();
    }

    void OnEnable()
    {
        StartCoroutine(RarityEffect());
    }
    IEnumerator RarityEffect()
    {
        yield return null;

        AttributeAssigner.Rarity highestRarity = GetHighestAttributeRarity();
        
        if (highestRarity == AttributeAssigner.Rarity.Rare)
        {
            ParticleSystem ps = ParticleManager.Instance.SpawnParticlesAndGetParticleSystem("LootParticles", this.transform.position, Quaternion.Euler(-90,0,0));
            ParticleSystem.MainModule main = ps.main;
            main.startColor = TooltipManager.Instance.rareBackgroundColor;
        }
        else if (highestRarity == AttributeAssigner.Rarity.Legendary)
        {
            ParticleSystem ps = ParticleManager.Instance.SpawnParticlesAndGetParticleSystem("LootParticles", this.transform.position, Quaternion.Euler(-90,0,0));
            ParticleSystem.MainModule main = ps.main;
            main.startColor = TooltipManager.Instance.legendaryBackgroundColor;
        }
    }

    public AttributeAssigner.Rarity GetHighestAttributeRarity()
    {
        AttributeBase[] attributes = this.gameObject.GetComponents<AttributeBase>();
        int highestRarity = (int) AttributeAssigner.Rarity.None;
        foreach (AttributeBase attribute in attributes)
        {
            int attributeRating = (int) attribute.rating;
            if (attributeRating > highestRarity)
            {
                highestRarity = attributeRating;
            }
        }

        return (AttributeAssigner.Rarity) highestRarity;
    }

    void Update()
    {
        if (gameObject.tag == "Weapon")
        {
            transform.parent = null;

            Quaternion targetRotation = Quaternion.Euler(0f, rotationSpeed * Time.deltaTime, 0f) * transform.rotation;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, tiltAngle);
        }
    }

    void SayStats()
    {
        if(wpnName != "Stick")
        {
            Debug.Log("Name: " + wpnName + "\tDamage: " + wpnMult + "\tKnockback: " + wpnKnockback, gameObject);
        }
    }

    //Every loop, add 50% of the weapons damage mult
    void ScaleWeaponStats(){
        //Debug.Log("Mult: " + mult + " + " + ((mult / 2) * (GlobalData.currentLoop - 1)));
        mult += (mult * 0.5f) * (GlobalData.currentLoop - 1);
    }
}
