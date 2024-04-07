using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance;
            
    [SerializeField] private float popDuration = 1f;
    [SerializeField] private float waitDuration = 1f;
    [SerializeField] private float fadeDuration = 1f;

    [SerializeField] private CanvasGroup notificationCanvasGroup;
    [SerializeField] private GameObject notificationHolder;
    [SerializeField] private TMP_Text notificationText;
    [SerializeField] private TMP_Text notificationCenterText;
    [SerializeField] private TMP_Text helperText;
    [SerializeField] private TMP_Text centerHelperText;
    [SerializeField] private Image notificationIcon;

    [SerializeField] private GameObject notificationPanel;

    [SerializeField] private Sprite noSkill;
    [SerializeField] private List<Sprite> notificationSprites;

    private HUDSkills hudSkills;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        hudSkills = GameObject.Find("HUD").GetComponent<HUDSkills>();
        notificationCanvasGroup.alpha = 0f;
        notificationHolder.transform.localScale = Vector3.zero;
    }

    public void Notification(string notification, string helper = "", Sprite notificationSprite = null, string notificationSpriteString = "")
    {
        StartCoroutine(NotificationCoroutine(notification, helper, notificationSprite, notificationSpriteString));
    }

    IEnumerator NotificationCoroutine(string notification, string helper, Sprite notificationSprite, string notificationSpriteString)
    {
        //notificationPanel.gameObject.SetActive(false);
        notificationIcon.sprite = noSkill;
        if (notificationSprite != null)
        {
            //notificationPanel.gameObject.SetActive(true);
            notificationIcon.sprite = notificationSprite;
        }
        else if (notificationSpriteString != "")
        {
            foreach (Sprite sprite in notificationSprites)
            {
                if (sprite.name == notificationSpriteString)
                {
                    //notificationPanel.gameObject.SetActive(true);
                    notificationIcon.sprite = sprite;
                }
            }
        }

        //Helper
        helperText.text = "";
        centerHelperText.text = "";
        if (notificationIcon.sprite == noSkill)
        {
            centerHelperText.text = helper;
        }
        else
        {
            helperText.text = helper;
        }

        //Notification
        notificationText.text = "";
        notificationCenterText.text = "";
        if (helperText.text == "" && centerHelperText.text == "")
        {
            notificationCenterText.text = notification;
        }
        else
        {
            notificationText.text = notification;
        }

        notificationCanvasGroup.alpha = 1f;

        float popCounter = 0f;
        while (popCounter < popDuration)
        {
            float popLerp = EaseOutQuart(popCounter / popDuration);
            notificationHolder.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, popLerp);

            popCounter += Time.deltaTime;
            yield return null;  
        }

        yield return new WaitForSeconds(waitDuration);

        float fadeCounter = 0f;
        while (fadeCounter < fadeDuration)
        {
            float fadeLerp = EaseOutQuart(fadeCounter / fadeDuration);
            notificationCanvasGroup.alpha = Mathf.Lerp(1f, 0f, fadeLerp);

            fadeCounter += Time.deltaTime;
            yield return null;
        }

        notificationHolder.transform.localScale = Vector3.zero;
        notificationCanvasGroup.alpha = 1f;
    }

    /*public void NotifySkillUnlock(string skillName, Sprite skillSprite)
    {
        StartCoroutine(NotifySkillUnlockCoroutine(skillName, skillSprite));
    }

    IEnumerator NotifySkillUnlockCoroutine(string skillName, Sprite skillSprite)
    {
        notificationText.text = "Skill Unlocked - " + skillName;
        notificationIcon.sprite = hudSkills.GetSkillSprite(skillName);

        notificationCanvasGroup.alpha = 1f;

        float popCounter = 0f;
        while (popCounter < popDuration)
        {
            float popLerp = TEquation(popCounter / popDuration);
            notificationHolder.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, popLerp);

            popCounter += Time.deltaTime;
            yield return null;  
        }

        yield return new WaitForSeconds(waitDuration);

        float fadeCounter = 0f;
        while (fadeCounter < fadeDuration)
        {
            float fadeLerp = TEquation(fadeCounter / fadeDuration);
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, fadeLerp);

            fadeCounter += Time.deltaTime;
            yield return null;
        }

        notificationHolder.transform.localScale = Vector3.zero;
        notificationCanvasGroup.alpha = 1f;
    }

    public void NotifyMultipleSkillUnlocks(int unlockCount)
    {
        StartCoroutine(NotifyMultipleSkillUnlockCoroutine(unlockCount));
    }

    IEnumerator NotifyMultipleSkillUnlockCoroutine(int unlockCount)
    {
        notificationText.text = unlockCount + " New Skills Unlocked";

        notificationCanvasGroup.alpha = 1f;

        notificationPanel.SetActive(false);

        float popCounter = 0f;
        while (popCounter < popDuration)
        {
            float popLerp = TEquation(popCounter / popDuration);
            notificationHolder.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, popLerp);

            popCounter += Time.deltaTime;
            yield return null;  
        }

        yield return new WaitForSeconds(waitDuration);

        float fadeCounter = 0f;
        while (fadeCounter < fadeDuration)
        {
            float fadeLerp = TEquation(fadeCounter / fadeDuration);
            notificationCanvasGroup.alpha = Mathf.Lerp(1f, 0f, fadeLerp);

            fadeCounter += Time.deltaTime;
            yield return null;
        }

        notificationHolder.transform.localScale = Vector3.zero;
        notificationCanvasGroup.alpha = 1f;
        notificationPanel.SetActive(true);
    }
    */

    float EaseOutQuart(float x)
    {
        return 1f - Mathf.Pow(1f - x, 4);
    }

    private float TEquation(float value)
    {
        float b = 1.18f;
        float c = 1.1f;

        value = ((Mathf.Pow((b * value), 2)) * (3f - 2f * (b * value))) * c;

        return value;
    }
}
