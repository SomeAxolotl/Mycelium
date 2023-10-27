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
    //private New Player Attack meleeAttack;


    private void OnCollisionEnter(Collision other)
    {
        playerController = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>();
        //meleeAttack = GameObject.FindWithTag("PlayerParent").GetComponent<MeleeAttack>();

        if (other.gameObject.CompareTag("currentPlayer"))
        {
            levelupmenu.SetActive(true);
            playerController.DisableController();
            
        }
    }
}
