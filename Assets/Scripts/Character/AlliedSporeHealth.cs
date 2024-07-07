using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlliedSporeHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public float dmgTaken;
    protected bool hasTakenDamage = false;
    [HideInInspector] public bool alreadyDead = false;
    public Action<float> TakeDamage;
    public Transform centerPoint;
    private Animator animator;
    private AlliedSporeHealthBar healthBar;
    private CharacterStats characterStats;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        healthBar = GetComponentInChildren<AlliedSporeHealthBar>();
        characterStats = GetComponent<CharacterStats>();
        maxHealth = characterStats.baseHealth;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
