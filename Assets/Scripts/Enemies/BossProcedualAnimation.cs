using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProcedualAnimation : MonoBehaviour
{
    TempMovement tempMovement;

    [SerializeField] private Transform leftArmTarget;
    [SerializeField] private Transform rightArmTarget;

    // [SerializeField] private Transform leftArmTargetRig;
    // [SerializeField] private Transform rightArmTargetRig;

    [SerializeField] private Transform head;
    // [SerializeField] private float headDuration;
    [SerializeField] private float headSpeed;
    [SerializeField] private float delayTime;
    [SerializeField] private float maxRotation;
    // private float elapsedHeadTime = 0f;
    // private float lastHeadLocationY;

    private bool playerIsRight;
    private bool playerIsLeft;
    private bool isTurningHead;

    // Start is called before the first frame update
    void Start()
    {
        tempMovement = GetComponent<TempMovement>();
        playerIsRight = tempMovement.playerIsRight;
        playerIsLeft = tempMovement.playerIsLeft;
        leftArmTarget.position = new Vector3(-2, -1, -4);
        leftArmTarget.rotation = Quaternion.Euler(2, 88, -159);
        rightArmTarget.position = new Vector3(8, -1, -4);
        rightArmTarget.rotation = Quaternion.Euler(2, 88, -159);
        // lastHeadLocationY = head.rotation.eulerAngles.y;
    }

    void Update()
    {
        // if (head.rotation.eulerAngles.y != lastHeadLocationY)
        // {
        //     lastHeadLocationY = head.rotation.eulerAngles.y;
        // }
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


    // Update is called once per frame
    // void FixedUpdate()
    // {
    //     if (tempMovement.playerIsRight)
    //     {
    //         // move head
    //         if (elapsedHeadTime < headDuration)
    //         {
    //             float rotationHeadAmount = headSpeed * Time.deltaTime;
    //             float headRotationY = lastHeadLocationY + rotationHeadAmount;
    //             head.rotation = Quaternion.Euler(head.transform.rotation.eulerAngles.x, headRotationY, head.transform.rotation.eulerAngles.z);
    //             elapsedHeadTime += Time.deltaTime;
    //         }

    //         /*
    //         1.	Move Headfirst as An Indicator
    //             •	Turn head to desired direction.
    //                     x+15 degrees
    //             •	Lean body forward
    //                 X = 46

    //         2.	Move Right Arm
    //             •	Raise top of arm in upside down L shape
    //             •	Move to desired position.
    //             •	Place on floor
    //             •	Bend arm
    //         3.	Move Left Arm
    //             •	Raise top of arm in upside down L shape
    //             •	Move to desired position.
    //             •	Place on floor
    //             •	Bend arm
    //         4.	Lift Body
    //             •	Straighten both arms.
    //             •	Lift body
    //             •	Turn body to desired direction.
    //             •	Drop onto floor. 
    //             •	Straighten
    //         */
    //     }
    //     else
    //         elapsedHeadTime = 0f;

    //     if (tempMovement.playerIsLeft)
    //     {
    //         elapsedHeadTime = 0f;
    //         // move head
    //         if (elapsedHeadTime < headDuration)
    //         {
    //             float rotationHeadAmount = headSpeed * Time.deltaTime;
    //             float headRotationY = lastHeadLocationY - rotationHeadAmount;
    //             head.rotation = Quaternion.Euler(head.transform.rotation.eulerAngles.x, headRotationY, head.transform.rotation.eulerAngles.z);
    //             elapsedHeadTime += Time.deltaTime;
    //         }

    //         /*
    //         1.	Move Headfirst as An Indicator
    //             •	Turn head to desired direction.
    //                     x+15 degrees
    //             •	Lean body forward
    //                 X = 46

    //         2.	Move Right Arm
    //             •	Raise top of arm in upside down L shape
    //             •	Move to desired position.
    //             •	Place on floor
    //             •	Bend arm
    //         3.	Move Left Arm
    //             •	Raise top of arm in upside down L shape
    //             •	Move to desired position.
    //             •	Place on floor
    //             •	Bend arm
    //         4.	Lift Body
    //             •	Straighten both arms.
    //             •	Lift body
    //             •	Turn body to desired direction.
    //             •	Drop onto floor. 
    //             •	Straighten
    //         */
    //     }
    //     else
    //         elapsedHeadTime = 0f;

    //     /*
    //     Right Arm Swing Attack
    //         1.	Left arm does not move.
    //         2.	Right to left swing.
    //     Left Arm Swing Attack
    //         1.	Right arm does not move.
    //         2.	Left to right swing.
    //     Smash Attack
    //         1.	Raise both arms.
    //         2.	Bring down with force.
    //     Tail Attack
    //         1.	Pops out of floor.
    //     */
    // }
}
