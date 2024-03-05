using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDFader : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "currentPlayer")
        {
            GameObject.Find("HUD").GetComponent<HUDController>().FadeInHUD();
        }
    }
}
