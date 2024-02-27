using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SwapWeapon : MonoBehaviour
{
    private ThirdPersonActionsAsset playerActionsAsset;
    private InputAction swapItem;
    private Collider[] weaponColliders;
    [SerializeField] private LayerMask weaponLayer;
    Transform weapon;
    [HideInInspector] public GameObject currentCharacter;
    [HideInInspector] public Transform weaponHolder;
    SwapCharacter swapCharacter;
    PlayerController playerController;
    public GameObject curWeapon;

    [SerializeField] private float proximityRadius = 5f;
    [SerializeField] private Color betterStatColor;
    [SerializeField] private Color worseStatColor;
    [SerializeField] private Color evenStatColor;

    // Start is called before the first frame update
    void Start()
    {
        playerActionsAsset = new ThirdPersonActionsAsset();
        playerActionsAsset.Player.Enable();
        swapItem = playerActionsAsset.Player.SwapItem;
        swapCharacter = GetComponent<SwapCharacter>();
        playerController = GetComponent<PlayerController>();
        currentCharacter = GameObject.FindWithTag("currentPlayer");
        if (GameObject.FindWithTag("currentWeapon") == null && SceneManager.GetActiveScene().buildIndex == 1)
        {
            GameObject startingWeapon = Instantiate(Resources.Load("Weapons/StartWeapon"), GameObject.FindWithTag("WeaponSlot").transform) as GameObject;
            startingWeapon.layer = LayerMask.NameToLayer("currentWeapon");
            startingWeapon.GetComponent<Collider>().enabled = false;
            curWeapon = startingWeapon;

        }
    }

    // Update is called once per frame
    void Update()
    {
        weaponColliders = Physics.OverlapSphere(currentCharacter.transform.position, proximityRadius, weaponLayer);
        foreach (var weaponCollider in weaponColliders)
        {
            weapon = weaponCollider.transform;
            WeaponStats oldStats = curWeapon.GetComponent<WeaponStats>();
            WeaponStats newStats = weapon.gameObject.GetComponent<WeaponStats>();
            Vector3 dirToWeapon = (weapon.position - currentCharacter.transform.position).normalized;
            float angleToWeapon = Vector3.Angle(currentCharacter.transform.forward, dirToWeapon);
            float distanceToWeapon = Vector3.Distance(currentCharacter.transform.position, weapon.position);
            Scene currentScene = SceneManager.GetActiveScene();
            //nesting so i can use tooltips
            if (distanceToWeapon <= 3f && playerController.canAct && currentScene.buildIndex != 2)
            {
                string damageComparisonText;
                if (newStats.wpnDamage > oldStats.wpnDamage)
                {
                    damageComparisonText = "<color=#" + ColorUtility.ToHtmlStringRGB(betterStatColor) + "> +</color>";
                }
                else if (newStats.wpnDamage < oldStats.wpnDamage)
                {
                    damageComparisonText = "<color=#" + ColorUtility.ToHtmlStringRGB(worseStatColor) + "> -</color>";
                }
                else
                {
                    damageComparisonText = "<color=#" + ColorUtility.ToHtmlStringRGB(evenStatColor) + "> +-</color>";
                }
                string knockbackComparisonText;
                if (newStats.wpnKnockback > oldStats.wpnKnockback)
                {
                    knockbackComparisonText = "<color=#" + ColorUtility.ToHtmlStringRGB(betterStatColor) + "> +</color>";
                }
                else if (newStats.wpnKnockback < oldStats.wpnKnockback)
                {
                    knockbackComparisonText = "<color=#" + ColorUtility.ToHtmlStringRGB(worseStatColor) + "> -</color>";
                }
                else
                {
                    knockbackComparisonText = "<color=#" + ColorUtility.ToHtmlStringRGB(evenStatColor) + "> +-</color>";
                }

                string weaponName = newStats.wpnName;
                string buttonText = "<color=#3cdb4e>A</color>";
                TooltipManager.Instance.CreateTooltip
                    (weapon.gameObject, 
                    weaponName, 
                    "Speed: " + newStats.wpnSpeed + 
                    "\nDamage: " + newStats.wpnDamage.ToString("F1") + damageComparisonText + 
                    "\nKnockback: " + newStats.wpnKnockback.ToString("F1") + knockbackComparisonText, 
                    "Press "+buttonText+" to Swap"
                    );

                if (swapItem.triggered)
                {
                    SoundEffectManager.Instance.PlaySound("Pickup", transform.position);

                    curWeapon.transform.position = weapon.position;
                    curWeapon.GetComponent<Collider>().enabled = true;
                    weapon.position = weaponHolder.position;
                    curWeapon.layer = LayerMask.NameToLayer("Weapon");
                    curWeapon.transform.parent = null;
                    curWeapon.tag = "Weapon";
                    weapon.gameObject.layer = LayerMask.NameToLayer("currentWeapon");
                    weapon.GetComponent<Collider>().enabled = false;
                    weapon.tag = "currentWeapon";
                    curWeapon = GameObject.FindWithTag("currentWeapon");
                    curWeapon.transform.parent = weaponHolder.transform;
                    curWeapon.transform.rotation = weaponHolder.transform.rotation;

                    Vector3 positionOffset = curWeapon.GetComponent<WeaponStats>().holdPositionOffset;
                    curWeapon.transform.localPosition = positionOffset;
                    Vector3 rotationOffset = curWeapon.GetComponent<WeaponStats>().holdRotationOffset;
                    weaponHolder.transform.localEulerAngles = rotationOffset;

                    TooltipManager.Instance.DestroyTooltip();
                }
            }
            else if (distanceToWeapon > 3f && distanceToWeapon < 5f || !playerController.canAct)
            {
                TooltipManager.Instance.DestroyTooltip();
            }
        }
    }
    /*public void UpdateCharacter(GameObject currentPlayer)
    {
        currentCharacter = currentPlayer;
        Transform[] playerChildren = currentCharacter.GetComponentsInChildren<Transform>();
        foreach (Transform child in playerChildren)
        {
            if(child.tag == "WeaponSlot")
            {
                weaponHolder = child;
            }
        }
        curWeapon = GameObject.FindWithTag("currentWeapon");
        curWeapon.transform.parent = weaponHolder.transform.parent;
        DontDestroyOnLoad(curWeapon);
    }*/
}
