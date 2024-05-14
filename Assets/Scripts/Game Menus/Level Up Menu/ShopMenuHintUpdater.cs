using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ShopMenuHintUpdater : MonoBehaviour
{
    [SerializeField] List<ShopHint> shopHints = new List<ShopHint>();

    void OnEnable()
    {
        foreach (ShopHint shopHint in shopHints)
        {
            var retrievedHint = InputManager.Instance.GetLatestController().GetHintFromInputActionReference(shopHint.hintInput);

            if (retrievedHint.isHintSprite)
            {
                shopHint.hintImage.sprite = retrievedHint.controlSprite;
                shopHint.ToggleImage();
            }
            else
            {
                shopHint.hintText.text = retrievedHint.controlText;
                shopHint.ToggleText();
            }
        }
    }

    [System.Serializable]
    public class ShopHint
    {
        [SerializeField] public InputActionReference hintInput;

        [SerializeField] public TMP_Text hintText;
        [SerializeField] public Image hintImage;

        public void ToggleText()
        {
            hintText?.gameObject.SetActive(true);
            hintImage?.gameObject.SetActive(false);
        }
        public void ToggleImage()
        {
            hintImage?.gameObject.SetActive(true);
            hintText?.gameObject.SetActive(false);
        }
    }    
}
