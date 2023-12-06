using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private GameObject tooltipCanvas;
    [SerializeField] private GameObject tooltipHolder;
    [SerializeField] public TMP_Text tooltipTitle;
    [SerializeField] public TMP_Text tooltipDescription;
    [SerializeField] public TMP_Text tooltipInteract;
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
            float popLerp = EaseOutQuart(popCounter / popDuration);
            tooltipHolder.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, popLerp);

            popCounter += Time.deltaTime;
            yield return null;  
        }
    }

    float EaseOutQuart(float x)
    {
        return 1f - Mathf.Pow(1f - x, 4);
    }

    void Update()
    {
        tooltipCanvas.transform.rotation = Quaternion.LookRotation(tooltipCanvas.transform.position - mainCamera.transform.position);
    }
}
