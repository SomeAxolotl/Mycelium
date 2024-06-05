using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpeedChange : MonoBehaviour
{
    private ReworkedEnemyNavigation enemyNav;
    private MeleeEnemyAttack chargeAttack;
    private PlayerController controller;

    //List of all slows and speeds on target
    private List<speedChangeInfo> slows = new List<speedChangeInfo>();
    private List<speedChangeInfo> speeds = new List<speedChangeInfo>();

    private string slowParticlePath = "Effects/SlowParticles";
    [SerializeField] private GameObject slowParticles;
    private string speedParticlePath = "Effects/SpeedParticles";
    [SerializeField] private GameObject speedParticles;
    private TrailRenderer speedTrail;

    SpeedChange[] speedInstances;
    void Awake(){
        speedInstances = GetComponents<SpeedChange>();
        if(speedInstances.Length == 1){
            accepting = true;
        }
        enemyNav = GetComponent<ReworkedEnemyNavigation>();
        if(enemyNav != null){
            baseSpeed = enemyNav.moveSpeed;
        }
        chargeAttack = GetComponent<MeleeEnemyAttack>();
        if(chargeAttack != null){
            baseChargeSpeed = chargeAttack.chargeSpeed;
        }
        controller = GetComponentInParent<PlayerController>();
        if(controller != null){
            baseSpeed = controller.moveSpeed;
        }
        speedParticles = Resources.Load<GameObject>(speedParticlePath);
        slowParticles = Resources.Load<GameObject>(slowParticlePath);
    }

    private bool accepting = false;
    public void InitializeSpeedChange(float duration = 5, float changeAmount = -50, bool fading = false){
        //Checks to see if there are other instances of speedChange already on the target
        if(speedInstances.Length > 1 && accepting == false){
            //Combines the speedChange
            foreach(SpeedChange speedChange in speedInstances){
                if(speedChange != this && speedChange.accepting == true){
                    speedChange.InitializeSpeedChange(duration, changeAmount);
                }
            }
            Destroy(this);
            return;
        }
        if(duration == 0 || changeAmount == 0){return;}
        if(changeAmount < 0){
            AddEffect(changeAmount, duration, fading, slows);
        }else{
            AddEffect(changeAmount, duration, fading, speeds);
        }
    }

    //The logic for comparing the slows on the target
    List<speedChangeInfo> toRemove = new List<speedChangeInfo>();
    public void AddEffect(float changeAmount, float duration, bool fading, List<speedChangeInfo> list){
        speedChangeInfo newEffect = new speedChangeInfo();
        newEffect.changeAmount = changeAmount;
        newEffect.changeDuration = duration;
        newEffect.fading = fading;
        //Checks the status of the effect
        bool passed = true;
        foreach(speedChangeInfo effect in list){
            //If the effect is the same changeAmount, refresh the original effect
            if(newEffect.changeAmount == effect.changeAmount){
                if(newEffect.changeDuration > effect.changeDuration){
                    effect.changeDuration = newEffect.changeDuration;
                    passed = false;
                    break;
                }
            }
            //Does not add the effect if it finds an effect that outshines it in every way
            if(newEffect.changeAmount < effect.changeAmount && newEffect.changeDuration < effect.changeDuration){
                passed = false;
            }
            //Removes effects that are outshined
            if(newEffect.changeAmount > effect.changeAmount && newEffect.changeDuration > effect.changeDuration){
                toRemove.Add(effect);
                break;
            }
        }
        foreach(var effect in toRemove){
            list.Remove(effect);
        }

        if(passed){
            list.Add(newEffect);
            //Starts the duration timer for the effect
            StartCoroutine(SpeedChangeTimer(newEffect));
            //Compares and then applies speed change
            SpeedUpdate();
        }
    }

    float baseSpeed;
    float baseChargeSpeed;
    public float speedChangePercent = 0;
    public void SpeedUpdate(){
        //If we run out of speed changes
        //Debug.Log("Slow length: " + slows.Count + "         Speed Length: " + speeds.Count);
        if(slows.Count == 0 && speeds.Count == 0){
            if(enemyNav != null){
                enemyNav.moveSpeed = baseSpeed;
            }
            if(chargeAttack != null){
                chargeAttack.chargeSpeed = baseChargeSpeed;
            }
            if(controller != null){
                controller.moveSpeed = baseSpeed;
            }
            if(speedTrail != null){speedTrail.emitting = false; speedTrail.autodestruct = true;}
            Destroy(this);
            return;
        }
        //Sorts the list by their strengths
        slows = slows.OrderBy(info => info.changeAmount).ToList();
        speeds = speeds.OrderBy(info => info.changeAmount).ToList();
        //Combines the slow and the speed to get the victor
        if(slows.Count != 0 && speeds.Count != 0){
            speedChangePercent = speeds[0].changeAmount + slows[0].changeAmount;
        }else{
            if(speeds.Count != 0){speedChangePercent = speeds[0].changeAmount; SpeedEffects(true);}
            if(slows.Count != 0){speedChangePercent = slows[0].changeAmount; SpeedEffects(false);}
        }
        //The clamp stops their movespeed from being negative and being weird
        if(enemyNav != null){
            enemyNav.moveSpeed = Mathf.Clamp(baseSpeed + (baseSpeed * (speedChangePercent / 100)), 0.0f, Mathf.Infinity);
        }
        if(chargeAttack != null){
            chargeAttack.chargeSpeed = Mathf.Clamp(baseChargeSpeed + (baseChargeSpeed * (speedChangePercent / 100)), 0.0f, Mathf.Infinity);
        }
        if(controller != null){
            controller.moveSpeed = Mathf.Clamp(baseSpeed + (baseSpeed * (speedChangePercent / 100)), 0.0f, Mathf.Infinity);
        }
        if(speedTrail != null){
            speedTrail.widthMultiplier = Mathf.Clamp(0.5f + (speedChangePercent * 0.01f), 0.5f, 2.5f);
        }
    }

    IEnumerator SpeedChangeTimer(speedChangeInfo speedChange){
        //Remember the max numbers
        speedChange.changeDurationMax = speedChange.changeDuration;
        speedChange.changeAmountMax = speedChange.changeAmount;
        //Logic to spawn particles and change appearance goes here
        while(speedChange.changeDuration > 0){
            speedChange.changeDuration -= Time.deltaTime;
            //If the change is of the fading type, decrease the charge amount and update speed
            if(speedChange.fading){
                speedChange.changeAmount = (speedChange.changeDuration / speedChange.changeDurationMax) * speedChange.changeAmountMax;
                SpeedUpdate();
            }
            yield return null;
        }
        speeds.Remove(speedChange);
        slows.Remove(speedChange);
        //Makes sure to recompare speed changes due to one being removed
        SpeedUpdate();
    }

    private void SpeedEffects(bool activate){
        if(speedTrail == null){
            GameObject speedParticlesObj = Resources.Load<GameObject>(speedParticlePath);
            GameObject tempObj1;
            if(controller != null){
                tempObj1 = Instantiate(speedParticlesObj, controller.animator.transform) as GameObject;
            }else{
                tempObj1 = Instantiate(speedParticlesObj, transform) as GameObject;
            }
            speedTrail = tempObj1.GetComponent<TrailRenderer>();
        }

        if(speedChangePercent > 0){
            speedTrail.emitting = true;
            speedTrail.autodestruct = false;
        }else{
            speedTrail.emitting = false;
            speedTrail.autodestruct = true;
        }
    }
}

public class speedChangeInfo{
    public float changeDurationMax = 5;
    public float changeDuration = 5;
    public float changeAmountMax = 0;
    public float changeAmount = 0;
    public bool fading = false;
}