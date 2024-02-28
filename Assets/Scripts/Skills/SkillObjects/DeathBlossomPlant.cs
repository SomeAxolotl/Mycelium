using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using Unity.VisualScripting;
using UnityEngine;

public class DeathBlossomPlant : DeathBlossom
{
    [SerializeField] private float destroyTime = 2f;
    [SerializeField] private float burstRadius = 10f;
    [SerializeField] private float damageOverTimeDuration = 7f;
    [SerializeField] private int particleSpacing = 36;
    [SerializeField] private float particleHeight = 0f;
    [SerializeField] private Renderer render;
    [SerializeField] private Light light;
    private Color StartGlow;
    private float startLightIntensity;
    void Start()
    {
        StartCoroutine(ExplodeAfterDelay());
        StartGlow = render.material.GetColor("_Glow_Color");
        if(light!=null){
            startLightIntensity = light.intensity;
            light.intensity = 0;
        }
        render.material.SetColor("_Glow_Color", new Color(0,0,0));
        StartCoroutine(Illuminate());
    }
    IEnumerator Illuminate(){
        float t = 0f;
        Color currentGlow;
        float currentModifier = 0;
        while (t < destroyTime) 
        {   
            currentModifier = (t/destroyTime);
            currentGlow = new Color(StartGlow.r*currentModifier, StartGlow.g*currentModifier, StartGlow.b*currentModifier);
            render.material.SetColor("_Glow_Color", currentGlow);
            if(light!=null)
                light.intensity = Mathf.Lerp(0,startLightIntensity,t);
            t += Time.deltaTime;
            yield return null;
        }
    }
    private IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(destroyTime);
        DamageEnemies();
        DeathBlossomParticles();
        if (damageOverTimeDuration > 0)
        {
            gameObject.GetComponentInChildren<Renderer>().enabled = false;
            Destroy(light);
        }
    }
    //DestoryAfterTime culls the gameobject after it is finished dealing damage over time.
    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(damageOverTimeDuration);
        Destroy(this);
    }
    private IEnumerator ApplyDamageOverTime(EnemyHealth enemyHealth, float damage)
    {
        Debug.Log("ApplyDamageOverTime started");
        float timeElapsed = 0f;
        float damageInterval = damage / damageOverTimeDuration;

        while (timeElapsed < damageOverTimeDuration)
        {
            Debug.Log("Applying damage: " + damageInterval);
            enemyHealth.EnemyTakeDamage(damageInterval);
            yield return new WaitForSeconds(1f);
            timeElapsed++;
        }
    }
    void DamageEnemies()
    {
        SoundEffectManager.Instance.PlaySound("Explosion", transform.position);

        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");

        Collider[] colliders = Physics.OverlapSphere(transform.position, burstRadius, enemyLayerMask);

        float damage = finalSkillValue;
        Debug.Log("FinalSkillValue: " + damage);
        foreach (Collider collider in colliders)
        {
            EnemyHealth enemyHealth = collider.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                StartCoroutine(ApplyDamageOverTime(enemyHealth, damage));
                Debug.Log("Death Blossom hit!");
            }
        }
        StartCoroutine(DestroyAfterTime());
    }

    void DeathBlossomParticles()
    {
        int particlesPerCircle = 360 / particleSpacing;

        int currentSmallSpacing = 0;
        for (int i = 0; i < particlesPerCircle; i++)
        {
            float smallX = Mathf.Cos(Mathf.Deg2Rad * currentSmallSpacing) * burstRadius;
            float smallZ = Mathf.Sin(Mathf.Deg2Rad * currentSmallSpacing) * burstRadius;

            InstantiateParticles(smallX, smallZ);
            currentSmallSpacing += particleSpacing;
        }
    }
    void InstantiateParticles(float x, float z)
    {
        Vector3 circlePosition = new Vector3(x, particleHeight, z);
        Vector3 spawnPosition = transform.position + circlePosition;
        ParticleManager.Instance.SpawnParticles("EruptionParticles", spawnPosition, Quaternion.LookRotation(Vector3.up, Vector3.up));
    }
}


