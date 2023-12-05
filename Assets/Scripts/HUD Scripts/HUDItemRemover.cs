using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDItemRemover : MonoBehaviour
{
    private HUDItem hudItem;

    void Start()
    {
        hudItem = GameObject.Find("HUD").GetComponent<HUDItem>();
        hudItem.LostItem();
    }
}
