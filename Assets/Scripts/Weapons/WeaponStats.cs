using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    [SerializeField] public string wpnName = "Wooden Sword";

    [SerializeField] float wpnDamageMin = 10f;
    [SerializeField] float wpnDamageMax = 50f;
    public float wpnDamage {get; private set;}

    [SerializeField] float wpnKnockbackMin = 10f;
    [SerializeField] float wpnKnockbackMax = 50f;
    public float wpnKnockback {get; private set;}

    float rotationSpeed = 30f;
    float tiltAngle = 45f;

    private Quaternion initialRotation;

    void Start()
    {
        wpnDamage = Mathf.RoundToInt(Random.Range(wpnDamageMin, wpnDamageMax));
        wpnKnockback = Random.Range(wpnKnockbackMin, wpnKnockbackMax);
        initialRotation = Quaternion.Euler(0f, 0f, tiltAngle);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == "Weapon")
        {
            transform.parent = null;

            Quaternion targetRotation = Quaternion.Euler(0f, rotationSpeed * Time.deltaTime, 0f) * transform.rotation;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, tiltAngle);
        }
    }
}
