using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[Serializable]
public class AdvStat
{
    protected List<StatModifier> statModifiers;
    public ReadOnlyCollection<StatModifier> StatModifiers;

    public float BaseValue = 0;
    protected float lastBaseValue = float.MinValue;
    protected float lastCombinedValue;
    protected bool isDirty = true;
    protected float recentValue;
    protected float recentCombinedValue;
    public virtual float Value{
        get{
            if(isDirty || BaseValue != lastBaseValue){
                lastBaseValue = BaseValue;
                recentValue = CalculateFinalValue();
                isDirty = false;
            }
            return recentValue;
        }
    }

    protected float percentTotal = 100;
    public virtual float PercentValue{
        get{
            if(isDirty || BaseValue != lastBaseValue){
                lastBaseValue = BaseValue;
                recentValue = CalculateFinalValue();
                isDirty = false;
            }
            return percentTotal;
        }
    }
    protected float multTotal = 1;
    public virtual float MultValue{
        get{
            if(isDirty || BaseValue != lastBaseValue){
                lastBaseValue = BaseValue;
                recentValue = CalculateFinalValue();
                isDirty = false;
            }
            return multTotal;
        }
    }

    public float CombinedValue(float value){
        if(isDirty || value != lastCombinedValue || BaseValue != lastBaseValue){
            lastCombinedValue = value;
            lastBaseValue = BaseValue;
            recentCombinedValue = FindCombinedValue(value);
            isDirty = false;
        }
        return recentCombinedValue;
    }

    public float FindCombinedValue(float value){
        float finalValue = BaseValue;
        float sumPercentAdd = 0;
        finalValue += value;
        for(int i = 0; i < statModifiers.Count; i++){
            StatModifier mod = statModifiers[i];
            if(mod.Type == StatModType.Flat){
                finalValue += statModifiers[i].Value;
            }else if(mod.Type == StatModType.PercentAdd){
                sumPercentAdd += mod.Value;
                if(i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != StatModType.PercentAdd){
                    finalValue *= 1 + sumPercentAdd;
                    sumPercentAdd = 0;
                }
            }else if(mod.Type == StatModType.PercentMult){
                finalValue *= 1 + mod.Value;
            }
        }
        return (float)Math.Round(finalValue, 4);
    }

    public AdvStat()
    {
        statModifiers = new List<StatModifier>();
        StatModifiers = statModifiers.AsReadOnly();
    }
 
    public AdvStat(float baseValue) : this()
    {
        BaseValue = baseValue;
    }

    public virtual void AddModifier(StatModifier mod){
        isDirty = true;
        statModifiers.Add(mod);
        statModifiers.Sort(CompareModifierOrder);
    }
    public virtual bool RemoveModifier(StatModifier mod){
        if(statModifiers.Remove(mod)){
            isDirty = true;
            return true;
        }
        return false;
    }
    public virtual bool RemoveAllModifiersFromSource(object source){
        bool didRemove = false;
        for(int i = statModifiers.Count - 1; i >= 0; i--){
            if(statModifiers[i].Source == source){
                isDirty = true;
                didRemove = true;
                statModifiers.RemoveAt(i);
            }
        }
        return didRemove;
    }
    public virtual bool RemoveModifierFromSource(object source){
        bool didRemove = false;
        for(int i = statModifiers.Count - 1; i >= 0; i--){
            if(statModifiers[i].Source == source){
                isDirty = true;
                didRemove = true;
                statModifiers.RemoveAt(i);
                break;
            }
        }
        return didRemove;
    }

    protected virtual int CompareModifierOrder(StatModifier a, StatModifier b){
        if(a.Order < b.Order){
            return -1;
        }else if (a.Order > b.Order){
            return 1;
        }
        return 0;
    }

    protected virtual float CalculateFinalValue(){
        float finalValue = BaseValue;
        float sumPercentAdd = 0;
        float multAdded = 1;
        for(int i = 0; i < statModifiers.Count; i++){
            StatModifier mod = statModifiers[i];
            if(mod.Type == StatModType.Flat){
                finalValue += statModifiers[i].Value;
            }else if(mod.Type == StatModType.PercentAdd){
                sumPercentAdd += mod.Value;
                if(i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != StatModType.PercentAdd){
                    finalValue *= 1 + sumPercentAdd;
                    multAdded += sumPercentAdd;
                    sumPercentAdd = 0;
                }
            }else if(mod.Type == StatModType.PercentMult){
                multAdded *= mod.Value;
                finalValue *= 1 + mod.Value;
            }
        }
        multAdded = Mathf.Clamp(multAdded, 0, Mathf.Infinity);
        multTotal = multAdded;
        percentTotal = (multAdded * 100);
        return (float)Math.Round(finalValue, 4);
    }
}
