using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleDisabler : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return null;

        if (!FurnitureManager.Instance.FurnitureIsUnlocked("FireflyBottle"))
        {
            gameObject.SetActive(false);
        }
    }
}
