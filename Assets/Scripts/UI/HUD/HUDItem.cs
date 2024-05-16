using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDItem : MonoBehaviour
{
    [SerializeField] RectTransform itemPanel;
    [SerializeField] RectTransform materialOutsideTarget;
    [SerializeField] RectTransform materialInsideTarget;
    [SerializeField] List<Sprite> itemSprites = new List<Sprite>();
    [SerializeField] Image itemImage;
    //[SerializeField] float zipDuration = 0.5f;

    public void PickUpItem(string itemName)
    {
        // itemPanel.SetActive(true);
        
        foreach(Sprite sprite in itemSprites)
        {
            if (itemName.Contains(sprite.name))
            {
                itemImage.sprite = sprite;
            }
        }

        GetComponent<HUDController>().SlideHUDElement(itemPanel, materialInsideTarget);
    }

    public void LostItem()
    {
        //itemPanel.SetActive(false);

        GetComponent<HUDController>().SlideHUDElement(itemPanel, materialOutsideTarget);
    }

    /*IEnumerator PickUpAnimation(Sprite itemSprite)
    {
        float zipCounter1 = 0f;
        while (zipCounter1 < zipDuration / 2)
        {
            float zipLerp = EaseOutBounce(zipCounter1 / (zipDuration / 2));

            itemHolderRectTransform.anchoredPosition = Vector2.Lerp(itemInPositionRectTransform.anchoredPosition, itemOutPositionRectTransform.anchoredPosition, zipLerp);

            zipCounter1 += Time.deltaTime;
            yield return null;  
        }

        itemImage.sprite = itemSprite;

        float zipCounter2 = 0f;
        while (zipCounter2 < zipDuration / 2)
        {
            float zipLerp = EaseOutBounce(zipCounter2 / (zipDuration / 2));
            
            itemHolderRectTransform.anchoredPosition = Vector2.Lerp(itemOutPositionRectTransform.anchoredPosition, itemInPositionRectTransform.anchoredPosition, zipLerp);

            zipCounter2 += Time.deltaTime;
            yield return null;  
        }
    }

    IEnumerator LoseItemAnimation()
    {
        float zipCounter = 0f;
        while (zipCounter < zipDuration / 2)
        {
            float zipLerp = EaseOutBounce(zipCounter / (zipDuration / 2));
            
            itemHolderRectTransform.anchoredPosition = Vector2.Lerp(itemOutPositionRectTransform.anchoredPosition, itemInPositionRectTransform.anchoredPosition, zipLerp);

            zipCounter += Time.deltaTime;
            yield return null;  
        }
    }

    float EaseOutBounce(float x)
    {
        const float n1 = 7.5625f;
        const float d1 = 2.75f;

        if (x < 1f / d1)
        {
            return n1 * x * x;
        }
        else if (x < 2f / d1)
        {
            x -= 1.5f / d1;
            return n1 * x * x + 0.75f;
        }
        else if (x < 2.5f / d1)
        {
            x -= 2.25f / d1;
            return n1 * x * x + 0.9375f;
        }
        else
        {
            x -= 2.625f / d1;
            return n1 * x * x + 0.984375f;
        }
    }*/
}
