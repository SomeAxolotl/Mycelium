using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDNutrients : MonoBehaviour
{
    private NutrientTracker nutrientsTracker;
    private int nutrientsNumber;

    private TMP_Text nutrientsNumberText;

    void Start()
    {
        nutrientsTracker = GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>();
        nutrientsNumberText = GameObject.Find("NutrientsNumber").GetComponent<TMP_Text>();
        UpdateNutrientsUI();
    }

    public void UpdateNutrientsUI()
    {
        nutrientsNumber = PlayerPrefs.GetInt("currentNutrients");
        nutrientsNumberText.text = nutrientsNumber.ToString();
    }

    private void AddTestNutrients()
    {
        int newNutrients = PlayerPrefs.GetInt("currentNutrients") + 50;
        PlayerPrefs.SetInt("currentNutrients", newNutrients);
    }

    IEnumerator NutrientsTest()
    {
        yield return new WaitForSeconds(1.0f);
        nutrientsNumber += 10;
        UpdateNutrientsUI();
        yield return new WaitForSeconds(1.0f);
        nutrientsNumber += 100;
        UpdateNutrientsUI();
        yield return new WaitForSeconds(1.0f);
        nutrientsNumber -= 12;
        UpdateNutrientsUI();
        yield return new WaitForSeconds(1.0f);
        nutrientsNumber += 37;
        UpdateNutrientsUI();
        yield return new WaitForSeconds(1.0f);
        nutrientsNumber += 1;
        UpdateNutrientsUI();
    }
}
