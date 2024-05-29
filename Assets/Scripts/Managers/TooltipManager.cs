using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;

    public GameObject currentTooltip;
    private GameObject currentParent;
    [SerializeField] private GameObject tooltipCanvasPrefab;

    [SerializeField] Color defaultBackgroundColor = Color.green;
    [SerializeField] Color noneBackgroundColor = Color.gray;
    [SerializeField] Color commonBackgroundColor = Color.white;
    [SerializeField] Color rareBackgroundColor = Color.blue;
    [SerializeField] Color legendaryBackgroundColor = Color.yellow;

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

    public Tooltip CreateTooltip(
                                GameObject parent,
                                string title,
                                string description,
                                string interactString,
                                string interactString2 = "",
                                bool shouldParent = false,
                                float verticalOffset = 0f,
                                bool descriptionShouldWrap = true,
                                AttributeAssigner.Rarity? rarity = null
                                )
    {
        Color backgroundColor = defaultBackgroundColor;
        switch (rarity)
        {
            case AttributeAssigner.Rarity.None:
            backgroundColor = noneBackgroundColor;
                break;
            case AttributeAssigner.Rarity.Common:
                backgroundColor = commonBackgroundColor;
                break;
            case AttributeAssigner.Rarity.Rare:
                backgroundColor = rareBackgroundColor;
                break;
            case AttributeAssigner.Rarity.Legendary:
                backgroundColor = legendaryBackgroundColor;
                break;
        }

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

            tooltip.tooltipBackground.color = backgroundColor;

            tooltip.tooltipDescription.enableWordWrapping = descriptionShouldWrap;

            return tooltip;
        }

        return null;
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
