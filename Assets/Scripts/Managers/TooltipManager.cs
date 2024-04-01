using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;

    private GameObject currentTooltip;
    private GameObject currentParent;
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

    public void CreateTooltip(GameObject parent, string title, string description, string interactString, string interactString2 = "", bool shouldParent = false, float verticalOffset = 0f)
    {
        if (currentParent != parent)
        {
            Vector3 tooltipPosition = parent.transform.position;
            tooltipPosition.y += verticalOffset;

            DestroyTooltip();
            currentParent = parent;
            if (shouldParent)
            {
                currentTooltip = Instantiate(tooltipCanvasPrefab, tooltipPosition, Quaternion.identity, parent.transform);
            }
            else
            {
                currentTooltip = Instantiate(tooltipCanvasPrefab, tooltipPosition, Quaternion.identity);
            }
            Tooltip tooltip = currentTooltip.GetComponent<Tooltip>();
            tooltip.tooltipTitle.text = title;
            tooltip.tooltipDescription.text = description;
            tooltip.tooltipInteract.text = interactString;
            tooltip.tooltipInteract2.text = interactString2;
        }
    }

    public void DestroyTooltip()
    {
        if (currentTooltip != null)
        {
            currentParent = null;
            Destroy(currentTooltip);
        }
    }
}
