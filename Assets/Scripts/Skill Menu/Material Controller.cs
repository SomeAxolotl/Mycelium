using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;

public class MaterialController : MonoBehaviour
{
    public NutrientTracker currentnutrients;
    public TMP_Text LogText;
    public TMP_Text ExoText;
    public TMP_Text CalciteText;
    public TMP_Text FleshText;
    //public TMP_Text Nutrients;
   
    void OnEnable()
    {
        currentnutrients = GameObject.FindWithTag("Tracker").GetComponent<NutrientTracker>();
        LogText.text = currentnutrients.storedLog.ToString();
        ExoText.text = currentnutrients.storedExoskeleton.ToString();
        CalciteText.text = currentnutrients.storedCalcite.ToString();
        FleshText.text = currentnutrients.storedFlesh.ToString();
        //Nutrients.text = currentnutrients.currentNutrients.ToString();
    }
}
