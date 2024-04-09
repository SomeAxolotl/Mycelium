using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using UnityEngine.EventSystems;

public class TempMovement : MonoBehaviour
{
    private Transform player;
    private RigBuilder rigBuilder;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("currentPlayer").transform;
        animator = GetComponent<Animator>();
        rigBuilder = GetComponent<RigBuilder>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        Vector3 dirToPlayer = (transform.position - player.position).normalized;
        Quaternion desiredRotation = Quaternion.LookRotation(dirToPlayer);
        float desiredYRotation = desiredRotation.eulerAngles.y + 180f;
        Quaternion targetRotation = Quaternion.Euler(0f, desiredYRotation, 0f);
        float angleToTarget = Quaternion.Angle(transform.rotation, targetRotation);
        float maxAngleThisFrame = 1f * Time.fixedDeltaTime;

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, maxAngleThisFrame);

        float angletoPlayer = Vector3.Angle(transform.forward, player.position);

        if (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f && angletoPlayer > 10f)
        {
            rigBuilder.enabled = false;
            float yRotationDifference = Quaternion.Angle(transform.rotation, targetRotation);
            if (yRotationDifference > 0f)
            {
                animator.SetTrigger("TurnLeft");
                Debug.Log("Turning left");
            }
            else if (yRotationDifference < 0f)
            {
                animator.SetTrigger("TurnRight");
                Debug.Log("Turning right");
            }
            else
            {
                rigBuilder.enabled = true;
            }
        }
        else
        {
            rigBuilder.enabled = true;
        }
    }
}
