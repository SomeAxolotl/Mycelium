using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDangerZone : MonoBehaviour
{
    private BossAnimationEvents bossAnimEvents;

    // Start is called before the first frame update
    void Start()
    {
        bossAnimEvents = GameObject.Find("Rival Sporemother").GetComponent<BossAnimationEvents>(); ;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "currentPlayer")
        {
            bossAnimEvents.isInDangerZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "currentPlayer")
        {
            bossAnimEvents.isInDangerZone = false;
        }
    }
}
