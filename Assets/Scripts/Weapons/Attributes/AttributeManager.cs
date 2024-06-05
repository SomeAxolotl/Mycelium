using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeManager : MonoBehaviour
{
    [HideInInspector] public WeaponInteraction interact;

    AttributeAssigner.Rarity highestRarity;
    AttributeBase[] attributes;

    private string allAttDesc;

    //Whenever this is references, it makes sure it is updated
    AttributeBase[] O_attributes{
        get{
            attributes = this.gameObject.GetComponents<AttributeBase>();
            return attributes;
        }}

    [SerializeField] private bool hasStats;
    [HideInInspector] public bool O_hasStats{
        get{
            hasStats = false;
            if(CheckPrimal()){hasStats = true;}
            if(CheckSentience()){hasStats = true;}
            if(CheckSpeed()){hasStats = true;}
            if(CheckVitality()){hasStats = true;}
            return hasStats;
        }}

    //All of these update dynamically
    [SerializeField] private float primalAmount;
    [HideInInspector] public float O_primalAmount{get{CheckPrimal(); return primalAmount;}}
    [SerializeField] private float sentienceAmount;
    [HideInInspector] public float O_sentienceAmount{get{CheckSentience(); return sentienceAmount;}}
    [SerializeField] private float speedAmount;
    [HideInInspector] public float O_speedAmount{get{CheckSpeed(); return speedAmount;}}
    [SerializeField] private float vitalityAmount;
    [HideInInspector] public float O_vitalityAmount{get{CheckVitality(); return vitalityAmount;}}

    private void Awake(){
        interact = GetComponent<WeaponInteraction>();
    }

    public void Start(){
        highestRarity = GetHighestAttributeRarity();
        UpdateStatDesc();
        UpdateUniqueDesc();
        interact.attributeDescription = allAttDesc;
    }

    private void UpdateStatDesc(){
        //Segments this section if there are any stats
        if(O_hasStats){allAttDesc += "\n";}
        //Because O_hasStats was called earlier, all amounts are updated
        if(primalAmount > 0){allAttDesc += " <sprite="+0+"> +" + primalAmount;}
        if(speedAmount > 0){allAttDesc += " <sprite="+1+"> +" + speedAmount;}
        if(sentienceAmount > 0){allAttDesc += " <sprite="+2+"> +" + sentienceAmount;}
        if(vitalityAmount > 0){allAttDesc += " <sprite="+3+"> +" + vitalityAmount;}
    }

    private List<string> addedDescs = new List<string>();
    private void UpdateUniqueDesc(){
        foreach(AttributeBase attribute in O_attributes){
            if(attribute.attDesc != "" && attribute.attDesc != " " && attribute.attDesc != null){
                Debug.Log("Add new att");
                allAttDesc += "\n" + attribute.attDesc;
            }
        }
    }

    public bool CheckPrimal(){foreach(AttributeBase currAtt in O_attributes){primalAmount += currAtt.primalAmount;} if(primalAmount > 0){return true;} return false;}
    public bool CheckSentience(){foreach(AttributeBase currAtt in O_attributes){sentienceAmount += currAtt.sentienceAmount;} if(sentienceAmount > 0){return true;} return false;}
    public bool CheckSpeed(){foreach(AttributeBase currAtt in O_attributes){speedAmount += currAtt.speedAmount;} if(speedAmount > 0){return true;} return false;}
    public bool CheckVitality(){foreach(AttributeBase currAtt in O_attributes){vitalityAmount += currAtt.vitalityAmount;} if(vitalityAmount > 0){return true;} return false;}
    

    public AttributeAssigner.Rarity GetHighestAttributeRarity(){
        int highestRarity = (int) AttributeAssigner.Rarity.None;
        foreach(AttributeBase attribute in O_attributes){
            int attributeRating = (int) attribute.rating;
            if(attributeRating > highestRarity){
                highestRarity = attributeRating;
            }
        }
        return (AttributeAssigner.Rarity) highestRarity;
    }
}
