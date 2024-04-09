using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDHappiness : MonoBehaviour
{
    [SerializeField] private Slider happinessSlider;
    [SerializeField] private TMP_Text colonyText;

    IEnumerator Start()
    {
        yield return null;

        UpdateHappinessMeter();
    }

    public void HideColonyHappinessMeter()
    {
        happinessSlider.gameObject.SetActive(false);
    }

    public void UpdateHappinessMeter()
    {
        float averageColonyHappiness = HappinessManager.Instance.GetAverageColonyHappiness();
        happinessSlider.value = averageColonyHappiness;

        int sporeCount = GameObject.FindWithTag("PlayerParent").GetComponent<SwapCharacter>().characters.Count;
        colonyText.text = "Colony x " + sporeCount;
    }
}
