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

    [SerializeField] private TMP_Text nutrientText;
    [SerializeField] private TMP_Text logText;
    [SerializeField] private TMP_Text exoText;
    [SerializeField] private TMP_Text calciteText;
    [SerializeField] private TMP_Text fleshText;
    [SerializeField] private TMP_Text createText;
    [SerializeField] private TMP_Text confirmDeleteText;
    [SerializeField] private Button deleteProfileButton;

    private string filePath;
    private ProfileData profileData;

    private PauseMenu pauseMenuScript;

    private void Start()
    {
        pauseMenuScript = GameObject.Find("PauseMenuCanvas").GetComponent<PauseMenu>();

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

        try
        {
            profileData = JsonUtility.FromJson<ProfileData>(System.IO.File.ReadAllText(filePath));

            nutrientText.text = profileData.nutrients.ToString();
            logText.text = profileData.log.ToString();
            exoText.text = profileData.exoskeleton.ToString();
            calciteText.text = profileData.calcite.ToString();
            fleshText.text = profileData.flesh.ToString();
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

    public void SetGlobalProfileNumber()
    {
        GlobalData.profileNumber = profileNumber;
        Debug.Log("Now using profile " + profileNumber);
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
