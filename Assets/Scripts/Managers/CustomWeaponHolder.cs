using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomWeaponHolder : MonoBehaviour
{
    public static CustomWeaponHolder Instance;

    public GameObject stickbugSpear;

    void Awake(){
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
    }

}
