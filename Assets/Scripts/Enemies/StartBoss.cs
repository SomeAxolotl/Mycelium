using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StartBoss : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    private GameObject playerParent;
    void Start()
    {
        boss.GetComponent<TempMovement>().enabled = false;
        boss.GetComponent<MonsterBossAttack>().enabled = false;
        boss.SetActive(false);
        playerParent = GameObject.FindWithTag("PlayerParent");
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("currentPlayer"))
        {
            boss.SetActive(true);
            playerParent.GetComponent<PlayerController>().playerActionsAsset.Player.Disable();
            playerParent.GetComponent<PlayerAttack>().playerActionsAsset.Player.Disable();

            //Ryan's Camera Stuff
            GameObject.Find("Cutscene VCams").GetComponent<BossCam>().StartBossCutscene();

            //Ronald's Music Stuff :o
            GameObject.Find("BackgroundMusicPlayer").GetComponent<AudioSource>().Play();

            this.gameObject.SetActive(false);
        }
    }
}
