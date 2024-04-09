using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StartBoss : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    [SerializeField] private TempMovement tempMovement;

    void Start()
    {
        boss.SetActive(false);
        tempMovement.enabled = false;
        StartCoroutine(ReEnableStuff());
    }

    IEnumerator ReEnableStuff()
    {
        yield return new WaitForSeconds(35f);
        GameObject boss = GameObject.Find("Rival Colony Leader");
        boss.GetComponent<MonsterBossAttack>().enabled = true;
        tempMovement.enabled = true;
        GameObject player = GameObject.FindWithTag("PlayerParent");
        player.GetComponent<PlayerController>().playerActionsAsset.Player.Enable();
        player.GetComponent<PlayerAttack>().playerActionsAsset.Player.Enable();
        GameObject colliderObj = GameObject.Find("StartAnimBoss");
        colliderObj.SetActive(false);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("currentPlayer"))
        {
            boss.SetActive(true);
            boss.GetComponent<MonsterBossAttack>().enabled = false;
            GameObject player = GameObject.FindWithTag("PlayerParent");
            player.GetComponent<PlayerController>().playerActionsAsset.Player.Disable();
            player.GetComponent<PlayerAttack>().playerActionsAsset.Player.Disable();

            //Ryan's Camera Stuff
            GameObject.Find("Cutscene VCams").GetComponent<BossCam>().StartBossCutscene();
        }
    }
}
