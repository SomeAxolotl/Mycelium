using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
public class GrowObjectManager : MonoBehaviour
{
    float distance;
    ThirdPersonActionAsset playerActionAsset;
    private InputAction interact;
    GameObject player;
    private void Start()
    {
        playerActionAsset = new ThirdPersonActionAsset();
        playerActionAsset.PlayerEnable();
        interact = playerActionAsset.Player.Interact;
        player = GameObject.FindwithTag("currentPlayer")
    }

    // Update is called once per frame
    private void Update()
    {
        distance = Vector3.Distance(player.transform.position, this.Transform.position);
        if(distance < 2)
        {
            
        }
    }
}
