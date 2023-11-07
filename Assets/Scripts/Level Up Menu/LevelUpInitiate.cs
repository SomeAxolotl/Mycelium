using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelUpInitiate : MonoBehaviour
{
    [SerializeField]
    GameObject levelupmenu;
    public GameObject HUD;
    private CanvasGroup HUDCanvasGroup;
    public Button firstbutton;

    private PlayerController playerController;
    //private New Player Attack meleeAttack;

    void Start()
    {
        HUDCanvasGroup = GameObject.Find("HUD").GetComponent<CanvasGroup>();
        //HUDCanvasGroup = HUD.GetComponent<CanvasGroup>();
    }

    private void OnCollisionEnter(Collision other)
    {
        playerController = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>();
        //meleeAttack = GameObject.FindWithTag("PlayerParent").GetComponent<MeleeAttack>();

        if (other.gameObject.tag == "currentPlayer")
        {
            levelupmenu.SetActive(true);
            playerController.DisableController();
            HUDCanvasGroup.alpha = 0;
        }
    }
}
