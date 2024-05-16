using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golden : AttributeBase
{
    private Renderer wepRen;

    public override void Initialize(){
        if(stats == null || hit == null){return;}
        attName = "Golden";
        attDesc = "\n<sprite="+0+"> +1 <sprite="+2+"> +1 <sprite="+1+"> +1 <sprite="+3+"> +1";
        stats.wpnName = attName + " " + stats.wpnName;
        interact.attributeDescription = attDesc;
        SetGolden();
    }

    public override void Equipped(){
        characterStats.AddStat("Primal", 1);
        characterStats.AddStat("Sentience", 1);
        characterStats.AddStat("Speed", 1);
        characterStats.AddStat("Vitality", 1);
    }

    public override void Unequipped(){
        characterStats.AddStat("Primal", -1);
        characterStats.AddStat("Sentience", -1);
        characterStats.AddStat("Speed", -1);
        characterStats.AddStat("Vitality", -1);
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
