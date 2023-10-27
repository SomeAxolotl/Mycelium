/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;

public class LevelUpMenuManager : MonoBehaviour
{
    public TMP_Text PrimalText;
    public TMP_Text SpeedText;
    public TMP_Text SentienceText;
    public TMP_Text VitalityText;
    public GameObject UIenable;
    public GameObject HUD;

    private NutrientTracker nutrientTracker;

    public CharacterStats currentstattracker;
   public New Player Health playerHealth;
  public New Player Attack meleeAttack;

    [SerializeField]
    private GameObject eruptionObject, fungalMightObject;

    void Start()
    {
      playerController = GameObject.FindWithTag("PlayerParent").GetComponent<Player Controller>();
      playerHealth = GameObject.FindWithTag("PlayerParent").GetComponent<New Player Health>();
      meleeAttack = GameObject.FindWithTag("PlayerParent").GetComponent<New Player Attack>();
      currentstattracker = GameObject.FindWithTag("currentPlayer").GetComponent<CharacterStats>();
      nutrientTracker = GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>();
    }

    
    void Update()
    {
        UpdateUI();
    }
    void UpdateUI()
    {
        
        PrimalText.text = currentstattracker.primalLevel.ToString(); 
        SpeedText.text = currentstattracker.speedLevel.ToString(); 
        SentienceText.text = currentstattracker.sentienceLevel.ToString();
        VitalityText.text = currentstattracker.vitalityLevel.ToString();
    }

    public void PrimalUP()
    {
      currentstattracker.LevelPrimal();  
    }
    public void PrimalDown()
    {
      if (currentstattracker.primalLevel > 0)
      {
       currentstattracker = GameObject.FindWithTag("currentPlayer").GetComponent<StatTracker>();
       currentstattracker.primalLevel -= 1;
       nutrientTracker.AddNutrients(100);
       }
       
    }
    public void SpeedUP()
    {
      if (EnoughNutrients())
       {
         currentstattracker = GameObject.FindWithTag("currentPlayer").GetComponent<StatTracker>();
         currentstattracker.IncreaseStat("speed", 1);
         nutrientTracker.SubtractNutrients(100);
       }
       
     
       
    }
    public void SpeedDown()
    {
      if (currentstattracker.speedLevel > 0)
      {
         currentstattracker = GameObject.FindWithTag("currentPlayer").GetComponent<StatTracker>();
         currentstattracker.speedLevel -= 1;
         nutrientTracker.AddNutrients(100);
      }
       
       
    }
     public void SentienceUP()
    {
      if (EnoughNutrients())
       {
        currentstattracker = GameObject.FindWithTag("currentPlayer").GetComponent<StatTracker>();
       currentstattracker.IncreaseStat("sentience", 1);
       nutrientTracker.SubtractNutrients(100);
       }
       
       
    }
    public void SentienceDown()
    {
      if (currentstattracker.sentienceLevel > 0)
      {
       currentstattracker = GameObject.FindWithTag("currentPlayer").GetComponent<StatTracker>();
       currentstattracker.sentienceLevel -= 1;
       nutrientTracker.AddNutrients(100);
       }
       
    }
     public void VitalityUP()
    {
      if (EnoughNutrients())
       {
         currentstattracker = GameObject.FindWithTag("currentPlayer").GetComponent<StatTracker>();
       currentstattracker.IncreaseStat("vitality", 1);
       nutrientTracker.SubtractNutrients(100);
       }
       
       
       
    }
    public void VitalityDown()
    {
       if (currentstattracker.vitalityLevel > 0)
      {
       currentstattracker = GameObject.FindWithTag("currentPlayer").GetComponent<StatTracker>();
       currentstattracker.vitalityLevel -= 1;
       nutrientTracker.AddNutrients(100);
      }
       
    }

    private bool EnoughNutrients()
    {
       if (nutrientTracker.GetNutrients() < 100)
       {
         return false;
       }
       return true;
    }

    public void Commit()
    {
         UIenable.SetActive(false);
         playerController.EnableController();
         meleeAttack.EnableAttack();
         playerController.UpdateStats();
         playerHealth.UpdateStats();

      if (currentstattracker.primalLevel >= 3 || currentstattracker.speedLevel >= 3 || currentstattracker.sentienceLevel >= 3 || currentstattracker.vitalityLevel >= 3)
      {
         Transform skillLoadout = GameObject.FindWithTag("currentPlayer").transform.Find("SkillLoadout");
         GameObject oldSkill = skillLoadout.GetChild(0).gameObject;

         Destroy(GameObject.FindWithTag("currentPlayer").transform.Find("SkillLoadout").GetChild(0).gameObject);
         Instantiate(eruptionObject, skillLoadout);
      }
    }
}*/
