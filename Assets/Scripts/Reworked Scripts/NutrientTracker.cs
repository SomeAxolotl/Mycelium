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
        //FOR TESTING

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            currentNutrients += 1000;
        }
    }
    public void SubtractNutrients(int cost)
    {
        currentNutrients -= cost;
        hudNutrients.UpdateNutrientsUI(currentNutrients);
    }
    public void AddNutrients(int amount)
    {
        currentNutrients += amount;
        hudNutrients.UpdateNutrientsUI(currentNutrients);
    }
}
