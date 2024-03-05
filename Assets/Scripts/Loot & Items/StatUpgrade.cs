using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class StatUpgrade : MonoBehaviour
{
    ThirdPersonActionsAsset playerActionsAsset;
    InputAction upgrade1;
    InputAction upgrade2;
    GameObject player;
    CharacterStats characterStats;
    string upgradeStat1;
    string upgradeStat2;

    [SerializeField] float multiplyAmount = 1.0f;
    void Start()
    {
        playerActionsAsset = new ThirdPersonActionsAsset();
        playerActionsAsset.Player.Enable();
        upgrade1 = playerActionsAsset.Player.Interact;
        upgrade2 = playerActionsAsset.Player.Salvage;
        player = GameObject.FindWithTag("currentPlayer");
        characterStats = player.GetComponent<CharacterStats>();

        List<int> statNumbers = new List<int> {0, 1, 2, 3};
        int statNumber1 = statNumbers[Random.Range(0, statNumbers.Count)];
        statNumbers.Remove(statNumber1);
        int statNumber2 = statNumbers[Random.Range(0, statNumbers.Count)];

        upgradeStat1 = NumberToStat(statNumber1);
        upgradeStat2 = NumberToStat(statNumber2);
    }

    string NumberToStat(int statNumber)
    {
        switch (statNumber)
        {
            case 0:
                return "Primal";
            case 1:
                return "Sentience";
            case 2:
                return "Speed";
            case 3:
                return "Vitality";
        }

        return null;
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Rotate(0, 60 * Time.deltaTime, 0);

        float distance = Vector3.Distance(player.transform.position, this.transform.position);
        if (distance < 3f)
        {
            string buttonText = "<color=#3cdb4e>A</color>";
            TooltipManager.Instance.CreateTooltip(gameObject, "Nutrient Deposit", "Press "+buttonText+" - " + multiplyAmount + "x " + upgradeStat1 + "\nOR" + "\nHold "+buttonText+" - " + multiplyAmount + "x " +upgradeStat2, "Choose One");

            if (upgrade1.triggered)
            {
                Upgrade1();
            }
            if (upgrade2.triggered)
            {
                Debug.Log("upgrade2");
                Upgrade2();
            }
        }
        else if (distance >3f && distance <5f)
        {
            TooltipManager.Instance.DestroyTooltip();
        }
    }

    void Upgrade1()
    {
        characterStats.MultiplyStat(upgradeStat1, multiplyAmount);
        SoundEffectManager.Instance.PlaySound("Pickup", transform.position);
        TooltipManager.Instance.DestroyTooltip();
        Destroy(this.gameObject);
    }

    void Upgrade2()
    {
        characterStats.MultiplyStat(upgradeStat2, multiplyAmount);
        SoundEffectManager.Instance.PlaySound("Pickup", transform.position);
        TooltipManager.Instance.DestroyTooltip();
        Destroy(this.gameObject);
    }
}
