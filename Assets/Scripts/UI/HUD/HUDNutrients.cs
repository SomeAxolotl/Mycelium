using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDNutrients : MonoBehaviour
{
    [SerializeField] private TMP_Text nutrientsNumberText;

    public void UpdateNutrientsUI(int currentNutrients)
    {
        Debug.Log("Nutrients: " + currentNutrients);
        nutrientsNumberText.text = currentNutrients.ToString();
    }
}
