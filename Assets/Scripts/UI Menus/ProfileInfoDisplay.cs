using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ProfileManager;

public class ProfileInfoDisplay : MonoBehaviour
{
    [SerializeField][Range(0, 2)] private int profileNumber;

    [Header("==Text==")]
    [SerializeField] private TMP_Text profileText;
    [SerializeField] private TMP_Text nutrientText;
    [SerializeField] private TMP_Text logText;
    [SerializeField] private TMP_Text exoText;
    [SerializeField] private TMP_Text calciteText;
    [SerializeField] private TMP_Text fleshText;
    [SerializeField] private TMP_Text createText;
    [SerializeField] private TMP_Text confirmDeleteText;

    [Header("==Misc.==")]
    [SerializeField] private GameObject modeImage;
    [SerializeField] private Button deleteProfileButton;
    [SerializeField] private GameObject modeMenu;

    private string filePath;
    private ProfileData profileData;

    private PauseMenu pauseMenuScript;
    private Vector3 originalProfileTextPos;

    private void Start()
    {
        pauseMenuScript = GameObject.Find("PauseMenuCanvas").GetComponent<PauseMenu>();
        originalProfileTextPos = new Vector3(0, 200, 0);

        RefreshProfileText();
    }

    private void OnEnable()
    {
        RefreshProfileText();
    }

    void RefreshProfileText()
    {
        //Set the file path
        if (Application.isEditor)
        {
            filePath = Application.dataPath + "/ProfileData" + profileNumber + ".json";
        }
        else
        {
            filePath = Application.persistentDataPath + "/ProfileData" + profileNumber + ".json";
        }

        profileText.rectTransform.localPosition = originalProfileTextPos;
        modeImage.SetActive(false);

        try
        {
            profileData = JsonUtility.FromJson<ProfileData>(System.IO.File.ReadAllText(filePath));

            nutrientText.text = profileData.nutrients.ToString();
            logText.text = profileData.log.ToString();
            exoText.text = profileData.exoskeleton.ToString();
            calciteText.text = profileData.calcite.ToString();
            fleshText.text = profileData.flesh.ToString();

            if(profileData.permadeathIsOn == true)
            {
                profileText.rectTransform.localPosition = originalProfileTextPos + new Vector3(-25, 0, 0);
                modeImage.SetActive(true);
            }
        }
        catch
        {
            nutrientText.transform.parent.gameObject.SetActive(false);
            logText.transform.parent.gameObject.SetActive(false);
            exoText.transform.parent.gameObject.SetActive(false);
            calciteText.transform.parent.gameObject.SetActive(false);
            fleshText.transform.parent.gameObject.SetActive(false);

            createText.gameObject.SetActive(true);
            deleteProfileButton.gameObject.SetActive(false);
        }
    }

    public void PlayOrCreate()
    {
        if(createText.gameObject.activeSelf == true)
        {
            RememberButton();
            modeMenu.SetActive(true);
            GameObject.Find("EasyMode").GetComponent<Button>().Select();
            transform.parent.gameObject.SetActive(false);
        }
        else
        {
            GameObject.Find("Canvas").GetComponent<MainMenu>().StartGame();
        }
    }

    public void SetGlobalProfileNumber()
    {
        GlobalData.profileNumber = profileNumber;
        Debug.Log("Now using profile " + profileNumber);
    }

    public void RememberButton()
    {
        pauseMenuScript.profileToSelect = GetComponent<Button>();
    }

    public void PrepareProfileDelete()
    {
        string profileLetter = "null";

        switch(profileNumber)
        {
            case 0:
                profileLetter = "A?";
                break;
            case 1:
                profileLetter = "B?";
                break;
            case 2:
                profileLetter = "C?";
                break;
            default:
                break;
        }
        confirmDeleteText.text = "Are you sure you wish to DELETE Profile " + profileLetter;

        pauseMenuScript.profileToDelete = profileNumber;
        pauseMenuScript.profileToSelect = GetComponent<Button>();
        Debug.Log("Preparing to delete profile " + profileNumber);
    }
}
