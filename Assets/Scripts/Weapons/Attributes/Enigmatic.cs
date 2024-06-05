using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enigmatic : AttributeBase
{
    private bool hitSomething = false;
    private bool repeat = false;

    public override void Initialize(){
        attName = "Enigmatic";
        attDesc = "+50% Damage, changes on hit";
        if(stats == null || hit == null){return;}
        stats.wpnName = attName + " " + stats.wpnName;

        stats.wpnMult *= 1.5f;

        //If there are multiple enigmatics turn this off
        if(GetComponentsInChildren<Enigmatic>().Length > 1){
            repeat = true;
        }
    }

    public override void StartAttack(){
        if(repeat){return;}
        hitSomething = true;
    }

    public override void Hit(GameObject target, float damage){
        if(repeat){return;}
        if(!hitSomething){
            hitSomething = true;
        }
    }

    public List<AttributeInfo> copyAtts = new List<AttributeInfo>();
    public List<AttributeBase> newAtts = new List<AttributeBase>();
    public override void StopAttack(){
        //If you hit something, swap weapons
        if(hitSomething){
            //Checks that the weapon is not going to break
            AttributeBase fragileAtt = GetComponent<Fragile>();
            if(fragileAtt != null && fragileAtt.specialAttNum <= 0){return;}
            //Poof particles
            ParticleManager.Instance.SpawnParticles("TrophicCascadePoof", transform.position, Quaternion.Euler(-90,0,0), null, new Vector3(0.3f, 0.3f, 0.3f));

            playerParent = player.transform.parent.gameObject;
            swap = playerParent.GetComponent<SwapWeapon>();
            GameObject randomWeapon = Instantiate(Resources.Load(RandomWeapon(gameObject)), GameObject.FindWithTag("WeaponSlot").transform) as GameObject;
            if(randomWeapon.GetComponent<WeaponStats>().wpnName != "Stick"){
                randomWeapon.transform.localScale = randomWeapon.transform.localScale / 0.03f / 100f / 1.2563f;
            }
            randomWeapon.GetComponent<WeaponStats>().acceptingAttribute = false;
            
            //Get all current attributes as strings
            AttributeBase[] attsToCopy = GetComponents<AttributeBase>();
            foreach(AttributeBase currAtt in attsToCopy){
                AttributeInfo newAtt = new AttributeInfo();
                newAtt.attName = currAtt.attName;
                newAtt.attValue = currAtt.specialAttNum;
                newAtt.rating = currAtt.rating;
                copyAtts.Add(newAtt);
                currAtt.Unequipped();
            }
            //Adds the attributes onto the new weapon
            foreach(AttributeInfo currAtt in copyAtts){
                AttributeBase newAtt = AttributeAssigner.Instance.PickAttFromString(randomWeapon, currAtt.attName);
                newAtt.specialAttNum = currAtt.attValue;
                newAtt.rating = currAtt.rating;
                newAtts.Add(newAtt);
            }

            randomWeapon.layer = LayerMask.NameToLayer("currentWeapon");
            randomWeapon.GetComponent<Collider>().enabled = false;
            gameObject.tag = "Weapon";
            randomWeapon.tag = "currentWeapon";
            StartCoroutine(SetPreviousWeaponStats(randomWeapon));
        }
        hitSomething = false;
    }

    IEnumerator SetPreviousWeaponStats(GameObject randomWeapon){
        //Next frame do these things
        yield return 0;
        Destroy(gameObject);
        randomWeapon.GetComponent<WeaponInteraction>().ApplyWeaponPositionAndRotation();
        foreach(AttributeBase currAtt in newAtts){
            currAtt.Equipped();
        }
        //randomWeapon.GetComponent<Enigmatic>().Equipped();
    }

    public void OnDestroy(){
        if(attack != null){
            attack.StartedAttack -= StartAttack;
            attack.FinishedAttack -= StopAttack;
        }
    }

    private string RandomWeapon(GameObject currWeapon, float infiniteProtection = 0){
        //Protection against it unluckily looping for too long, having a low number also makes it rarely return a stick
        infiniteProtection += 1;
        if(infiniteProtection > 2){return "Slash/Stick";}
        switch(Random.Range(0, 8)){
            case 0:
                if(currWeapon.GetComponent<WeaponStats>().wpnName.Contains("Avocado Flamberge")){return(RandomWeapon(currWeapon, infiniteProtection));}
                return "Slash/AvocadoFlamberge";
            case 1:
                if(currWeapon.GetComponent<WeaponStats>().wpnName.Contains("Obsidian Scimitar")){return(RandomWeapon(currWeapon, infiniteProtection));}
                return "Slash/ObsidianScimitar";
            case 2:
                if(currWeapon.GetComponent<WeaponStats>().wpnName.Contains("Mandible Sickle")){return(RandomWeapon(currWeapon, infiniteProtection));}
                return "Slash/MandibleSickle";
            case 3:
                if(currWeapon.GetComponent<WeaponStats>().wpnName.Contains("Rose Mace")){return(RandomWeapon(currWeapon, infiniteProtection));}
                return "Smash/RoseMace";
            case 4:
                if(currWeapon.GetComponent<WeaponStats>().wpnName.Contains("Geode Hammer")){return(RandomWeapon(currWeapon, infiniteProtection));}
                return "Smash/GeodeHammer";
            case 5:
                if(currWeapon.GetComponent<WeaponStats>().wpnName.Contains("Femur Club")){return(RandomWeapon(currWeapon, infiniteProtection));}
                return "Smash/FemurClub";
            case 6:
                if(currWeapon.GetComponent<WeaponStats>().wpnName.Contains("Bamboo Partisan")){return(RandomWeapon(currWeapon, infiniteProtection));}
                return "Stab/BambooPartisan";
            case 7:
                if(currWeapon.GetComponent<WeaponStats>().wpnName.Contains("Opal Rapier")){return(RandomWeapon(currWeapon, infiniteProtection));}
                return "Stab/OpalRapier";
            case 8:
                if(currWeapon.GetComponent<WeaponStats>().wpnName.Contains("Carpal Sais")){return(RandomWeapon(currWeapon, infiniteProtection));}
                return "Stab/CarpalSais";
            default:
                return "Slash/Stick";
        }
    }
}
