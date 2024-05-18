using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private GameObject tooltipCanvas;
    [SerializeField] private GameObject tooltipHolder;
    [SerializeField] public Image tooltipBackground;
    [SerializeField] public TMP_Text tooltipTitle;
    [SerializeField] public TMP_Text tooltipDescription;
    [SerializeField] public TMP_Text tooltipInteract;
    [SerializeField] public TMP_Text tooltipInteract2;
    [SerializeField] public Slider happinessSlider;
    [SerializeField] private float popDuration = 0.25f;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        tooltipCanvas.GetComponent<Canvas>().worldCamera = mainCamera;
        StartCoroutine(PopTooltip());
    }

    IEnumerator PopTooltip()
    {
        float popCounter = 0f;
        while (popCounter < popDuration)
        {
            float popLerp = DylanTree.EaseOutQuart(popCounter / popDuration);
            tooltipHolder.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, popLerp);

            popCounter += Time.deltaTime;
            yield return null;  
        }
    }

    public void ShowHappiness(float happinessValue)
    {
        happinessSlider.gameObject.SetActive(true);
        happinessSlider.value = happinessValue;
    }

    void Update()
    {
        tooltipCanvas.transform.rotation = Quaternion.LookRotation(tooltipCanvas.transform.position - mainCamera.transform.position);
    }
}
