using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NutrientTracker : MonoBehaviour
{
    [Header("Current Nutrients")]
    public int currentNutrients;

    public GameObject heldItem;

    public int heldLog;
    public int heldExoskeleton;
    public int heldCalcite;
    public int heldFlesh;

    public int storedLog;
    public int storedExoskeleton;
    public int storedCalcite;
    public int storedFlesh;

    private HUDNutrients hudNutrients;

    // Start is called before the first frame update
    void Start()
    {
        hudNutrients = GameObject.Find("HUD").GetComponent<HUDNutrients>();
        hudNutrients.UpdateNutrientsUI(currentNutrients);
    }

    // Update is called once per frame
    void Update()
    {
        //FOR TESTING

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            AddNutrients(100);
        }
    }
    public void SubtractNutrients(int cost)
    {
        currentNutrients -= cost;
        hudNutrients = GameObject.Find("HUD").GetComponent<HUDNutrients>();
        hudNutrients.UpdateNutrientsUI(currentNutrients);
    }
    public void AddNutrients(int amount)
    {
        currentNutrients += amount;
        hudNutrients = GameObject.Find("HUD").GetComponent<HUDNutrients>();
        hudNutrients.UpdateNutrientsUI(currentNutrients);
    }
    public void LoseMaterials()
    {
        heldLog = 0;
        heldExoskeleton = 0;
        heldCalcite = 0;
        heldFlesh = 0;
    }
    public void KeepMaterials()
    {
        if (heldLog != 0)
        {
            storedLog++;
        }
        else if (heldExoskeleton != 0)
        {
            storedExoskeleton++;
        }
        else if (heldCalcite != 0)
        {
            storedCalcite++;
        }
        else if (heldFlesh != 0)
        {
            storedFlesh++;
        }
    }
    public void SpendLog(int cost)
    {
        storedLog -= cost;
    }
    public void SpendExoskeleton(int cost)
    {
        storedExoskeleton -= cost;
    }
    public void SpendCalcite(int cost)
    {
        storedCalcite -= cost;
    }
    public void SpendFlesh(int cost)
    {
        storedFlesh -= cost;
    }
}
