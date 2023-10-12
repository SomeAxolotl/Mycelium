using System.Collections;
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
  
    public StatTracker currentstattracker;
    public GameObject UIenable;
    public GameObject HUD;

    private NutrientTracker nutrientTracker;

    private PlayerController playerController;
    private PlayerHealth playerHealth;
    private MeleeAttack meleeAttack;

    [SerializeField]
    private GameObject eruptionObject, fungalMightObject;

    void Start()
    {
      playerController = GameObject.FindWithTag("currentPlayer").GetComponent<PlayerController>();
      playerHealth = GameObject.FindWithTag("currentPlayer").GetComponent<PlayerHealth>();
      meleeAttack = GameObject.FindWithTag("currentPlayer").GetComponent<MeleeAttack>();
      currentstattracker = GameObject.FindWithTag("currentPlayer").GetComponent<StatTracker>();
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
       currentstattracker = GameObject.FindWithTag("currentPlayer").GetComponent<StatTracker>();
       currentstattracker.IncreaseStat("primal", 1);
       nutrientTracker.SubtractNutrients(100);
    }
    public void PrimalDown()
    {
       currentstattracker = GameObject.FindWithTag("currentPlayer").GetComponent<StatTracker>();
       currentstattracker.primalLevel -= 1;
       nutrientTracker.AddNutrients(100);
       
       
    }
    public void SpeedUP()
    {
       currentstattracker = GameObject.FindWithTag("currentPlayer").GetComponent<StatTracker>();
       currentstattracker.IncreaseStat("speed", 1);
       nutrientTracker.SubtractNutrients(100);
     
       
    }
    public void SpeedDown()
    {
       currentstattracker = GameObject.FindWithTag("currentPlayer").GetComponent<StatTracker>();
       currentstattracker.speedLevel -= 1;
       nutrientTracker.AddNutrients(100);
       
       
    }
     public void SentienceUP()
    {
       currentstattracker = GameObject.FindWithTag("currentPlayer").GetComponent<StatTracker>();
       currentstattracker.IncreaseStat("sentience", 1);
       nutrientTracker.SubtractNutrients(100);
       
    }
    public void SentienceDown()
    {
       currentstattracker = GameObject.FindWithTag("currentPlayer").GetComponent<StatTracker>();
       currentstattracker.sentienceLevel -= 1;
       nutrientTracker.AddNutrients(100);
       
       
    }
     public void VitalityUP()
    {
       currentstattracker = GameObject.FindWithTag("currentPlayer").GetComponent<StatTracker>();
       currentstattracker.IncreaseStat("vitality", 1);
       nutrientTracker.SubtractNutrients(100);
       
       
    }
    public void VitalityDown()
    {
       currentstattracker = GameObject.FindWithTag("currentPlayer").GetComponent<StatTracker>();
       currentstattracker.vitalityLevel -= 1;
       nutrientTracker.AddNutrients(100);
      
       
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
}
