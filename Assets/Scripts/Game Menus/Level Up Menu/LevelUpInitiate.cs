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
    private CanvasGroup HUDCanvasGroup;
    public Button firstbutton;

    public PlayerController playerController;
    //private New Player Attack meleeAttack;

    [SerializeField] float verticalOffset = 0f;

    void Start()
    {
        HUDCanvasGroup = GameObject.Find("HUD").GetComponent<CanvasGroup>();
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
            
            HUDCanvasGroup.alpha = 0;

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
        string buttonText = "<color=#3cdb4e>A</color>";
        TooltipManager.Instance.CreateTooltip(gameObject, "Sporemother", "Upgrade, equip, and grow new Spores!", "Press " + buttonText + " to Interact", false, verticalOffset);
    }

    public void DestroyTooltip(GameObject interactObject)
    {
        TooltipManager.Instance.DestroyTooltip();
    }
}
