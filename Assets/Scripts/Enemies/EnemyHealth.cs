using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using System;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

//using UnityEditor.Rendering.Universal.ShaderGUI;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public int nutrientDrop;
    Rigidbody rb;
    protected List<BaseEnemyHealthBar> enemyHealthBars = new List<BaseEnemyHealthBar>();
    public Transform centerPoint;
    protected bool hasTakenDamage = false;
    [HideInInspector] public bool alreadyDead = false;
    Animator animator;

    private ProfileManager profileManagerScript;

    public Action<float> TakeDamage;
    public float dmgTaken;

    [HideInInspector][SerializeField] public bool isMiniBoss = false;
    [HideInInspector] public string miniBossName = "";
    [HideInInspector][SerializeField] List<GameObject> possibleRewards = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth * GlobalData.currentLoop;
        maxHealth = maxHealth * GlobalData.currentLoop;
        this.transform.parent = null;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        foreach (BaseEnemyHealthBar enemyHealthBar in GetComponentsInChildren<BaseEnemyHealthBar>())
        {
            enemyHealthBars.Add(enemyHealthBar);
        }

        profileManagerScript = GameObject.Find("ProfileManager").GetComponent<ProfileManager>();
    }

    public virtual void EnemyTakeDamage(float damage)
    {
        //Save current damage taken
        dmgTaken = damage;
        //Call action to modify damage
        TakeDamage?.Invoke(dmgTaken);

        currentHealth -= dmgTaken;

        foreach (BaseEnemyHealthBar enemyHealthBar in enemyHealthBars)
        {
            if(enemyHealthBar != null && currentHealth + dmgTaken > 0) 
            {
                enemyHealthBar.UpdateEnemyHealthUI();
                enemyHealthBar.DamageNumber(dmgTaken);
                ParticleManager.Instance.SpawnParticles("Blood", centerPoint.position, Quaternion.identity);
            }
        }
        if(currentHealth <= 0 && !alreadyDead)
        {
            StartCoroutine(Death());
        }
        hasTakenDamage = true;
    }
    public Action Died;
    protected IEnumerator Death()
    {
        Died?.Invoke();
        
        gameObject.GetComponent<EnemyAttack>().CancelAttack();
        gameObject.GetComponent<EnemyAttack>().enabled = false;
        gameObject.GetComponent<ReworkedEnemyNavigation>().enabled = false;
        gameObject.layer = LayerMask.NameToLayer("DeadEnemy");

        alreadyDead = true;

        animator.Rebind();
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(1.25f);
        float elapsedTime = 0f;
        float shrinkDuration = 1f;
        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = Vector3.zero;
        while (elapsedTime < shrinkDuration)
        {

            elapsedTime += Time.deltaTime;
            float t = elapsedTime / shrinkDuration;
            transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            yield return null;
        }
        if (GlobalData.currentLoop >= 2)
        {
            nutrientDrop = (nutrientDrop * (GlobalData.currentLoop / 2));
        }
        ParticleManager.Instance.SpawnParticleFlurry("NutrientParticles", nutrientDrop, 0.1f, this.gameObject.transform.position, Quaternion.Euler(-90f, 0f, 0f));
        if (gameObject.name == "Giga Beetle")
        {
            profileManagerScript.tutorialIsDone[GlobalData.profileNumber] = true;
            GameObject.Find("SceneLoader").GetComponent<SceneLoader>().BeginLoadScene("The Carcass", false);
        }

        if (isMiniBoss)
        {
            SpawnMinibossReward();
        }

        this.gameObject.SetActive(false);
    }

    void SpawnMinibossReward()
    {
        if (possibleRewards.Count > 0)
        {
            int randomRewardIndex = UnityEngine.Random.Range(0, possibleRewards.Count);

            Instantiate(possibleRewards[randomRewardIndex], new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z), Quaternion.identity);
        }
        else
        {
            Debug.LogError(gameObject + " is a miniBoss has no possibleRewards set");
        }
    }

    protected IEnumerator BossDeath()
    {
        if(GameObject.Find("Rival Colony Leader") != null)
        {
            BossCam.Instance.StartBossDeathCutscene();

            GameObject boss = GameObject.Find("Rival Colony Leader");

            Collider[] bossCollider = boss.GetComponents<Collider>();
            foreach (Collider collider in bossCollider)
            {
                collider.enabled = false;
            }
            Collider[] childColliders = boss.GetComponentsInChildren<Collider>();
            foreach (Collider collider in childColliders)
            {
                collider.enabled = false;
            }

            boss.GetComponent<MonsterBossAttack>().enabled = false;
            boss.GetComponent<TempMovement>().enabled = false;
            boss.GetComponent<Animator>().SetTrigger("Death");

            GameObject player = GameObject.FindWithTag("PlayerParent");
            player.GetComponent<PlayerController>().playerActionsAsset.Player.Disable();
            player.GetComponent<PlayerAttack>().playerActionsAsset.Player.Disable();
            yield return null;
        }
    }

    public bool HasTakenDamage()
    {
        return hasTakenDamage;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(EnemyHealth))]
class EnemyHealthEditor : Editor
{
    SerializedProperty isMiniBoss;
    SerializedProperty miniBossName;
    SerializedProperty possibleRewards;

    void OnEnable()
    {
        isMiniBoss = serializedObject.FindProperty("isMiniBoss");
        miniBossName = serializedObject.FindProperty("miniBossName");
        possibleRewards = serializedObject.FindProperty("possibleRewards");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        base.OnInspectorGUI();

        EditorGUILayout.PropertyField(isMiniBoss);

        EditorGUI.indentLevel++;
        if (isMiniBoss.boolValue)
        {
            EditorGUILayout.PropertyField(miniBossName);
            EditorGUILayout.PropertyField(possibleRewards);
        }
        EditorGUI.indentLevel--;

        EnemyHealth enemyHealth = (EnemyHealth)target;
        Transform thisTransform = enemyHealth.transform;
        EnemyHealthBar enemyHealthBar = thisTransform.GetComponentInChildren<EnemyHealthBar>();
        if (enemyHealthBar != null)
        {
            enemyHealthBar.enemyHealthName.text = miniBossName.stringValue;
            EditorUtility.SetDirty(enemyHealthBar.enemyHealthName);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
