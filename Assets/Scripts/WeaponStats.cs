using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    //Set these values in the inspector
    public float weaponDmg;
    public float weaponAtkCooldown;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.tag == "Weapon")
        {
            transform.Rotate(0, 75 * Time.deltaTime, 0);
        }
    }
}
