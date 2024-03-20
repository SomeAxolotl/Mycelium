using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TidepoolSlow : MonoBehaviour
{
    [SerializeField] float tidepoolMoveSpeedModifier = 0.65f;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "currentPlayer")
        {
            other.gameObject.GetComponent<CharacterStats>().moveSpeed *= tidepoolMoveSpeedModifier;
            GameObject.Find("PlayerParent").GetComponent<PlayerController>().GetStats();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "currentPlayer")
        {
            other.gameObject.GetComponent<CharacterStats>().moveSpeed /= tidepoolMoveSpeedModifier;
            GameObject.Find("PlayerParent").GetComponent<PlayerController>().GetStats();
        }
    }
}
