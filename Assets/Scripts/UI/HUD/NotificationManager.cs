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

    [HideInInspector] public float totalDuration;

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
        notificationCanvasGroup.alpha = 0f;
        notificationHolder.transform.localScale = Vector3.zero;

        totalDuration = popDuration + waitDuration + fadeDuration;
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
            float popLerp = DylanTree.EaseOutQuart(popCounter / popDuration);
            notificationHolder.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, popLerp);

            popCounter += Time.deltaTime;
            yield return null;  
        }

        yield return new WaitForSeconds(waitDuration);

        float fadeCounter = 0f;
        while (fadeCounter < fadeDuration)
        {
            float fadeLerp = DylanTree.EaseOutQuart(fadeCounter / fadeDuration);
            notificationCanvasGroup.alpha = Mathf.Lerp(1f, 0f, fadeLerp);

            fadeCounter += Time.deltaTime;
            yield return null;
        }

        notificationHolder.transform.localScale = Vector3.zero;
        notificationCanvasGroup.alpha = 1f;
    }
}
