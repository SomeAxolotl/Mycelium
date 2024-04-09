using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class CharSelectManagerNew : MonoBehaviour
{
    public SwapCharacter swapCharacter;
    public Button startButton;
    public GameObject UIEnable;
    public GameObject HUD;
    ThirdPersonActionsAsset controls;
    

    public PlayerController playerController;

    private SporeManager sporeManagerScript;
    private ProfileManager profileManagerScript;

    private void Awake()
    {
        controls = new ThirdPersonActionsAsset();
    }

    void Start()
    {
        swapCharacter = GameObject.FindWithTag("PlayerParent").GetComponent<SwapCharacter>();
        sporeManagerScript = GameObject.Find("SporeManager").GetComponent<SporeManager>();
        profileManagerScript = GameObject.Find("ProfileManager").GetComponent<ProfileManager>();
    }

    void OnEnable()
    {
        playerController = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>();
        playerController.DisableController();
        startButton.Select();
        HUD.GetComponent<HUDController>().FadeOutHUD();
        controls.UI.Close.performed += ctx => Close();
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    public void Close()
    {
        playerController.EnableController();
        UIEnable.SetActive(false);
        GlobalData.isAbleToPause = true;
        HUD.GetComponent<HUDController>().FadeInHUD();
    }
    public void StartGame()
    {
        sporeManagerScript.Save();
        profileManagerScript.Save();
        playerController.EnableController();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            Destroy(player);
        }
        //SceneManager.LoadScene("Prototype Level");
        playerController.EnableController();
        UIEnable.SetActive(false);
        GlobalData.isAbleToPause = true;

        float happinessStatMultiplier;
        int happinessStatIncrement;
        if (HappinessManager.Instance.doesHappinessMultiply)
        {
            happinessStatMultiplier = HappinessManager.Instance.GetHappinessStatMultiplier();
            string colonyHappinessWord;
            if (happinessStatMultiplier > 1f)
            {
                colonyHappinessWord = "<color=#38DB35>Happy</color>";
            }
            else if (happinessStatMultiplier < 1f)
            {
                colonyHappinessWord = "<color=#C83434>Unhappy</color>";
            }
            else
            {
                colonyHappinessWord = "<color=#FFD700>Neutral</color>";
            }
            NotificationManager.Instance.Notification("Your Colony is " + colonyHappinessWord, "All Stats +" + ((happinessStatMultiplier * 100f) - 100f) + "%");

            GlobalData.happinessStatMultiplier = happinessStatMultiplier;
        }
        else
        {
            happinessStatIncrement = HappinessManager.Instance.GetHappinessStatIncrement();
            string colonyHappinessWord;
            string plusMinus;
            string colorHex;
            if (happinessStatIncrement > 0f)
            {
                colonyHappinessWord = "<color=#38DB35>Happy</color>";
                plusMinus = "+";
                colorHex = "38DB35";
            }
            else if (happinessStatIncrement < 0f)
            {
                colonyHappinessWord = "<color=#C83434>Unhappy</color>";
                plusMinus = "-";
                colorHex = "C83434";
            }
            else
            {
                colonyHappinessWord = "<color=#FFD700>Neutral</color>";
                plusMinus = "+";
                colorHex = "FFD700";
            }

            int absoluteHappinessStatModifier = (int)Mathf.Abs(happinessStatIncrement);
            NotificationManager.Instance.Notification("Your Colony is " + colonyHappinessWord, "All Stats " + "<color=#"+colorHex+">" + plusMinus + absoluteHappinessStatModifier + "</color>");
        
            GlobalData.happinessStatIncrement = happinessStatIncrement;
        }

        SceneLoader.Instance.BeginLoadScene(3, "Dylano");
    }
}
