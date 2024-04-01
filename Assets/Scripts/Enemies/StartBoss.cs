using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StartBoss : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    void Start()
    {
        boss.SetActive(false);
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
        }
    }
}
