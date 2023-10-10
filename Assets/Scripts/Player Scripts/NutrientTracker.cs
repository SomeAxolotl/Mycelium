using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NutrientTracker : MonoBehaviour
{
    [Header("Current Nutrients")]
    public int currentNutrients;

    private HUDNutrients hudNutrients;
    // Start is called before the first frame update
    void Start()
    {
        hudNutrients = GameObject.Find("HUD").GetComponent<HUDNutrients>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.parent = GameObject.FindWithTag("currentPlayer").transform.parent;
    }
    public void SaveNutrients()
    {
        PlayerPrefs.SetInt("currentNutrients", currentNutrients);
        hudNutrients.UpdateNutrientsUI();
    }
    public void GetNutrients()
    {
        currentNutrients = PlayerPrefs.GetInt("currentNutrients");
    }
}
