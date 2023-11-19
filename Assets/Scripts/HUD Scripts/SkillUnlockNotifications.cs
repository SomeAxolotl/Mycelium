using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillUnlockNotifications : MonoBehaviour
{
    [SerializeField] private float popDuration = 1f;
    [SerializeField] private float waitDuration = 1f;
    [SerializeField] private float fadeDuration = 1f;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject skillUnlockHolder;
    [SerializeField] private TMP_Text skillUnlockText;
    [SerializeField] private Image skillUnlockIcon;

    [SerializeField] private GameObject skillUnlockPanel;

    private HUDSkills hudSkills;

    void Start()
    {
        hudSkills = GameObject.Find("HUD").GetComponent<HUDSkills>();
        canvasGroup.alpha = 0f;
        skillUnlockHolder.transform.localScale = Vector3.zero;
    }

    public void NotifySkillUnlock(string skillName, Sprite skillSprite)
    {
        StartCoroutine(NotifySkillUnlockCoroutine(skillName, skillSprite));
    }

    IEnumerator NotifySkillUnlockCoroutine(string skillName, Sprite skillSprite)
    {
        skillUnlockText.text = skillName + " Unlocked";
        skillUnlockIcon.sprite = hudSkills.GetSkillSprite(skillName);

        canvasGroup.alpha = 1f;

        float popCounter = 0f;
        while (popCounter < popDuration)
        {
            float popLerp = EaseOutQuart(popCounter / popDuration);
            skillUnlockHolder.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, popLerp);

            popCounter += Time.deltaTime;
            yield return null;  
        }

        yield return new WaitForSeconds(waitDuration);

        float fadeCounter = 0f;
        while (fadeCounter < fadeDuration)
        {
            float fadeLerp = EaseOutQuart(fadeCounter / fadeDuration);
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, fadeLerp);

            fadeCounter += Time.deltaTime;
            yield return null;
        }

        skillUnlockHolder.transform.localScale = Vector3.zero;
        canvasGroup.alpha = 1f;
    }

    public void NotifyMultipleSkillUnlocks(int unlockCount)
    {
        StartCoroutine(NotifyMultipleSkillUnlockCoroutine(unlockCount));
    }

    IEnumerator NotifyMultipleSkillUnlockCoroutine(int unlockCount)
    {
        skillUnlockText.text = unlockCount + " New Skills Unlocked";

        canvasGroup.alpha = 1f;

        skillUnlockPanel.SetActive(false);

        float popCounter = 0f;
        while (popCounter < popDuration)
        {
            float popLerp = EaseOutQuart(popCounter / popDuration);
            skillUnlockHolder.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, popLerp);

            popCounter += Time.deltaTime;
            yield return null;  
        }

        yield return new WaitForSeconds(waitDuration);

        float fadeCounter = 0f;
        while (fadeCounter < fadeDuration)
        {
            float fadeLerp = EaseOutQuart(fadeCounter / fadeDuration);
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, fadeLerp);

            fadeCounter += Time.deltaTime;
            yield return null;
        }

        skillUnlockHolder.transform.localScale = Vector3.zero;
        canvasGroup.alpha = 1f;
        skillUnlockPanel.SetActive(true);
    }

    float EaseOutQuart(float x)
    {
        return 1f - Mathf.Pow(1f - x, 4);
    }
}
