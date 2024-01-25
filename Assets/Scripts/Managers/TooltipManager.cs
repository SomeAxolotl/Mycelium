using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;

    private GameObject currentTooltip;
    [SerializeField] private GameObject tooltipCanvasPrefab;

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

    public void CreateTooltip(GameObject parent, string title, string description, string interactString)
    {
        if (currentTooltip == null)
        {
            DestroyTooltip();
            currentTooltip = Instantiate(tooltipCanvasPrefab, parent.transform.position, Quaternion.identity);
            Tooltip tooltip = currentTooltip.GetComponent<Tooltip>();
            tooltip.tooltipTitle.text = title;
            tooltip.tooltipDescription.text = description;
            tooltip.tooltipInteract.text = interactString;
        }
    }

    public void DestroyTooltip()
    {
        if (currentTooltip != null)
        {
            Destroy(currentTooltip);
        }
    }
}
