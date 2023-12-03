using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewWeaponStats : MonoBehaviour
{
    [SerializeField] public string wpnName = "Wooden Sword";

    [SerializeField] float wpnDamageMin = 10f;
    [SerializeField] float wpnDamageMax = 50f;
    public float wpnDamage {get; private set;}

    [SerializeField] float wpnKnockbackMin = 10f;
    [SerializeField] float wpnKnockbackMax = 50f;
    public float wpnKnockback {get; private set;}

    void Start()
    {
        wpnDamage = Random.Range(wpnDamageMin, wpnDamageMax);
        wpnKnockback = Random.Range(wpnKnockbackMin, wpnKnockbackMax);
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
