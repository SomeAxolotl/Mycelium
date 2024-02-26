using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLevelGeneration : MonoBehaviour
{
    void Start()
    {
        GameObject.Find("PlayerParent").GetComponent<SwapWeapon>().currentCharacter = GameObject.FindWithTag("currentPlayer");

        Transform[] playerChildren = GameObject.FindWithTag("currentPlayer").GetComponentsInChildren<Transform>();
        foreach (Transform child in playerChildren)
        {
            if (child.tag == "WeaponSlot")
            {
                GameObject.Find("PlayerParent").GetComponent<SwapWeapon>().weaponHolder = child;
            }
        }

        if (GameObject.FindWithTag("currentWeapon") == null)
        {
            GameObject startingWeapon = Instantiate(Resources.Load("Weapons/StartWeapon"), GameObject.FindWithTag("WeaponSlot").transform) as GameObject;
            startingWeapon.layer = LayerMask.NameToLayer("currentWeapon");
            startingWeapon.GetComponent<Collider>().enabled = false;
            GameObject.Find("PlayerParent").GetComponent<SwapWeapon>().curWeapon = startingWeapon;
        }
    }

    private void spawnPlayer(Transform spawnpoint)
    {
        GameObject player = GameObject.FindGameObjectWithTag("currentPlayer");
        player.transform.position = spawnpoint.position;
        player.transform.rotation = spawnpoint.rotation;
    }
}
