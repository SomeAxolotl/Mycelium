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

    public WeaponStatList statNums = new WeaponStatList();

    //Stats of the weapon, only used to set values
    [HideInInspector][SerializeField] private float baseDmg = 10;
    [SerializeField] private float mult = 1;
    [SerializeField] private float knockback;

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
        //Adds the base damage of the weapon
        statNums.advDamage.AddModifier(new StatModifier(baseDmg, StatModType.Flat, this));
        //Adds the base multiplier of the weapon
        statNums.advDamage.AddModifier(new StatModifier(mult, StatModType.PercentMult, this));

        statNums.advKnockback.AddModifier(new StatModifier(knockback, StatModType.Flat, this));

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
            Debug.Log("Name: " + wpnName + "\tDamage: " + statNums.advDamage.Value + "\tKnockback: " + statNums.advKnockback.Value, gameObject);
        }
    }

    //Every loop, add 50% of the weapons damage mult
    void ScaleWeaponStats(){
        //Debug.Log("Mult: " + mult + " + " + ((mult / 2) * (GlobalData.currentLoop - 1)));
        mult += (mult * 0.5f) * (GlobalData.currentLoop - 1);
    }
}

public class WeaponStatList
{
    public AdvStat advDamage = new AdvStat();
    public AdvStat advKnockback = new AdvStat();
}
