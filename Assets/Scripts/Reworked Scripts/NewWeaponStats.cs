using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewWeaponStats : MonoBehaviour
{
    [Header("Weapon Stats")]
    public float wpnCooldown;
    public float wpnDamage;
    public 
    

    // Start is called before the first frame update
    void Start()
    {
        if (this.gameObject.name == "StartWeapon")
        {
            wpnCooldown = 2f;
            wpnDamage = 0f;
        }

        if (this.gameObject.name == "WoodSmash")
        {
            wpnCooldown = Random.Range(1.8f, 1.9f);
            wpnDamage = Random.Range(25, 30);
        }
        
        if (this.gameObject.name == "WoodSlash")
        {
            wpnCooldown = Random.Range(1.5f, 1.6f);
            wpnDamage = Random.Range(15, 20);
        }
        
        if (this.gameObject.name == "WoodStab")
        {
            wpnCooldown = Random.Range(1.3f, 1.4f);
            wpnDamage = Random.Range(5, 10);
        }

        if (this.gameObject.name == "StoneSmash")
        {
            wpnCooldown = Random.Range(1.6f, 1.7f);
            wpnDamage = Random.Range(35, 40);
        }
        
        if (this.gameObject.name == "StoneSlash")
        {
            wpnCooldown = Random.Range(1.3f, 1.4f);
            wpnDamage = Random.Range(25, 30);
        }
        
        if (this.gameObject.name == "StoneStab")
        {
            wpnCooldown = Random.Range(1.1f, 1.2f);
            wpnDamage = Random.Range(15, 20);
        }

        if (this.gameObject.name == "BoneSmash")
        {
            wpnCooldown = Random.Range(1.4f, 1.5f);
            wpnDamage = Random.Range(45, 50);
        }
        
        if (this.gameObject.name == "BoneSlash" )
        {
            wpnCooldown = Random.Range(1.1f, 1.2f);
            wpnDamage = Random.Range(35, 40);
        }
        
        if (this.gameObject.name == "BoneStab")
        {
            wpnCooldown = Random.Range(0.9f, 1.0f);
            wpnDamage = Random.Range(25, 30);
        }

        if (gameObject.tag == "Weapon")
        {
            transform.rotation = Quaternion.Euler(45, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == "Weapon")
        {
            transform.Rotate(0, 0, 75 * Time.deltaTime);
        }
    }
}
