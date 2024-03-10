using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProcedualAnimation : MonoBehaviour
{
    TempMovement tempMovement;

    [SerializeField] private Transform leftArmTarget;
    [SerializeField] private Transform rightArmTarget;

    [SerializeField] private Transform leftArmTargetRig;
    [SerializeField] private Transform rightArmTargetRig;

    [SerializeField] private Transform head;
    [SerializeField] private float headDuration;
    [SerializeField] private float headSpeed;
    private float elapsedHeadTime = 0f;
    private float lastHeadLocationY;

    // Start is called before the first frame update
    void Start()
    {
        tempMovement = GetComponent<TempMovement>();
        leftArmTarget.position = new Vector3(-2, -1, -4);
        leftArmTarget.rotation = Quaternion.Euler(2, 88, -159);
        rightArmTarget.position = new Vector3(8, -1, -4);
        rightArmTarget.rotation = Quaternion.Euler(2, 88, -159);
        lastHeadLocationY = head.rotation.eulerAngles.y;
    }

    void Update()
    {
        if (head.rotation.eulerAngles.y != lastHeadLocationY)
        {
            lastHeadLocationY = head.rotation.eulerAngles.y;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (tempMovement.playerIsRight)
        {
            // move head
            if (elapsedHeadTime < headDuration)
            {
                float rotationHeadAmount = headSpeed * Time.deltaTime;
                float headRotationY = lastHeadLocationY + rotationHeadAmount;
                head.rotation = Quaternion.Euler(head.transform.rotation.eulerAngles.x, headRotationY, head.transform.rotation.eulerAngles.z);
                elapsedHeadTime += Time.deltaTime;
            }

            /*
            1.	Move Headfirst as An Indicator
                •	Turn head to desired direction.
                        x+15 degrees
                •	Lean body forward
                    X = 46

            2.	Move Right Arm
                •	Raise top of arm in upside down L shape
                •	Move to desired position.
                •	Place on floor
                •	Bend arm
            3.	Move Left Arm
                •	Raise top of arm in upside down L shape
                •	Move to desired position.
                •	Place on floor
                •	Bend arm
            4.	Lift Body
                •	Straighten both arms.
                •	Lift body
                •	Turn body to desired direction.
                •	Drop onto floor. 
                •	Straighten
            */
        }
        else
            elapsedHeadTime = 0f;

        if (tempMovement.playerIsLeft)
        {
            elapsedHeadTime = 0f;
            // move head
            if (elapsedHeadTime < headDuration)
            {
                float rotationHeadAmount = headSpeed * Time.deltaTime;
                float headRotationY = lastHeadLocationY - rotationHeadAmount;
                head.rotation = Quaternion.Euler(head.transform.rotation.eulerAngles.x, headRotationY, head.transform.rotation.eulerAngles.z);
                elapsedHeadTime += Time.deltaTime;
            }

            /*
            1.	Move Headfirst as An Indicator
                •	Turn head to desired direction.
                        x+15 degrees
                •	Lean body forward
                    X = 46

            2.	Move Right Arm
                •	Raise top of arm in upside down L shape
                •	Move to desired position.
                •	Place on floor
                •	Bend arm
            3.	Move Left Arm
                •	Raise top of arm in upside down L shape
                •	Move to desired position.
                •	Place on floor
                •	Bend arm
            4.	Lift Body
                •	Straighten both arms.
                •	Lift body
                •	Turn body to desired direction.
                •	Drop onto floor. 
                •	Straighten
            */
        }
        else
            elapsedHeadTime = 0f;

        /*
        Right Arm Swing Attack
            1.	Left arm does not move.
            2.	Right to left swing.
        Left Arm Swing Attack
            1.	Right arm does not move.
            2.	Left to right swing.
        Smash Attack
            1.	Raise both arms.
            2.	Bring down with force.
        Tail Attack
            1.	Pops out of floor.
        */
    }
}
