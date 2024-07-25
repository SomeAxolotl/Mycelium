using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using RonaldSunglassesEmoji.Interaction;

public class StatUpgrade : MonoBehaviour, IInteractable
{
    GameObject player;
    CharacterStats characterStats;
    int statNumber1;
    int statNumber2;
    string upgradeStat1;
    string upgradeStat2;

    [SerializeField] bool doesMultiply = false;
    [SerializeField] float multiplyAmount = 1.0f;
    [SerializeField] int statIncreaseStartingAmount = 2;
    int statIncreaseAmount;

    HUDStats hudStats;

    void Start()
    {
        //Every 3 loops, the stat increase goes up by 1
        statIncreaseAmount = statIncreaseStartingAmount + (GlobalData.currentLoop / 3);

        List<int> statNumbers = new List<int> {0, 1, 2, 3};
        statNumber1 = statNumbers[Random.Range(0, statNumbers.Count)];
        statNumbers.Remove(statNumber1);
        statNumber2 = statNumbers[Random.Range(0, statNumbers.Count)];

        upgradeStat1 = NumberToStat(statNumber1);
        upgradeStat2 = NumberToStat(statNumber2);

        hudStats = GameObject.Find("HUD").GetComponent<HUDStats>();
    }

    string NumberToStat(int statNumber)
    {
        switch (statNumber)
        {
            case 0:
                return "Primal";
            case 1:
                return "Speed";
            case 2:
                return "Sentience";
            case 3:
                return "Vitality";
        }

        return null;
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Rotate(0, 60 * Time.deltaTime, 0);
    }

    public void Interact(GameObject interactObject)
    {
        Upgrade1();
    }

    public void Swap(GameObject interactObject)
    {
        //buh
    }

    public void Salvage(GameObject interactObject)
    {
        Upgrade2();
    }

    public void CreateTooltip(GameObject interactObject)
    {
        hudStats.ShowStats();

        string interactText = InputManager.Instance.GetLatestController().interactHint.GenerateColoredHintString();
        string salvageText = InputManager.Instance.GetLatestController().salvageHint.GenerateColoredHintString();
        if (doesMultiply)
        {
            TooltipManager.Instance.CreateTooltip
            (
                gameObject, 
                "Nutritious Deposit", 
                "Press "+interactText+"  <sprite="+statNumber1+"> +" + ((multiplyAmount * 100) - 100f) + "%" + "\nOR \nPress "+salvageText+"  <sprite="+statNumber2+"> +" + ((multiplyAmount * 100f) - 100f) + "%", 
                "Choose One"
            );
        }
        else
        {
            TooltipManager.Instance.CreateTooltip
            (
                gameObject, 
                "Nutritious Deposit", 
                "Press "+interactText+"  <sprite="+statNumber1+"> +" + statIncreaseAmount + "\nOR \nPress "+salvageText+"  <sprite="+statNumber2+"> +" + statIncreaseAmount, 
                "Choose One"
            );
        }
    }

    public void DestroyTooltip(GameObject interactObject, bool isFromInteracting = false)
    {
        if (!isFromInteracting)
        {
            hudStats.HideStats();
        }

        TooltipManager.Instance.DestroyTooltip();
    }

    void Upgrade1()
    {
        player = GameObject.FindWithTag("currentPlayer");
        characterStats = player.GetComponent<CharacterStats>();

        if (doesMultiply)
        {
            characterStats.MultiplyStat(upgradeStat1, multiplyAmount);
        }
        else
        {
            characterStats.AddStat(upgradeStat1, statIncreaseAmount);
        }
        
        SoundEffectManager.Instance.PlaySound("Pickup", transform.position);
        DestroyTooltip(this.gameObject, true);
        hudStats.FlashHUDStats();
        hudStats.HideStats(1.5f);
        Destroy(this.gameObject);
    }

    void Upgrade2()
    {
        player = GameObject.FindWithTag("currentPlayer");
        characterStats = player.GetComponent<CharacterStats>();

        if (doesMultiply)
        {
            characterStats.MultiplyStat(upgradeStat2, multiplyAmount);
        }
        else
        {
            characterStats.AddStat(upgradeStat2, statIncreaseAmount);
        }

        SoundEffectManager.Instance.PlaySound("Pickup", transform.position);
        DestroyTooltip(this.gameObject, true);
        hudStats.FlashHUDStats();
        hudStats.HideStats(1.5f);
        Destroy(this.gameObject);
    }
}
