using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using RonaldSunglassesEmoji.Interaction;

public class LevelUpInitiate : MonoBehaviour, IInteractable
{
    [SerializeField]
    GameObject levelupmenu;
    public GameObject HUD;
    private HUDController hudController;
    public Button firstbutton;

    public PlayerController playerController;
    //private New Player Attack meleeAttack;

    [SerializeField] float verticalOffset = 0f;

    void Start()
    {
        hudController = GameObject.Find("HUD").GetComponent<HUDController>();
        playerController = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>();
        //HUDCanvasGroup = HUD.GetComponent<CanvasGroup>();
    }

    public void Interact(GameObject interactObject)
    {
        if (SceneLoader.Instance.isLoading == false)
        {
            playerController = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>();
            playerController.DisableController();
            levelupmenu.SetActive(true);
            
            hudController.FadeOutHUD();

            //This helps fix the bug where you could pause in the shop
            GlobalData.isAbleToPause = false;
        }
    }

    public void Salvage(GameObject interactObject)
    {
        //buh
    }

    public void CreateTooltip(GameObject interactObject)
    {
        string buttonText = InputManager.Instance.GetLatestController().interactHint.GenerateColoredHintString();
        TooltipManager.Instance.CreateTooltip
        (
            gameObject, 
            "Sporemother", 
            "Upgrade, equip, and grow new Spores!", 
            "Press " + buttonText + " to Interact", 
            "", 
            false, 
            verticalOffset
        );
    }

    public void DestroyTooltip(GameObject interactObject, bool isFromInteracting = false)
    {
        TooltipManager.Instance.DestroyTooltip();
    }
}
