using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static System.TimeZoneInfo;

public class TextPopUp : MonoBehaviour
{
    //Options for the dropdowns
    private enum HideVia {Timer, PlayerInput, LeavingTrigger}
    private enum Input {Attack, Roll, SpeciesSkill, GoalCamera, None}

    //Text Pop Up stuff
    private CanvasGroup textPopUpCanvasGroup;
    private TMP_Text textPopUp;
    private RectTransform textPopUpRectTransform;

    //Used for script functionality
    private bool isActivated;

    //Input stuff
    private ThirdPersonActionsAsset playerInput = null;

    [Header("--General Settings--")]
    [SerializeField] private HideVia hideVia;
    [SerializeField] private bool shouldDestroyItself;
    [SerializeField] private bool shouldDestroyOther;
    [SerializeField] string textToDisplay;

    [Header("--If Hiding Via Timer--")]
    [SerializeField] private float timeToDisplay;

    [Header("--If Hiding Via Player Input--")]
    [SerializeField] private Input requiredInput;
    [SerializeField][Min(1)] private int numberOfPressesRequired;

    [Header("--If Destroying Other--")]
    [SerializeField] private GameObject toBeDestroyed;

    void Awake()
    {
        playerInput = new ThirdPersonActionsAsset();

        textPopUpCanvasGroup = GameObject.Find("TextPopUpCanvas").GetComponent<CanvasGroup>();
        textPopUp = GameObject.Find("TextPopUpCanvas").transform.GetChild(0).GetComponent<TMP_Text>();
        textPopUpRectTransform = GameObject.Find("TextPopUpCanvas").transform.GetChild(0).GetComponent<RectTransform>();

        isActivated = false;
}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "currentPlayer")
        {
            return;
        }

        if (isActivated == false)
        {
            isActivated = true;

            switch (hideVia)
            {
                case HideVia.Timer:
                    UpdateText(GetHintFromRequiredInput(requiredInput));
                    StartCoroutine(ShowText(0.5f));
                    StartCoroutine(StartTimer(timeToDisplay + 0.5f));
                    break;

                case HideVia.PlayerInput:
                    UpdateText(GetHintFromRequiredInput(requiredInput));
                    StartCoroutine(ShowText(0.5f));
                    StartCoroutine(WaitForInput(requiredInput, numberOfPressesRequired));
                    break;

                case HideVia.LeavingTrigger:
                    UpdateText(GetHintFromRequiredInput(requiredInput));
                    StartCoroutine(ShowText(0.5f));
                    break;

                default:
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "currentPlayer")
        {
            return;
        }

        if (hideVia == HideVia.LeavingTrigger)
        {
            StartCoroutine(HideText(0.5f));
        }
    }

    private float TEquation(float value)
    {
        float b = 1.18f;
        float c = 1.1f;

        value = ((Mathf.Pow((b * value), 2)) * (3f - 2f * (b * value))) * c;

        return value;
    }

    private void UpdateText(string newText)
    {
        textPopUp.text = newText;
    }

    /*private string CreatePlayerInputText(Input input)
    {
        string output = "null";

        switch(input)
        {
            case Input.Attack:
                output = "Press RT to attack!";
                break;

            case Input.Roll:
                output = "Press B to roll!";
                break;

            case Input.SpeciesSkill:
                output = "Press LT to use your species skill!";
                break;
            case Input.GoalCamera:
                output
            default:
                break;
        }

        return output;
    }*/

    IEnumerator StartTimer(float time)
    {
        yield return new WaitForSeconds(time);
        StartCoroutine(HideText(0.5f));
    }

    IEnumerator WaitForInput(Input input, int requiredPresses)
    {
        int currentPresses = 0;

        while(currentPresses < requiredPresses)
        {
            switch(input)
            {
                case Input.Attack:
                    if (playerInput.Player.Attack.WasPressedThisFrame())
                    {
                        currentPresses += 1;
                    }
                    break;

                case Input.Roll:
                    if (playerInput.Player.Dodge.WasPressedThisFrame())
                    {
                        currentPresses += 1;
                    }
                    break;

                case Input.SpeciesSkill:
                    if (playerInput.Player.Subspecies_Skill.WasPressedThisFrame())
                    {
                        currentPresses += 1;
                    }
                    break;

                case Input.GoalCamera:
                    if (playerInput.Player.NavigateCamera.WasPressedThisFrame())
                    {
                        currentPresses += 1;
                    }
                    break;

                default:
                    break;
            }

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(HideText(0.5f));
    }

    IEnumerator ShowText(float time)
    {
        float elapsedTime = 0f;
        float t = 0f;

        textPopUpCanvasGroup.alpha = 1f;

        while (elapsedTime < time)
        {
            t = elapsedTime / time;

            textPopUpRectTransform.localScale = new Vector3(Mathf.LerpUnclamped(t, 1f, TEquation(t)), Mathf.LerpUnclamped(0f, 1f, TEquation(t)), 1);

            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        textPopUpRectTransform.localScale = new Vector3(1, 1, 1);
    }

    IEnumerator HideText(float time)
    {
        float elapsedTime = 0f;
        float t = 0f;

        while (elapsedTime < time && textPopUpRectTransform.localScale.x >= 0)
        {
            t = elapsedTime / time;

            textPopUpRectTransform.localScale = new Vector3(Mathf.LerpUnclamped((1f - t), 0f, TEquation(t)), Mathf.LerpUnclamped(1f, 0f, TEquation(t)), 1);

            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        textPopUpCanvasGroup.alpha = 0f;

        if(shouldDestroyOther == true && toBeDestroyed != null)
        {
            Destroy(toBeDestroyed);
        }

        if(shouldDestroyItself == true)
        {
            Destroy(this.gameObject);
        }
        else
        {
            isActivated = false;
        }
    }

    string GetHintFromRequiredInput(Input requiredInput = Input.None)
    {
        string inputString;
        switch (requiredInput)
        {
            case Input.Roll:
                inputString = InputManager.Instance.GetLatestController().dodgeHint.GenerateColoredHintString();
                break;
            case Input.SpeciesSkill:
                inputString = InputManager.Instance.GetLatestController().subspeciesSkillHint.GenerateColoredHintString();
                break;
            case Input.GoalCamera:
                inputString = InputManager.Instance.GetLatestController().goalCameraHint.GenerateColoredHintString();
                break;
            default:
                inputString = InputManager.Instance.GetLatestController().attackHint.GenerateColoredHintString();
                break;
        }

        string displayHint = textToDisplay.Replace("{RequiredInput}", inputString);
        return displayHint;
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }
}
