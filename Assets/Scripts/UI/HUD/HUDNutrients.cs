using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDNutrients : MonoBehaviour
{
    [SerializeField] private TMP_Text nutrientsNumberText;
    [SerializeField] private TMP_Text nutrientMultiplierText;

    public void UpdateNutrientsUI(int currentNutrients)
    {
        //Debug.Log("Nutrients: " + currentNutrients);
        nutrientsNumberText.text = currentNutrients.ToString();
    }

    public void UpdateNutrientMultiplierUI()
    {
        if (GlobalData.currentLoop >= 2)
        {
            nutrientMultiplierText.text = "+" + ((GlobalData.currentLoop - 1) * 50) + "%";
        }
        else
        {
            nutrientMultiplierText.text = "";
        }

        Debug.Log("Current Loop: " + GlobalData.currentLoop);
    }
}
