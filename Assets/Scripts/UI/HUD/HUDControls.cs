using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDControls : MonoBehaviour
{
    [SerializeField] SkillHint subspeciesSkillHint;
    [SerializeField] SkillHint statSkill1Hint;
    [SerializeField] SkillHint statSkill2Hint;

    [SerializeField] TMP_Text dodgeSkillHintText;

    public void ChangeHUDControls(InputManager.Controller controller)
    {
        Debug.Log($"Refreshing HUD with {controller.controllerName} controls");

        if (controller.subspeciesSkillHint.isHintSprite)
        {
            subspeciesSkillHint.skillHintImage.sprite = controller.subspeciesSkillHint.controlSprite;
            subspeciesSkillHint.ToggleImage();
        }
        else
        {
            subspeciesSkillHint.skillHintText.text = controller.subspeciesSkillHint.GenerateColoredHintString(true);
            subspeciesSkillHint.ToggleText();
        }

        if (controller.statSkill1Hint.isHintSprite)
        {
            statSkill1Hint.skillHintImage.sprite = controller.statSkill1Hint.controlSprite;
            statSkill1Hint.ToggleImage();
        }
        else
        {
            statSkill1Hint.skillHintText.text = controller.statSkill1Hint.GenerateColoredHintString(true);
            statSkill1Hint.ToggleText();
        }

        if (controller.statSkill2Hint.isHintSprite)
        {
            statSkill2Hint.skillHintImage.sprite = controller.statSkill2Hint.controlSprite;
            statSkill2Hint.ToggleImage();
        }
        else
        {
            statSkill2Hint.skillHintText.text = controller.statSkill2Hint.GenerateColoredHintString(true);
            statSkill2Hint.ToggleText();
        }

        dodgeSkillHintText.text = controller.dodgeHint.GenerateColoredHintString(true);
    }

    [System.Serializable]
    public class SkillHint
    {
        [SerializeField] public TMP_Text skillHintText;
        [SerializeField] public Image skillHintImage;

        public void ToggleText()
        {
            skillHintText.gameObject.SetActive(true);
            skillHintImage.gameObject.SetActive(false);
        }
        public void ToggleImage()
        {
            skillHintImage.gameObject.SetActive(true);
            skillHintText.gameObject.SetActive(false);
        }
    }    
}
