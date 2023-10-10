using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NutrientTracker : MonoBehaviour
{
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

    public void AddNutrients(int addedNutrients)
    {
        int newNutrients = GetNutrients() + addedNutrients;
        PlayerPrefs.SetInt("currentNutrients", newNutrients);
        hudNutrients.UpdateNutrientsUI();
    }

    public void SubtractNutrients(int subtractedNutrients)
    {
        int newNutrients = GetNutrients() - subtractedNutrients;
        PlayerPrefs.SetInt("currentNutrients", newNutrients);
        hudNutrients.UpdateNutrientsUI();
    }

    public int GetNutrients()
    {
        return PlayerPrefs.GetInt("currentNutrients");
    }
}
