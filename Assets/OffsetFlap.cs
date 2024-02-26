using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetFlap : MonoBehaviour
{
    [SerializeField] private float WaitTime;
    [SerializeField] private Animator animator;
    void Start()
    {
        StartCoroutine(WaitForFlap());
    }

    IEnumerator WaitForFlap()
    {
        yield return new WaitForSeconds(WaitTime);
        animator.SetBool("Flapping", true);
    }
}
