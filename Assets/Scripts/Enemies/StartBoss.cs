using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class StartBoss : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    private GameObject playerParent;
    private Animator bossAnimator;
    private float defaultAnimatorSpeed;
    void Start()
    {
        bossAnimator = boss.GetComponent<Animator>();

        boss.GetComponent<TempMovement>().enabled = false;
        boss.GetComponent<MonsterBossAttack>().enabled = false;

        defaultAnimatorSpeed = bossAnimator.speed;
        bossAnimator.speed = 0f;

        playerParent = GameObject.FindWithTag("PlayerParent");
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("currentPlayer"))
        {
            boss.SetActive(true);
            bossAnimator.speed = defaultAnimatorSpeed;

            playerParent.GetComponent<PlayerController>().playerActionsAsset.Player.Disable();
            playerParent.GetComponent<PlayerAttack>().playerActionsAsset.Player.Disable();

            //Ryan's Camera Stuff
            GameObject.Find("Cutscene VCams").GetComponent<BossCam>().StartBossIntroCutscene();

            this.gameObject.SetActive(false);
        }
    }

    public void PauseAnimator()
    {
        if(Application.isPlaying == true)
        {
            bossAnimator.speed = 0f;
        }
    }

    public void UnpauseAnimator()
    {
        if (Application.isPlaying == true)
        {
            bossAnimator.speed = 1f;
        }
    }
}

//RYAN'S FANCY BUTTON STUFF
#if UNITY_EDITOR
[CustomEditor(typeof(StartBoss))]
class StartBossEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //Setup Stuff
        base.OnInspectorGUI();

        var startBoss = (StartBoss)target;
        if (startBoss == null) return;

        //Actual Stuff
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Pause Animator"))
        {
            startBoss.PauseAnimator();
        }

        if (GUILayout.Button("Unpause Animator"))
        {
            startBoss.UnpauseAnimator();
        }

        GUILayout.EndHorizontal();
    }
}
#endif
