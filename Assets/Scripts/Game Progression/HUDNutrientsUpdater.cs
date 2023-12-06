using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDNutrientsUpdater : MonoBehaviour
{

    void Start()
    {
        StartCoroutine(UpdateNutrients());
    }

    IEnumerator UpdateNutrients()
    {
        yield return null;
        GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>().AddNutrients(0);
    }
}
