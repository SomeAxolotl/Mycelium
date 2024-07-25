using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mycotoxins : Skill
{
    private PlayerController controller;
    private float timer = 0f;
    [SerializeField] private float speedBoost = 50f;
    [SerializeField] private float speedDuration = 3f;
    [SerializeField] private int spawnCount = 0;
    private float originalMoveSpeed;
    public GameObject sporeConePrefab;
    //Skill specific fields

    public override void DoSkill()
    {
        ParticleManager.Instance.SpawnParticles("MycoToxinsParticles", player.transform.position, Quaternion.identity, player);
        spawnCount = 0;
        controller = GetComponentInParent<PlayerController>();
        if (controller != null)
        {
            SpeedChange speedChange = controller.gameObject.AddComponent<SpeedChange>();
            speedChange.InitializeSpeedChange(3f, speedBoost);

            RefreshTimer();
            StartCoroutine(ReleaseSpores());

            timer = 0f;
        }

        //Skill specific stuff

        EndSkill();
    }
    private void Update()
    {
        // Increment the timer in each frame
        timer += Time.deltaTime;
    }
    public LayerMask groundLayerMask;
    private IEnumerator ReleaseSpores()
    {
        float basePitch = 0.5f;
        float pitchIncrement = 0.2f;
        while (spawnCount < 3)
        {
            // Spawn a spore cone
            GameObject sporeCone = Instantiate(sporeConePrefab, transform.position - transform.forward * 2f, Quaternion.identity);

            float pitchMultiplier = basePitch + pitchIncrement * spawnCount;
            SoundEffectManager.Instance.PlaySound("Projectile", player.transform.position, volumeModifier: 0.5f, pitchMultiplier: pitchMultiplier);

            sporeCone.GetComponent<DamageArea>().finalDamageValue = finalSkillValue;
            spawnCount++;
            if (sporeCone != null)
            {
                Debug.Log("Spore damage cone spawned");
            }
            if (sporeCone == null)
            {
                Debug.Log("Toxic Failure");
            }
            Destroy(sporeCone, 3f);

            yield return new WaitForSeconds(1f); // Wait for 1 second before spawning the next cone
        }
        ActualCooldownStart();
    }

    //Code to have an active duration that goes down before real cooldown starts
    float savedCooldown;
    public override void StartCooldown(float skillCooldown){
        savedCooldown = skillCooldown;
        //Does not do cooldown normally
        canSkill = false;
    }

    protected override void ActualCooldownStart(){
        hudSkills.ToggleActiveBorder(skillSlot, false);
        
        if(cooldownCoroutine != null){
            StopCoroutine(cooldownCoroutine);
        }
        if(hudCooldownCoroutine != null){
            hudSkills.StopHUDCoroutine(hudCooldownCoroutine);
        }

        cooldownCoroutine = StartCoroutine(Cooldown(savedCooldown));
    }

    private void RefreshTimer(){
        hudSkills.ToggleActiveBorder(skillSlot, true);
        if(hudCooldownCoroutine != null){
            hudSkills.StopHUDEffectCoroutine(hudCooldownCoroutine);
        }
        hudCooldownCoroutine = hudSkills.StartEffectUI(skillSlot, speedDuration);
    }
}

