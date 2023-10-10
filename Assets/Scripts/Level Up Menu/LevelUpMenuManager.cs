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
    public TMP_Text Nutrients;
  
    public StatTracker currentstattracker;
    public GameObject UIenable;
    public GameObject HUD;
    void Start()
    {
        
    }

    
    void Update()
    {
        UpdateUI();
    }
    void UpdateUI()
    {
        
        currentstattracker = GameObject.FindWithTag("currentPlayer").GetComponent<StatTracker>();
        PrimalText.text = currentstattracker.primalLevel.ToString(); 
        SpeedText.text = currentstattracker.speedLevel.ToString(); 
        SentienceText.text = currentstattracker.sentienceLevel.ToString();
        VitalityText.text = currentstattracker.vitalityLevel.ToString(); 
        Nutrients.text = "Nutrients: " + PlayerPrefs.GetInt("currentNutrients").ToString(); 
    }

    public void PrimalUP()
    {
       currentstattracker = GameObject.FindWithTag("currentPlayer").GetComponent<StatTracker>();
       currentstattracker.primalLevel += 1;
       PlayerPrefs.SetInt("currentNutrients", PlayerPrefs.GetInt("currentNutrients",0) -100);
      
 
    }
    public void PrimalDown()
    {
       currentstattracker = GameObject.FindWithTag("currentPlayer").GetComponent<StatTracker>();
       currentstattracker.primalLevel -= 1;
       PlayerPrefs.SetInt("currentNutrients", PlayerPrefs.GetInt("currentNutrients",0) +100);
       
       
    }
    public void SpeedUP()
    {
       currentstattracker = GameObject.FindWithTag("currentPlayer").GetComponent<StatTracker>();
       currentstattracker.speedLevel += 1;
       PlayerPrefs.SetInt("currentNutrients", PlayerPrefs.GetInt("currentNutrients",0) -100);
     
       
    }
    public void SpeedDown()
    {
       currentstattracker = GameObject.FindWithTag("currentPlayer").GetComponent<StatTracker>();
       currentstattracker.speedLevel -= 1;
       PlayerPrefs.SetInt("currentNutrients", PlayerPrefs.GetInt("currentNutrients",0) +100);
       
       
    }
     public void SentienceUP()
    {
       currentstattracker = GameObject.FindWithTag("currentPlayer").GetComponent<StatTracker>();
       currentstattracker.sentienceLevel += 1;
       PlayerPrefs.SetInt("currentNutrients", PlayerPrefs.GetInt("currentNutrients",0) -100);
      
       
    }
    public void SentienceDown()
    {
       currentstattracker = GameObject.FindWithTag("currentPlayer").GetComponent<StatTracker>();
       currentstattracker.sentienceLevel -= 1;
       PlayerPrefs.SetInt("currentNutrients", PlayerPrefs.GetInt("currentNutrients",0) +100);
       
       
    }
     public void VitalityUP()
    {
       currentstattracker = GameObject.FindWithTag("currentPlayer").GetComponent<StatTracker>();
       currentstattracker.vitalityLevel += 1;
       PlayerPrefs.SetInt("currentNutrients", PlayerPrefs.GetInt("currentNutrients",0) -100);
       
       
    }
    public void VitalityDown()
    {
       currentstattracker = GameObject.FindWithTag("currentPlayer").GetComponent<StatTracker>();
       currentstattracker.vitalityLevel -= 1;
       PlayerPrefs.SetInt("currentNutrients", PlayerPrefs.GetInt("currentNutrients",0) +100);
      
       
    }
    public void Commit()
    {
      UIenable.SetActive(false);
      HUD.SetActive(true);
    }
}
