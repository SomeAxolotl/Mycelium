using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NutrientTracker : MonoBehaviour
{
    [Header("Current Nutrients")]
    public int currentNutrients;
    // Start is called before the first frame update
    void Start()
    {

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
    }
    public void AddNutrients(int amount)
    {
        currentNutrients += amount;
    }
}
