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

    public PlayerController playerController;
    //private New Player Attack meleeAttack;

    void Start()
    {
        HUDCanvasGroup = GameObject.Find("HUD").GetComponent<CanvasGroup>();
         playerController = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>();
        //HUDCanvasGroup = HUD.GetComponent<CanvasGroup>();
    }

    private void OnCollisionEnter(Collision other)
    {
        //playerController = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>();
        //meleeAttack = GameObject.FindWithTag("PlayerParent").GetComponent<MeleeAttack>();

        if (other.gameObject.tag == "currentPlayer" && SceneLoader.Instance.isLoading == false)
        {
            playerController = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>();
            playerController.DisableController();
            levelupmenu.SetActive(true);
            
            HUDCanvasGroup.alpha = 0;

            //This helps fix the bug where you could pause in the shop
            GlobalData.isAbleToPause = false;
        }
    }
}
