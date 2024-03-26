using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mycotoxins : Skill
{
    private PlayerController controller;
    private float timer = 0f;
    [SerializeField] private float speedBoost = 1.5f;
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
            originalMoveSpeed = controller.moveSpeed;
            controller.moveSpeed *= speedBoost;

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

        if (controller != null && timer >= speedDuration)
        {
            controller.moveSpeed = originalMoveSpeed;
            // End the skill when duration is over

        }
    }
    private IEnumerator ReleaseSpores()
    {
        while (spawnCount < 3)
        {
            // Spawn a spore cone
            GameObject sporeCone = Instantiate(sporeConePrefab, transform.position - transform.forward * 2f, Quaternion.identity);
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
    }
}

