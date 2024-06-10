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
    [HideInInspector] public GameObject player;
    [HideInInspector] public WeaponCollision hit;
    [HideInInspector] public PlayerAttack attack;

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

        startSize = transform.localScale;
        AdjustSize();

        player = GameObject.FindWithTag("currentPlayer");
        attack = player.transform.parent.gameObject.GetComponent<PlayerAttack>();
        if(attack != null){
            attack.FinishedAttack += StopAttack;
        }

        //InvokeRepeating("SayStats", 1f, 1f);
    }

    private void Awake(){
        hit = GetComponent<WeaponCollision>();
        if(hit != null){
            hit.HitEnemy += Hit;
        }
        ScaleWeaponStats();
    }
    private void OnDisable(){
        if(hit != null){
            hit.HitEnemy -= Hit;
        }
        if(attack != null){
            attack.FinishedAttack -= StopAttack;
        }
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

    private Vector3 startSize;
    [HideInInspector] public Vector3 O_startSize{get{return startSize;}}
    public void AdjustSize(){
        transform.localScale = startSize;
        Vector3 newScale = new Vector3(transform.localScale.x * statNums.advSize.Value, transform.localScale.y * statNums.advSize.Value, transform.localScale.z * statNums.advSize.Value);
        transform.localScale = newScale;
    }

    //Every loop, add 50% of the weapons damage mult
    void ScaleWeaponStats(){
        //Debug.Log("Mult: " + mult + " + " + ((mult / 2) * (GlobalData.currentLoop - 1)));
        mult += (mult * 0.5f) * (GlobalData.currentLoop - 1);
    }

    //Clears stats when a level in unloaded so the data from components is not saved
    public void ClearAllStatsFrom(Component com){
        statNums.advDamage.RemoveModifierFromSource(com);
        statNums.advKnockback.RemoveModifierFromSource(com);
        statNums.advSize.RemoveModifierFromSource(com);
    }

    bool hitSomething = false;
    public void Hit(GameObject target, float damage){
        if(!hitSomething){
            hitSomething = true;
        }
    }
    public void StopAttack(){
        //If you hit something, do the thing
        if(hitSomething){
            RandomSkillRoll();
        }
        hitSomething = false;
    }

    public List<WeaponSkillProc> skillChances = new List<WeaponSkillProc>();
    public void RandomSkillRoll(){
        float totalChance = 0;
        foreach(WeaponSkillProc skill in skillChances){
            totalChance += skill.triggerChance;
        }
        //If there is under a 100% chance to activate, gives a chance to fail
        if(totalChance < 100){
            totalChance = 100;
        }
        float randomValue = UnityEngine.Random.Range(0f, totalChance);

        float currChance = 0;
        for(int i = 0; i < skillChances.Count; i++){
            currChance += skillChances[i].triggerChance;
            if(randomValue < currChance){
                //Activate skill
                skillChances[i].skillInstance.ActivateSkill(0, true);
                return;
            }
        }
    }
}

public class WeaponStatList{
    public AdvStat advDamage = new AdvStat();
    public AdvStat advKnockback = new AdvStat();
    public AdvStat advSize = new AdvStat(1);
}

public class WeaponSkillProc{
    public float triggerChance;
    public Skill skillInstance;
}
