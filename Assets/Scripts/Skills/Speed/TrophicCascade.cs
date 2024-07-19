using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TrophicCascade : Skill
{
    [SerializeField] private float vanishDuration = 1.4f;
    private float newVanishDuration = 0.5f;
    [SerializeField] private float cascadeRadius = 3f;
    [SerializeField] private float cameraBlendTimeScalar = 0.75f;

    private string trophicParticlePath = "Effects/TrophicParticles";
    private GameObject trophicPrefab;
    private string explodeParticlePath = "Effects/TrophicExplosionParticles";
    private GameObject explodePrefab;

    private GameObject trophicVisuals;
    private TrailRenderer trophicTrail;
    private ParticleSystemRenderer trophicParticles;

    public override void DoSkill(){
        trophicPrefab = Resources.Load<GameObject>(trophicParticlePath);
        explodePrefab = Resources.Load<GameObject>(explodeParticlePath);
        StartCoroutine(Vanish());
    }

    IEnumerator Vanish(){
        Renderer[] childRenderers = player.GetComponentsInChildren<Renderer>();
        ParticleManager.Instance.SpawnParticles("TrophicCascadePoof", player.transform.position, Quaternion.Euler(-90,0,0));

        if(trophicPrefab != null){
            trophicVisuals = Instantiate(trophicPrefab, new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z), Quaternion.Euler(0,0,0)) as GameObject;
            Debug.Log(trophicVisuals.transform.position);
        }else{
            Debug.Log("Why are trophic particles not loaded?");
        }

        if (isPlayerCurrentPlayer())
        {
            playerController.isInvincible = true;
        }
        
        foreach (Renderer renderer in childRenderers)
        {
            renderer.enabled = false;
        }

        yield return StartCoroutine(Cascade());

        if(trophicVisuals != null){
            Destroy(trophicVisuals, 3);
        }

        if (isPlayerCurrentPlayer())
        {
            playerController.isInvincible = false;
        }
        foreach (Renderer renderer in childRenderers)
        {
            if (!(renderer is ParticleSystemRenderer))
            {
                renderer.enabled = true;
            }
        }
    }

    IEnumerator Cascade()
    {
        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");
        Collider[] colliders = Physics.OverlapSphere(player.transform.position, cascadeRadius, enemyLayerMask);
        //Stops the player from being able to be pushed while gone, I could turn off the collider but that could cause problems when spawning back into an enemy
        if(player.GetComponent<Rigidbody>() != null){
            player.GetComponent<Rigidbody>().isKinematic = true;
        }

        yield return new WaitForSeconds(vanishDuration / 4f);

        List<GameObject> enemies = new List<GameObject>();
        foreach(Collider collider in colliders)
        {
            if(!enemies.Contains(collider.gameObject) && collider.gameObject.GetComponent<EnemyHealth>() != null){
                //Only adds it if they have health
                if(collider.gameObject.GetComponent<EnemyHealth>().currentHealth > 0){
                    enemies.Add(collider.gameObject);
                }
            }
        }

        newVanishDuration = Mathf.Clamp(enemies.Count * 0.5f, 0.4f, vanishDuration);
        float cascadeDuration = newVanishDuration / 2f;


        VCamRotator vcamRotator = GameObject.Find("VCamHolder").GetComponent<VCamRotator>();

        for (int i = 0; i < enemies.Count; i++)
        {
            Mark(enemies[i]);
            //trophicVisuals.transform.position = new Vector3(enemies[i].transform.position.x, enemies[i].transform.position.y + 1, enemies[i].transform.position.z);
            //Debug.Log(trophicVisuals.transform.position);
            StartCoroutine(TrailMovement(new Vector3(enemies[i].transform.position.x, enemies[i].transform.position.y + 1, enemies[i].transform.position.z), cascadeDuration / enemies.Count));

            float blendTime = Mathf.Clamp(cascadeDuration / enemies.Count * cameraBlendTimeScalar, 0, cascadeDuration * cameraBlendTimeScalar * 0.5f);
            //vcamRotator.DramaticCamera(enemies[i].transform, blendTime);
            yield return new WaitForSeconds(cascadeDuration / enemies.Count);
        }
        //Little bit faster than the final extinguish
        StartCoroutine(TrailMovement(new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z), newVanishDuration / 6f));
        yield return new WaitForSeconds(newVanishDuration / 4f);
        ParticleManager.Instance.SpawnParticles("TrophicCascadePoof", player.transform.position, Quaternion.Euler(-90,0,0));
        if(player.GetComponent<Rigidbody>() != null){
            player.GetComponent<Rigidbody>().isKinematic = false;
        }
        //omae wa mou shindeiru
        Extinguish(enemies);
    }

    IEnumerator TrailMovement(Vector3 targetPosition, float timeToFinish){
        timeToFinish *= 0.25f;
        Vector3 startPosition = trophicVisuals.transform.position;
        float elapsedTime = 0f;
        while(elapsedTime < timeToFinish){
            trophicVisuals.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / timeToFinish);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        trophicVisuals.transform.position = targetPosition;
    }

    void Mark(GameObject enemy)
    {
        ParticleManager.Instance.SpawnParticles("TrophicCascadePoof", enemy.transform.position, Quaternion.identity);
        SoundEffectManager.Instance.PlaySound("Projectile", enemy.transform);
    }

    void Extinguish(List<GameObject> enemies)
    {
        float damagePerEnemy = finalSkillValue;// / enemies.Count;
        foreach (GameObject enemy in enemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            enemyHealth.EnemyTakeDamage(damagePerEnemy);
            if(explodePrefab != null){
                GameObject boomVisuals = Instantiate(explodePrefab, new Vector3(enemy.transform.position.x, enemy.transform.position.y + 1, enemy.transform.position.z), Quaternion.Euler(0,0,0)) as GameObject;
            }
        }

        if (enemies.Count > 0)
        {
            SoundEffectManager.Instance.PlaySound("Impact", player.transform);
        }

        EndSkill();
    }
}
