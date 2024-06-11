using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DefenseChange : MonoBehaviour
{
    private PlayerHealth playerHealth;
    private EnemyHealth enemyHealth;
    private BossHealth bossHealth;

    //List of all vulnerables and defenses on target
    private List<defenseChangeInfo> vulnerables = new List<defenseChangeInfo>();
    private List<defenseChangeInfo> defenses = new List<defenseChangeInfo>();

    private string vulnerableParticlePath = "Effects/VulnerableParticles";
    [SerializeField] private DefenseBubble vulnerableParticles;
    private string defenseParticlePath = "Effects/DefenseParticles";
    [SerializeField] private DefenseBubble defenseParticles;

    DefenseChange[] defenseInstances;
    void Awake(){
        defenseInstances = GetComponents<DefenseChange>();
        if(defenseInstances.Length == 1){
            accepting = true;
        }
        playerHealth = GetComponentInParent<PlayerHealth>();
        if(playerHealth == null){
            playerHealth = GetComponent<PlayerHealth>();
        }
        enemyHealth = GetComponent<EnemyHealth>();
        bossHealth = GetComponent<BossHealth>();

        //defenseParticles = Resources.Load<GameObject>(defenseParticlePath);
        GameObject defenseParticlesObj = Resources.Load<GameObject>(defenseParticlePath);
        GameObject tempObj1;
        if(playerHealth != null){
            tempObj1 = Instantiate(defenseParticlesObj, transform.Find("Spore/SporeModel")) as GameObject;
        }else{
            tempObj1 = Instantiate(defenseParticlesObj, transform) as GameObject;
        }
        defenseParticles = tempObj1.GetComponent<DefenseBubble>();
        defenseParticles.SwitchStatement(3);

        GameObject vulnerableParticlesObj = Resources.Load<GameObject>(vulnerableParticlePath);
        GameObject tempObj2;
        if(playerHealth != null){
            tempObj2 = Instantiate(vulnerableParticlesObj, transform.Find("Spore/SporeModel")) as GameObject;
        }else{
            tempObj2 = Instantiate(vulnerableParticlesObj, transform) as GameObject;
        }
        vulnerableParticles = tempObj2.GetComponent<DefenseBubble>();
        vulnerableParticles.SwitchStatement(3);
        Subscribe();
    }

    void Subscribe(){
        if(playerHealth != null){
            playerHealth.TakeDamage += ChangeDamageTaken;
            playerHealth.Died += defenseParticles.DisableBubble;
            playerHealth.Died += vulnerableParticles.DisableBubble;
        }
        if(enemyHealth != null){
            enemyHealth.TakeDamage += ChangeDamageTaken;
            enemyHealth.Died += defenseParticles.DisableBubble;
            enemyHealth.Died += vulnerableParticles.DisableBubble;
        }
        if(bossHealth != null){
            bossHealth.TakeDamage += ChangeDamageTaken;
            bossHealth.Died += defenseParticles.DisableBubble;
            bossHealth.Died += vulnerableParticles.DisableBubble;
        }
    }
    void OnDisable(){
        if(playerHealth != null){
            playerHealth.TakeDamage -= ChangeDamageTaken;
            playerHealth.Died -= defenseParticles.DisableBubble;
            playerHealth.Died -= vulnerableParticles.DisableBubble;
        }
        if(enemyHealth != null){
            enemyHealth.TakeDamage -= ChangeDamageTaken;
            enemyHealth.Died -= defenseParticles.DisableBubble;
            enemyHealth.Died -= vulnerableParticles.DisableBubble;
        }
        if(bossHealth != null){
            bossHealth.TakeDamage -= ChangeDamageTaken;
            bossHealth.Died -= defenseParticles.DisableBubble;
            bossHealth.Died -= vulnerableParticles.DisableBubble;
        }
    }

    private bool accepting = false;
    public void InitializeDefenseChange(float duration = 5, float changeAmount = -50, bool fading = false){
        //Checks to see if there are other instances of defenseChange already on the target
        if(defenseInstances.Length > 1 && accepting == false){
            //Combines the defenseChange
            foreach(DefenseChange defenseChange in defenseInstances){
                if(defenseChange != this && defenseChange.accepting == true){
                    defenseChange.InitializeDefenseChange(duration, changeAmount);
                }
            }
            Destroy(this);
            return;
        }
        if(duration == 0 || changeAmount == 0){return;}
        if(changeAmount < 0){
            AddEffect(changeAmount, duration, fading, vulnerables);
        }else{
            AddEffect(changeAmount, duration, fading, defenses);
        }
    }

    //The logic for comparing the vulnerables on the target
    public void AddEffect(float changeAmount, float duration, bool fading, List<defenseChangeInfo> list){
        defenseChangeInfo newEffect = new defenseChangeInfo();
        newEffect.changeAmount = changeAmount;
        newEffect.changeDuration = duration;
        newEffect.fading = fading;
        //Checks the status of the effect
        bool passed = true;
        foreach(defenseChangeInfo effect in list){
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
                list.Remove(effect);
                break;
            }
        }

        if(passed){
            list.Add(newEffect);
            //Starts the duration timer for the effect
            StartCoroutine(DefenseChangeTimer(newEffect));
            //Compares and then applies defense change
            DefenseUpdate();
        }
    }

    float defenseChangePercent = 0;
    private void DefenseUpdate(){
        //If we run out of defense changes
        //Debug.Log("Vulnerable length: " + vulnerables.Count + "         Defense Length: " + defenses.Count);
        if(vulnerables.Count == 0 && defenses.Count == 0){
            Destroy(defenseParticles.gameObject);
            Destroy(vulnerableParticles.gameObject);
            Destroy(this);
            return;
        }
        //Sorts the list by their strengths
        vulnerables = vulnerables.OrderBy(info => info.changeAmount).ToList();
        defenses = defenses.OrderBy(info => info.changeAmount).ToList();
        //Combines the vulnerable and the defense to get the victor
        if(vulnerables.Count != 0 && defenses.Count != 0){
            defenseChangePercent = defenses[0].changeAmount + vulnerables[0].changeAmount;
        }else{
            if(defenses.Count != 0){defenseChangePercent = defenses[0].changeAmount;}
            if(vulnerables.Count != 0){defenseChangePercent = vulnerables[0].changeAmount;}
        }
        ManageBubbles();
    }

    private void ManageBubbles(){
        //Reset their timers (So they know when to fade out)
        if(defenses.Count > 0){defenseParticles.timeLeft = defenses[0].changeDuration;}
        if(vulnerables.Count > 0){vulnerableParticles.timeLeft = vulnerables[0].changeDuration;}
        //Change the current states of the bubbles
        if(defenseChangePercent > 0){
            //Turn on Defense
            defenseParticles.SwitchStatement(0);
            vulnerableParticles.SwitchStatement(2);
        }else if(defenseChangePercent < 0){
            //Turn on Vulnerable
            defenseParticles.SwitchStatement(2);
            vulnerableParticles.SwitchStatement(0);
        }else{
            //They are even?
            defenseParticles.SwitchStatement(2);
            vulnerableParticles.SwitchStatement(2);
        }
    }

    private void ChangeDamageTaken(float dmgTaken){
        //Code to change damage taken
        //Debug.Log("Damage has been taken: " + dmgTaken);
        if(playerHealth != null){
            playerHealth.dmgTaken -= (dmgTaken * (defenseChangePercent / 100));
            //Debug.Log("Damage was changed to: " + playerHealth.dmgTaken);
        }
        if(enemyHealth != null){
            enemyHealth.dmgTaken -= (dmgTaken * (defenseChangePercent / 100));
            //Debug.Log("Damage was changed to: " + enemyHealth.dmgTaken);
        }
        if(bossHealth != null){
            bossHealth.dmgTaken -= (dmgTaken * (defenseChangePercent / 100));
            //Debug.Log("Damage was changed to: " + bossHealth.dmgTaken);
        }
    }

    IEnumerator DefenseChangeTimer(defenseChangeInfo defenseChange){
        //Remember the max numbers
        defenseChange.changeDurationMax = defenseChange.changeDuration;
        defenseChange.changeAmountMax = defenseChange.changeAmount;
        //Logic to spawn particles and change appearance goes here
        while(defenseChange.changeDuration > 0){
            defenseChange.changeDuration -= Time.deltaTime;
            //If the change is of the fading type, decrease the charge amount and update defense
            if(defenseChange.fading){
                defenseChange.changeAmount = (defenseChange.changeDuration / defenseChange.changeDurationMax) * defenseChange.changeAmountMax;
                DefenseUpdate();
            }
            yield return null;
        }
        defenses.Remove(defenseChange);
        vulnerables.Remove(defenseChange);
        //Makes sure to recompare defense changes due to one being removed
        DefenseUpdate();
    }
}

public class defenseChangeInfo{
    public float changeDurationMax = 5;
    public float changeDuration = 5;
    public float changeAmountMax = 0;
    public float changeAmount = 0;
    public bool fading = false;
}
