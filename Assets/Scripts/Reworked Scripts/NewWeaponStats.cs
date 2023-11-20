using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewWeaponStats : MonoBehaviour
{
    [Header("Weapon Stats")]
    public float wpnCooldown;
    public float wpnDamage;
    public float knockback;
    // Start is called before the first frame update
    void Start()
    {
        if (this.gameObject.name == "StartWeapon(Clone)")
        {
            wpnCooldown = 2f;
            wpnDamage = 0f;
            knockback = 5f;
        }

        if (this.gameObject.name == "WoodSmash(Clone)")
        {
            wpnCooldown = Random.Range(1.8f, 1.9f);
            wpnDamage = Random.Range(25, 30);
            knockback = 7f;
        }

        if (this.gameObject.name == "WoodSlash(Clone)")
        {
            wpnCooldown = Random.Range(1.5f, 1.6f);
            wpnDamage = Random.Range(15, 20);
            knockback = 5f;
        }

        if (this.gameObject.name == "WoodStab(Clone)")
        {
            wpnCooldown = Random.Range(1.3f, 1.4f);
            wpnDamage = Random.Range(5, 10);
            knockback = 3f;
        }

        if (this.gameObject.name == "StoneSmash(Clone)")
        {
            wpnCooldown = Random.Range(1.6f, 1.7f);
            wpnDamage = Random.Range(35, 40);
            knockback = 7f;
        }

        if (this.gameObject.name == "StoneSlash(Clone)")
        {
            wpnCooldown = Random.Range(1.3f, 1.4f);
            wpnDamage = Random.Range(25, 30);
            knockback = 5f;
        }

        if (this.gameObject.name == "StoneStab(Clone)")
        {
            wpnCooldown = Random.Range(1.1f, 1.2f);
            wpnDamage = Random.Range(15, 20); 
            knockback = 3f;
        }

        if (this.gameObject.name == "BoneSmash(Clone)")
        {
            wpnCooldown = Random.Range(1.4f, 1.5f);
            wpnDamage = Random.Range(45, 50);
            knockback = 7f;
        }

        if (this.gameObject.name == "BoneSlash(Clone)")
        {
            wpnCooldown = Random.Range(1.1f, 1.2f);
            wpnDamage = Random.Range(35, 40);
            knockback = 5f;
        }

        if (this.gameObject.name == "BoneStab(Clone)")
        {
            wpnCooldown = Random.Range(0.9f, 1.0f);
            wpnDamage = Random.Range(25, 30);
            knockback = 3f;
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
            transform.parent = null;
        }
    }
}
