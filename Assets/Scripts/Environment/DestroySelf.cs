using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class DestroySelf : MonoBehaviour
{
    public void InitiateSelfDestructSequence()
    {
        StartCoroutine(SelfDestruct());
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);
    }
}
