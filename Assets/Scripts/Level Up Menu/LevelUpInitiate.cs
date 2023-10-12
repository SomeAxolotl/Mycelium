using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelUpInitiate : MonoBehaviour
{
    [SerializeField]
    GameObject levelupmenu;
    public Button firstbutton;

    private PlayerController playerController;
    private MeleeAttack meleeAttack;


    private void OnCollisionEnter(Collision other)
    {
        playerController = GameObject.FindWithTag("currentPlayer").GetComponent<PlayerController>();
        meleeAttack = GameObject.FindWithTag("currentPlayer").GetComponent<MeleeAttack>();

        if (other.gameObject.CompareTag("currentPlayer"))
        {
            levelupmenu.SetActive(true);
            playerController.DisableController();
            meleeAttack.DisableAttack();
            firstbutton.Select();
        }
    }
}
