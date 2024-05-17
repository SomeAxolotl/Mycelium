using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AttributeAssigner : MonoBehaviour
{
    public static AttributeAssigner Instance;

    public enum Rarity {Common, Rare, Legendary};
    public float commonChance;
    public float rareChance;
    public float legendaryChance;

    [SerializeField] private GameObject commons;
    [SerializeField] private GameObject rares;
    [SerializeField] private GameObject legendaries;

    private AttributeBase[] commonAtts;
    private AttributeBase[] rareAtts;
    private AttributeBase[] legendaryAtts;

    void Awake(){
        if(Instance == null){
            Instance = this;
        }else if(Instance != this){
            Destroy(gameObject);
        }
        commonAtts = commons.GetComponents<AttributeBase>();
        rareAtts = rares.GetComponents<AttributeBase>();
        legendaryAtts = legendaries.GetComponents<AttributeBase>();
    }

    public void AddRandomAttribute(GameObject weapon){
        float totalChance = commonChance + rareChance + legendaryChance;
        float randomValue = UnityEngine.Random.Range(0f, totalChance);

        if(randomValue <= commonChance){
            PickAttFromArray(weapon, commonAtts, Rarity.Common);
        }else if(randomValue - commonChance <= rareChance){
            PickAttFromArray(weapon, rareAtts, Rarity.Rare);
        }else{
            PickAttFromArray(weapon, legendaryAtts, Rarity.Legendary);
        }
    }

    private void PickAttFromArray(GameObject weapon, AttributeBase[] attArray, Rarity rating){
        AttributeBase randomAtt = attArray[UnityEngine.Random.Range(0, attArray.Length)];
        Component newComponent = weapon.AddComponent(randomAtt.GetType());
        AttributeBase newAttribute = newComponent as AttributeBase;
        newAttribute.statChange = randomAtt.statChange;
        newAttribute.rating = rating;
    }
}
