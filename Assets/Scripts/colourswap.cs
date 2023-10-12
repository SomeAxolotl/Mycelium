using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colourswap : MonoBehaviour
{
   public bool play2;
   public GameObject Purple;
   public GameObject green;
   public bool unlock;

    // Update is called once per frame
    void Update()
    {
        if (play2 == true)
        {
            Purple.SetActive(false);
            green.SetActive(true);
        }
        if (play2 == false)
        {
            Purple.SetActive(true);
            green.SetActive(false);
        }
    }
}
