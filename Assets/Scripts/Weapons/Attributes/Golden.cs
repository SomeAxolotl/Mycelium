using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golden : AttributeBase
{
    private Renderer wepRen;

    public override void Initialize(){
        primalAmount = 1;
        sentienceAmount = 1;
        speedAmount = 1;
        vitalityAmount = 1;
        
        attName = "Golden";
        if(stats == null || hit == null){return;}
        SetGolden();
    }

    public override void Equipped(){
        characterStats.AddStat("Primal", primalAmount);
        characterStats.AddStat("Sentience", sentienceAmount);
        characterStats.AddStat("Speed", speedAmount);
        characterStats.AddStat("Vitality", vitalityAmount);
    }

    public override void Unequipped(){
        characterStats.AddStat("Primal", -primalAmount);
        characterStats.AddStat("Sentience", -sentienceAmount);
        characterStats.AddStat("Speed", -speedAmount);
        characterStats.AddStat("Vitality", -vitalityAmount);
    }

    private void SetGolden(){
        wepRen = GetComponent<Renderer>();
        Material weapon;
        switch(wepRen.materials[0].name){
            case "AvacadoHilt (Instance)":
                weapon = Resources.Load("Gold/AvacadoHiltGold", typeof(Material)) as Material;
                Material leaf = Resources.Load<Material>("Gold/AvacadoLeafGold");
                wepRen.materials = new Material[]{weapon, leaf};
                break;
            case "MandibleSickle (Instance)":
                weapon = Resources.Load("Gold/MandibleSickleGold", typeof(Material)) as Material;
                wepRen.materials = new Material[]{weapon};
                break;
            case "ObsidianSchmitar (Instance)":
                weapon = Resources.Load("Gold/ObsidianSchmitarGold", typeof(Material)) as Material;
                wepRen.materials = new Material[]{weapon};
                break;
            case "RoseMace (Instance)":
                weapon = Resources.Load("Gold/RoseMaceGold", typeof(Material)) as Material;
                wepRen.materials = new Material[]{weapon};
                break;
            case "FemurClub (Instance)":
                weapon = Resources.Load("Gold/FemurClubGold", typeof(Material)) as Material;
                wepRen.materials = new Material[]{weapon};
                break;
            case "Geode (Instance)":
                weapon = Resources.Load("Gold/GeodeGold", typeof(Material)) as Material;
                Material outside = Resources.Load("Gold/OuterGeodeGold", typeof(Material)) as Material;
                Material spikes = Resources.Load("Gold/GeodeSpikesGold", typeof(Material)) as Material;
                wepRen.materials = new Material[]{weapon, outside, spikes};
                break;
            case "Bamboo (Instance)":
                weapon = Resources.Load("Gold/BambooGold", typeof(Material)) as Material;
                wepRen.materials = new Material[]{weapon};
                break;
            case "CarpalSais (Instance)":
                weapon = Resources.Load("Gold/CarpalSaisGold", typeof(Material)) as Material;
                wepRen.materials = new Material[]{weapon};
                break;
            case "OpalRapier (Instance)":
                weapon = Resources.Load("Gold/OpalRapierGold", typeof(Material)) as Material;
                wepRen.materials = new Material[]{weapon};
                break;
            default:
                Debug.Log("Missing material: " + wepRen.materials[0]);
                break;
        }
    }
}
