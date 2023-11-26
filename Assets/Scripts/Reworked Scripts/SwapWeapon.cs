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
    GameObject currentCharacter;
    Transform weaponHolder;
    SwapCharacter swapCharacter;
    public GameObject curWeapon;

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
        if(GameObject.FindWithTag("currentWeapon") == null)
        {
            GameObject startingWeapon = Instantiate(Resources.Load("StartWeapon"), GameObject.FindWithTag("WeaponSlot").transform) as GameObject;
            UpdateCharacter(GameObject.FindWithTag("currentPlayer"));
            startingWeapon.layer = LayerMask.NameToLayer("currentWeapon");
        }
    }

    // Update is called once per frame
    void Update()
    {
        curWeapon.transform.position = weaponHolder.transform.position;
        curWeapon.transform.rotation = weaponHolder.transform.rotation;
      
        weaponColliders = Physics.OverlapSphere(currentCharacter.transform.position, 4f, weaponLayer);
        foreach (var weaponCollider in weaponColliders)
        {
            weapon = weaponCollider.transform;
            Vector3 dirToWeapon = (weapon.position - currentCharacter.transform.position).normalized;
            //nesting so i can use tooltips
            if (Vector3.Angle(currentCharacter.transform.forward, dirToWeapon) <= 40 || Vector3.Distance(currentCharacter.transform.position, weapon.position) <= 1f)
            {
                string damageComparisonText;
                //[TYLER ELP]
                //if newWeaponDamage > oldWeaponDamage
                if (10 > 5)
                {
                    damageComparisonText = "<color=#" + ColorUtility.ToHtmlStringRGB(betterStatColor) + "> +</color>";
                }
                //[TYLER ELP]
                //if newWeaponDamage < oldWeaponDamage
                else if (10 < 5)
                {
                    damageComparisonText = "<color=#" + ColorUtility.ToHtmlStringRGB(worseStatColor) + "> -</color>";
                }
                else
                {
                    damageComparisonText = "<color=#" + ColorUtility.ToHtmlStringRGB(evenStatColor) + "> +-</color>";
                }
                string speedComparisonText;
                //[TYLER ELP]
                //if newWeaponSpeed > oldWeaponSpeed
                if (2 > 4)
                {
                    speedComparisonText = "<color=#" + ColorUtility.ToHtmlStringRGB(betterStatColor) + "> +</color>";
                }
                //[TYLER ELP]
                //if newWeaponSpeed < oldWeaponSpeed
                else if (2 < 4)
                {
                    speedComparisonText = "<color=#" + ColorUtility.ToHtmlStringRGB(worseStatColor) + "> -</color>";
                }
                else
                {
                    speedComparisonText = "<color=#" + ColorUtility.ToHtmlStringRGB(evenStatColor) + "> +-</color>";
                }

                //[TYLER ELP]
                string weaponName = "[WeaponName]";
                TooltipManager.Instance.CreateTooltip(weapon.gameObject, weaponName, "Damage: " + 5 + damageComparisonText + "\nSpeed: " + 10 + speedComparisonText, "Swap");

                if (swapItem.triggered)
                {
    
                    //Debug.Log("swap");
                    curWeapon.transform.position = weapon.position;
                    // curWeapon.transform.rotation = Quaternion.Euler(-25, 0, 0);
                    curWeapon.GetComponent<Collider>().enabled = true;
                    weapon.position = weaponHolder.position;
                    curWeapon.layer = LayerMask.NameToLayer("Weapon");
                    curWeapon.transform.parent = null;
                    curWeapon.tag = "Weapon";
                    weapon.gameObject.layer = LayerMask.NameToLayer("currentWeapon");
                    weapon.GetComponent<Collider>().enabled = false;
                    weapon.tag = "currentWeapon";
                    curWeapon.transform.parent = weaponHolder.transform.parent;
                    curWeapon = GameObject.FindWithTag("currentWeapon");

                    TooltipManager.Instance.DestroyTooltip();
                }
            }
            else
            {
                TooltipManager.Instance.DestroyTooltip();
            }
        }
        //Debug.Log(weaponHolder.name);
    }
    public void UpdateCharacter(GameObject currentPlayer)
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
    }
    /*void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(currentCharacter.transform.position, 5f);
    }*/
}
