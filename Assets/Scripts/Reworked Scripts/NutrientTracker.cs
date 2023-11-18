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
        hudNutrients.UpdateNutrientsUI(currentNutrients);
    }
    public void AddNutrients(int amount)
    {
        currentNutrients += amount;
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
        storedLog = heldLog;
        storedExoskeleton = heldExoskeleton;
        storedCalcite = heldCalcite;
        storedFlesh = heldFlesh;
    }
}
