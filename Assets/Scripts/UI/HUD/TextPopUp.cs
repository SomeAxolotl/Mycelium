using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static System.TimeZoneInfo;

public class TextPopUp : MonoBehaviour
{
    //Options for the dropdowns
    private enum HideVia {Timer, PlayerInput, LeavingTrigger}
    private enum RequiredInput {Attack, Roll, SpeciesSkill}

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
    [SerializeField][Tooltip("This will do nothing when using PlayerInput.")] string textToDisplay;

    [Header("--If Hiding Via Timer--")]
    [SerializeField] private float timeToDisplay;

    [Header("--If Hiding Via Player Input--")]
    [SerializeField] private RequiredInput requiredInput;
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
        if (isActivated == false)
        {
            isActivated = true;

            switch (hideVia)
            {
                case HideVia.Timer:
                    UpdateText(textToDisplay);
                    StartCoroutine(ShowText(0.5f));
                    StartCoroutine(StartTimer(timeToDisplay + 0.5f));
                    break;

                case HideVia.PlayerInput:
                    UpdateText(CreatePlayerInputText(requiredInput));
                    StartCoroutine(ShowText(0.5f));
                    StartCoroutine(WaitForInput(requiredInput, numberOfPressesRequired));
                    break;

                case HideVia.LeavingTrigger:
                    UpdateText(textToDisplay);
                    StartCoroutine(ShowText(0.5f));
                    break;

                default:
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(hideVia == HideVia.LeavingTrigger)
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

    private string CreatePlayerInputText(RequiredInput input)
    {
        string output = "null";

        switch(input)
        {
            case RequiredInput.Attack:
                output = "Press RT to attack!";
                break;

            case RequiredInput.Roll:
                output = "Press B to roll!";
                break;

            case RequiredInput.SpeciesSkill:
                output = "Press LT to use your species skill!";
                break;

            default:
                break;
        }

        return output;
    }

    IEnumerator StartTimer(float time)
    {
        yield return new WaitForSeconds(time);
        StartCoroutine(HideText(0.5f));
    }

    IEnumerator WaitForInput(RequiredInput input, int requiredPresses)
    {
        int currentPresses = 0;

        while(currentPresses < requiredPresses)
        {
            switch(input)
            {
                case RequiredInput.Attack:
                    if (playerInput.Player.Attack.WasPressedThisFrame())
                    {
                        currentPresses += 1;
                    }
                    break;

                case RequiredInput.Roll:
                    if (playerInput.Player.Dodge.WasPressedThisFrame())
                    {
                        currentPresses += 1;
                    }
                    break;

                case RequiredInput.SpeciesSkill:
                    if (playerInput.Player.Subspecies_Skill.WasPressedThisFrame())
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

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }
}
