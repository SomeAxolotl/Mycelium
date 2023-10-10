using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelUpInitiate : MonoBehaviour
{
    [SerializeField]
    GameObject levelupmenu;

    private PlayerController playerController;

    private void OnCollisionEnter(Collision other)
    {
        playerController = GameObject.FindWithTag("currentPlayer").GetComponent<PlayerController>();

        if(other.gameObject.CompareTag("currentPlayer"))
        {
            levelupmenu.SetActive(true);
            playerController.DisableController();
        }
    }
}
