using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startRunTrigger : MonoBehaviour
{
    public PlayerController playerController;
  
    [SerializeField]
    GameObject UIEnable;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("currentPlayer"))
        {
           playerController = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>();
           playerController.DisableController();
           UIEnable.SetActive(true);
        }
    }
}
