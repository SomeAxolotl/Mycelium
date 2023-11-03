using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDNutrients : MonoBehaviour
{
    private TMP_Text nutrientsNumberText;

    void Start()
    {
        nutrientsNumberText = GameObject.Find("NutrientsNumber").GetComponent<TMP_Text>();
    }

    public void UpdateNutrientsUI(int currentNutrients)
    {
        nutrientsNumberText.text = currentNutrients.ToString();
    }
}
