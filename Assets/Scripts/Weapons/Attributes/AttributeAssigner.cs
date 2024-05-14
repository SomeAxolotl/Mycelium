using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AttributeAssigner : MonoBehaviour
{
    public static AttributeAssigner Instance;

    void Awake(){
        if(Instance == null){
            Instance = this;
        }else if(Instance != this){
            Destroy(gameObject);
        }
    }

    [Serializable]
    public class AttributeChance{
        public AttributeBase attribute;
        public float chance;
    }

    public AttributeChance[] attributes;

    public void AddRandomAttribute(GameObject weapon){
        float totalChance = 0;
        foreach(var att in attributes){
            totalChance += att.chance;
        }
        float randomValue = UnityEngine.Random.Range(0f, totalChance);
        foreach(var att in attributes){
            if(randomValue < att.chance){
                Component newComponent = weapon.AddComponent(att.attribute.GetType());
                AttributeBase newAttribute = newComponent as AttributeBase;
                newAttribute.statChange = att.attribute.statChange;
                break;
            }else{
                randomValue -= att.chance;
            }
        }
    }
}
