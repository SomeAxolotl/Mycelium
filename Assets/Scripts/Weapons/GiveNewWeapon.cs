using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveNewWeapon : MonoBehaviour
{
    [HideInInspector] public GameObject player;
    [HideInInspector] public GameObject playerParent;
    [HideInInspector] public SwapWeapon swap;
    //player = GameObject.FindWithTag("currentPlayer");
    public void NewWeapon(){
        playerParent = player.transform.parent.gameObject;
        swap = playerParent.GetComponent<SwapWeapon>();
        //Weapon you want to give goes in the first part
        GameObject randomWeapon = Instantiate(Resources.Load("Slash/Stick"), GameObject.FindWithTag("WeaponSlot").transform) as GameObject;
        if(randomWeapon.GetComponent<WeaponStats>().wpnName != "Stick"){
            randomWeapon.transform.localScale = randomWeapon.transform.localScale / 0.03f / 100f / 1.2563f;
        }
        randomWeapon.GetComponent<WeaponStats>().acceptingAttribute = false;
        AttributeAssigner.Instance.PickAttFromString(randomWeapon, "Enigmatic");
        randomWeapon.layer = LayerMask.NameToLayer("currentWeapon");
        randomWeapon.GetComponent<Collider>().enabled = false;
        gameObject.tag = "Weapon";
        randomWeapon.tag = "currentWeapon";
        StartCoroutine(SetPreviousWeaponStats(randomWeapon));
    }

    //Has to wait a frame for things to initialize
    IEnumerator SetPreviousWeaponStats(GameObject randomWeapon){
        //Next frame do these things
        yield return 0;
        Destroy(gameObject);
        randomWeapon.GetComponent<WeaponInteraction>().ApplyWeaponPositionAndRotation();
        randomWeapon.GetComponent<Enigmatic>().Equipped();
    }
}
