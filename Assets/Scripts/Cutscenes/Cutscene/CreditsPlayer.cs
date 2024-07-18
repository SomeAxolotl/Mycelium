using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using static System.TimeZoneInfo;

public class CreditsPlayer : MonoBehaviour
{
    [Header("==Credits Section==")]
    [SerializeField] private CanvasGroup blackCanvas;
    [SerializeField] private CanvasGroup creditsCanvas;
    [SerializeField] private CanvasGroup skipCanvas;

    [SerializeField] private RectTransform movePoint;

    [SerializeField] private float fadeTimeBlackCanvas;
    [SerializeField] private float fadeTimeCreditsCanvas;
    [SerializeField][Tooltip("Time in seconds")] private float creditsTime;
    [SerializeField] private float pauseTime;

    [Header("==End Of Run Section==")]
    [SerializeField] private CanvasGroup endOfRunCanvas;
    [SerializeField] private TMP_Text endingText;
    [SerializeField] private TMP_Text difficultyText;
    [SerializeField] private float fadeTimeEndOfRunCanvas;

    private PlayerController playerController;
    private LevelEnd levelEndScript;
    private GameObject camTracker;
    private bool creditsIsOn = false;
    private bool askSkipIsOn = false;

    private void Start()
    {
        playerController = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>();
        levelEndScript = GameObject.Find("LevelEndObject").GetComponent<LevelEnd>();
        camTracker = GameObject.FindWithTag("Camtracker");
    }

    private void OnSkip()
    {
        if (creditsIsOn == false)
        {
            return;
        }

        if (askSkipIsOn == true)
        {
            ConfirmSkip();
            return;
        }

        StartCoroutine(AskSkip());
    }

    public void StartPlayCredits()
    {
        GlobalData.isAbleToPause = false;
        playerController.DisableController();

        endingText.text = 
        (
            "You conquered Environment Tier " + GlobalData.currentLoop + "\n \n" +

            "CONTINUE to Tier " + (GlobalData.currentLoop + 1) + " or\nFINISH your adventure?"
        );
        difficultyText.text = "Enemy Stats +" + (GlobalData.currentLoop) * 100 + "%" + "\nNutrients Gained +" + (GlobalData.currentLoop) * 50 + "%";

        StartCoroutine(EndOfRun());
    }

    private void ConfirmSkip()
    {
        StopAllCoroutines();

        SceneLoader.Instance.BeginLoadScene("The Carcass", true);
    }

    public void LoopRun(int currentLoop)
    {
        EventSystem.current.SetSelectedGameObject(null);

        //Increment Area Completion Counts
        GlobalData.areaCleared = true;
        GlobalData.areasClearedThisRun ++;

        UnlockFurnitureFromBeatingLoop(GlobalData.currentLoop);
        ProfileManager.Instance.TestAgainstHighestLoopRecord(GlobalData.currentLoop);
        ProfileManager.Instance.SaveOverride();

        StartCoroutine(FadeOut(endOfRunCanvas, fadeTimeEndOfRunCanvas));

        GlobalData.currentLoop++;

        levelEndScript.SpecialCreditsLoopFunction();

        SceneLoader.Instance.BeginLoadScene("Daybreak Arboretum", "Meow");
        creditsIsOn = false;
    }

    public void FinishRun()
    {
        EventSystem.current.SetSelectedGameObject(null);

        UnlockFurnitureFromBeatingLoop(GlobalData.currentLoop);
        ProfileManager.Instance.TestAgainstHighestLoopRecord(GlobalData.currentLoop);
        ProfileManager.Instance.SaveOverride();

        // doesnt do anything functionally, this is for tracking Spore loop records
        // (currentLoop is reset on starting a run now)
        GlobalData.currentLoop++;
        
        StartCoroutine(PlayCredits());
    }

    public void UnlockFurnitureFromBeatingLoop(int loop)
    {
        string unlockString = "";
        bool showNotification = false;
        switch (loop)
        {
            case 1:
                unlockString = "Mushroom Bed";
                if (!FurnitureManager.Instance.FurnitureIsUnlocked(unlockString))
                {
                    showNotification = true;
                }
                FurnitureManager.Instance.bedIsUnlocked = true;
                break;
            case 2:
                unlockString = "Stump Chairs";
                if (!FurnitureManager.Instance.FurnitureIsUnlocked(unlockString))
                {
                    showNotification = true;
                }
                FurnitureManager.Instance.chairIsUnlocked = true;
                break;
            case 3:
                unlockString = "Bonfire";
                if (!FurnitureManager.Instance.FurnitureIsUnlocked(unlockString))
                {
                    showNotification = true;
                }
                FurnitureManager.Instance.fireIsUnlocked = true;
                break;
            case 5:
                unlockString = "Firefly Bottle";
                if (!FurnitureManager.Instance.FurnitureIsUnlocked(unlockString))
                {
                    showNotification = true;
                }
                FurnitureManager.Instance.fireflyIsUnlocked = true;
                break;
            case 7:
                unlockString = "Game Board";
                if (!FurnitureManager.Instance.FurnitureIsUnlocked(unlockString))
                {
                    showNotification = true;
                }
                FurnitureManager.Instance.gameboardIsUnlocked = true;
                break;
            case 9:
                unlockString = "Beetle Drum";
                if (!FurnitureManager.Instance.FurnitureIsUnlocked(unlockString))
                {
                    showNotification = true;
                }
                FurnitureManager.Instance.drumIsUnlocked = true;
                break; 
        }

        if (showNotification)
        {
            NotificationManager.Instance.Notification("You unlocked the <color=#d9db4d>" + unlockString + "</color>!", "Check it out at the Carcass!");
        }
    }

    public void PlaySelectSound()
    {
        SoundEffectManager.Instance.PlaySound("UISelect", camTracker.transform);
    }

    public void PlayMoveSound()
    {
        SoundEffectManager.Instance.PlaySound("UIMove", camTracker.transform);
    }

    IEnumerator AskSkip()
    {
        askSkipIsOn = true;

        yield return StartCoroutine(FadeIn(skipCanvas, 0.3f));

        yield return new WaitForSecondsRealtime(2f);

        yield return StartCoroutine(FadeOut(skipCanvas, 0.3f));

        askSkipIsOn = false;
    }

    IEnumerator PlayCredits()
    {
        creditsIsOn = true;

        yield return StartCoroutine(FadeOut(endOfRunCanvas, fadeTimeEndOfRunCanvas));

        yield return new WaitForSecondsRealtime(pauseTime);

        yield return StartCoroutine(FadeIn(creditsCanvas, fadeTimeCreditsCanvas));

        yield return new WaitForSecondsRealtime(pauseTime);

        yield return StartCoroutine(MoveText());

        yield return new WaitForSecondsRealtime(pauseTime);

        yield return StartCoroutine(FadeOut(creditsCanvas, fadeTimeCreditsCanvas));

        yield return new WaitForSecondsRealtime(1f);

        SceneLoader.Instance.BeginLoadScene("The Carcass", true);

        creditsIsOn = false;
    }

    IEnumerator EndOfRun()
    {
        yield return StartCoroutine(FadeIn(blackCanvas, fadeTimeBlackCanvas));

        yield return new WaitForSeconds(1f);

        GameObject.Find("ContinueButton").GetComponent<Button>().Select();
        yield return StartCoroutine(FadeIn(endOfRunCanvas, fadeTimeEndOfRunCanvas));
    }

    IEnumerator MoveText()
    {
        float elapsedTime = 0f;
        float t = 0f;

        while (elapsedTime < creditsTime)
        {
            t = elapsedTime / creditsTime;

            movePoint.localPosition = new Vector3(0f, Mathf.Lerp(0f, 2532f, t), 0f);

            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }
    }

    IEnumerator FadeIn(CanvasGroup canvasGroup, float transitionTime)
    {
        float elapsedTime = 0f;
        float t = 0f;

        while (elapsedTime < transitionTime)
        {
            t = elapsedTime / transitionTime;

            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);

            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    IEnumerator FadeOut(CanvasGroup canvasGroup, float transitionTime)
    {
        float elapsedTime = 0f;
        float t = 0f;

        while (elapsedTime < transitionTime)
        {
            t = elapsedTime / transitionTime;

            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);

            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        canvasGroup.alpha = 0f;
    }
}
