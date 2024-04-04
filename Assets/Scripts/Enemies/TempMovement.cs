using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class TempMovement : MonoBehaviour
{
    private Transform player;
    private MonsterBossAttack monsterBossAttack;
    Animator animator;
    float currentYRot;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("currentPlayer").transform;
        monsterBossAttack = GetComponent<MonsterBossAttack>();
        animator = GetComponent<Animator>();
        currentYRot = transform.rotation.y;
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
        //float fractionOfTurn = Mathf.Min(maxAngleThisFrame / angleToTarget, 1f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, maxAngleThisFrame);

        float angletoPlayer = Vector3.Angle(transform.forward, player.position);
        currentYRot = transform.rotation.y;

        if(transform.rotation.y > currentYRot && angletoPlayer > 10f)
        {
            animator.Play("LeftTurn");
        }
        else if(transform.rotation.y < currentYRot && angletoPlayer > 10f)
        {
            animator.Play("RightTurn");
        }
    }
}
