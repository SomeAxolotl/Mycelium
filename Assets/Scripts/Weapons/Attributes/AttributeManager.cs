using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeManager : MonoBehaviour
{
    [HideInInspector] public WeaponInteraction interact;
    [HideInInspector] public WeaponStats stats;

    [SerializeField] private AttributeAssigner.Rarity highestRarity;
    [HideInInspector] public AttributeAssigner.Rarity O_highestRarity{
        get{
            return GetHighestAttributeRarity();
        }}
    private AttributeBase[] attributes;
    //Whenever this is references, it makes sure it is updated
    [HideInInspector] public AttributeBase[] O_attributes{
        get{
            attributes = this.gameObject.GetComponents<AttributeBase>();
            return attributes;
        }}

    private string allAttName;
    private string defaultName;
    private string allAttDesc;

    [SerializeField] private bool hasStats;
    //Updates all stat values when called
    [HideInInspector] public bool O_hasStats{
        get{
            hasStats = false;
            if(CheckPrimal()){hasStats = true;}
            if(CheckSentience()){hasStats = true;}
            if(CheckSpeed()){hasStats = true;}
            if(CheckVitality()){hasStats = true;}
            return hasStats;
        }}

    //All of these update dynamically as you check them
    [SerializeField] private float primalAmount;
    [HideInInspector] public float O_primalAmount{get{CheckPrimal(); return primalAmount;}}
    [SerializeField] private float sentienceAmount;
    [HideInInspector] public float O_sentienceAmount{get{CheckSentience(); return sentienceAmount;}}
    [SerializeField] private float speedAmount;
    [HideInInspector] public float O_speedAmount{get{CheckSpeed(); return speedAmount;}}
    [SerializeField] private float vitalityAmount;
    [HideInInspector] public float O_vitalityAmount{get{CheckVitality(); return vitalityAmount;}}

    [HideInInspector] public bool doesSpawnRarityParticles = false;

    private void Awake(){
        interact = GetComponent<WeaponInteraction>();
        stats = GetComponent<WeaponStats>();
        //Gets the name of the weapon before anything is added to it
        defaultName = stats.wpnName;
    }

    public void RefreshData(){
        highestRarity = GetHighestAttributeRarity();
        UpdateAttName();
        UpdateDesc();
    }

    private void UpdateAttName(){
        allAttName = defaultName;
        foreach(AttributeBase attribute in O_attributes){
            //Debug.Log(attribute.attName);
            if(attribute.attName != "" && attribute.attName != " " && attribute.attName != null){
                allAttName = attribute.attName + " " + allAttName;
            }
        }
        stats.wpnName = allAttName;
    }

    private void UpdateDesc(){
        //Resets current string
        allAttDesc = "";
        UpdateStatDesc();
        UpdateUniqueDesc();
        interact.attributeDescription = allAttDesc;
    }

    private void UpdateStatDesc(){
        //Needs this to update stats and put it on a different line
        if(O_hasStats){allAttDesc += "\n";}
        //Because O_hasStats was called earlier, all amounts are updated
        if(primalAmount > 0){allAttDesc += " <sprite="+0+"> +" + primalAmount;}
        if(speedAmount > 0){allAttDesc += " <sprite="+1+"> +" + speedAmount;}
        if(sentienceAmount > 0){allAttDesc += " <sprite="+2+"> +" + sentienceAmount;}
        if(vitalityAmount > 0){allAttDesc += " <sprite="+3+"> +" + vitalityAmount;}
    }

    private List<string> addedDescs = new List<string>();
    private void UpdateUniqueDesc(){
        for (int i = attributes.Length - 1; i >= 0; i--){
            if(attributes[i].attDesc != "" && attributes[i].attDesc != " " && attributes[i].attDesc != null && allAttDesc.Contains(attributes[i].attDesc) == false){
                Component[] allInstances = GetComponents(attributes[i].GetType());
                string extraText = "";
                if(allInstances.Length > 1){
                    extraText += "(" + allInstances.Length + "x) ";
                }
                allAttDesc += "\n" + extraText + attributes[i].attDesc;
            }
        }
    }

    public bool CheckPrimal(){foreach(AttributeBase currAtt in O_attributes){primalAmount += currAtt.primalAmount;} if(primalAmount > 0){return true;} return false;}
    public bool CheckSentience(){foreach(AttributeBase currAtt in O_attributes){sentienceAmount += currAtt.sentienceAmount;} if(sentienceAmount > 0){return true;} return false;}
    public bool CheckSpeed(){foreach(AttributeBase currAtt in O_attributes){speedAmount += currAtt.speedAmount;} if(speedAmount > 0){return true;} return false;}
    public bool CheckVitality(){foreach(AttributeBase currAtt in O_attributes){vitalityAmount += currAtt.vitalityAmount;} if(vitalityAmount > 0){return true;} return false;}

    void OnEnable(){
        StartCoroutine(RarityEffect());
    }

    IEnumerator RarityEffect(){
        yield return null;

        if (!doesSpawnRarityParticles) yield break;

        AttributeAssigner.Rarity highestRarity = GetHighestAttributeRarity();
        
        //if u take away my bracket spaces again griffin i will fucking obliterate u
        if(highestRarity == AttributeAssigner.Rarity.Rare)
        {
            ParticleSystem ps = ParticleManager.Instance.SpawnParticlesAndGetParticleSystem("LootParticles", this.transform.position, Quaternion.Euler(-90,0,0));
            SoundEffectManager.Instance.PlaySound("Rare Find", this.transform.position);
            ParticleSystem.MainModule main = ps.main;
            main.startColor = TooltipManager.Instance.rareBackgroundColor;
        }
        else if(highestRarity == AttributeAssigner.Rarity.Legendary)
        {
            ParticleSystem ps = ParticleManager.Instance.SpawnParticlesAndGetParticleSystem("LootParticles", this.transform.position, Quaternion.Euler(-90,0,0));
            SoundEffectManager.Instance.PlaySound("Legendary Find", this.transform.position);
            ParticleSystem.MainModule main = ps.main;
            main.startColor = TooltipManager.Instance.legendaryBackgroundColor;
        }
        else if (highestRarity == AttributeAssigner.Rarity.Forged)
        {
            ParticleSystem ps = ParticleManager.Instance.SpawnParticlesAndGetParticleSystem("LootParticles", this.transform.position, Quaternion.Euler(-90,0,0));
            SoundEffectManager.Instance.PlaySound("Legendary Find", this.transform.position);
            ParticleSystem.MainModule main = ps.main;
            main.startColor = TooltipManager.Instance.forgedBackgroundColor;
        }
    }

    public AttributeAssigner.Rarity GetHighestAttributeRarity()
    {
        int highestRarity = (int) AttributeAssigner.Rarity.None;
        foreach(AttributeBase attribute in O_attributes)
        {
            int attributeRating = (int) attribute.rating;
            if(attributeRating > highestRarity)
            {
                highestRarity = attributeRating;
            }
        }
        return (AttributeAssigner.Rarity) highestRarity;
    }
}
