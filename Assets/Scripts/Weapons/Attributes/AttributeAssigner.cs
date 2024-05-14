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
        public Component attribute;
        [Range(0, 1)]
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
                weapon.AddComponent(att.attribute.GetType());
                break;
            }else{
                randomValue -= att.chance;
            }
        }
    }
}
