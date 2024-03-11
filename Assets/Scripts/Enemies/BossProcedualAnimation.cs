using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProcedualAnimation : MonoBehaviour
{
    TempMovement tempMovement;

    [SerializeField] private Transform leftArmTarget;
    [SerializeField] private Transform rightArmTarget;

    [SerializeField] private Transform head;
    [SerializeField] private float headSpeed;
    [SerializeField] private float delayTime;
    [SerializeField] private float maxRotation;

    private bool playerIsRight;
    private bool playerIsLeft;
    private bool isTurningHead;

    // Start is called before the first frame update
    void Start()
    {
        tempMovement = GetComponent<TempMovement>();
        playerIsRight = tempMovement.playerIsRight;
        playerIsLeft = tempMovement.playerIsLeft;
        // leftArmTarget.position = new Vector3(-2, -1, -4);
        // leftArmTarget.rotation = Quaternion.Euler(2, 88, -159);
        // rightArmTarget.position = new Vector3(8, -1, -4);
        // rightArmTarget.rotation = Quaternion.Euler(2, 88, -159);
    }

    void Update()
    {
        playerIsRight = tempMovement.playerIsRight;
        if (playerIsRight && !isTurningHead)
        {
            StartCoroutine(TurnHeadRight(delayTime));
        }
        else if (!playerIsRight)
        {
            StopCoroutine(TurnHeadRight(delayTime));
            isTurningHead = false;
        }

        playerIsLeft = tempMovement.playerIsLeft;
        if (playerIsLeft && !isTurningHead)
        {
            StartCoroutine(TurnHeadLeft(delayTime));
        }
        else if (!playerIsLeft)
        {
            StopCoroutine(TurnHeadLeft(delayTime));
            isTurningHead = false;
        }
    }

    IEnumerator TurnHeadRight(float delay)
    {
        yield return new WaitForSeconds(delay);
        isTurningHead = true;

        while (playerIsRight)
        {
            Vector3 directionToPlayer = (tempMovement.player.position - head.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);
            float clampedYRotation = Mathf.Clamp(targetRotation.eulerAngles.y, head.eulerAngles.y - maxRotation, head.eulerAngles.y + maxRotation);
            head.rotation = Quaternion.Euler(head.eulerAngles.x, clampedYRotation, head.eulerAngles.z);
            yield return null;
        }

        isTurningHead = false;
    }

    IEnumerator TurnHeadLeft(float delay)
    {
        yield return new WaitForSeconds(delay);
        isTurningHead = true;

        while (playerIsLeft)
        {
            Vector3 directionToPlayer = (tempMovement.player.position - head.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);
            float clampedYRotation = Mathf.Clamp(targetRotation.eulerAngles.y, head.eulerAngles.y - maxRotation, head.eulerAngles.y + maxRotation);
            head.rotation = Quaternion.Euler(head.eulerAngles.x, clampedYRotation, head.eulerAngles.z);
            yield return null;
        }

        isTurningHead = false;
    }
}
