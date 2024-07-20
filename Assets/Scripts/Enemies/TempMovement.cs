using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TempMovement : MonoBehaviour
{
    Transform player;
    Animator animator;
    MonsterBossAttack monsterBossAttack;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("currentPlayer").transform;
        animator = GetComponent<Animator>();
        monsterBossAttack = GetComponent<MonsterBossAttack>();
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
        float maxAngleThisFrame = 2f * Time.fixedDeltaTime;

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") == true)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, maxAngleThisFrame);
        }

        /*float angletoPlayer = Vector3.Angle(transform.forward, player.position);
        //Debug.DrawRay(transform.position, transform.forward, Color.green, 2f);
        Debug.Log("angletoplayer - " + angletoPlayer);
        Debug.Log("angletotarget - " + angleToTarget);
        if (angletoPlayer > 10f && !monsterBossAttack.isAttacking)
        {
            float yRotationDifference = angleToTarget;
            if (yRotationDifference > 0f)
            {
                animator.SetTrigger("TurnRight");
            }
            else if (yRotationDifference < 0f)
            {
                animator.SetTrigger("TurnLeft");
            }
        }*/

        /*Vector3 relativeDirToPlayer = transform.InverseTransformDirection(dirToPlayer);
        float yRotationDifference = Mathf.Atan2(relativeDirToPlayer.x, relativeDirToPlayer.z) * Mathf.Rad2Deg;

        if (angleToTarget > 0.1f && Mathf.Abs(yRotationDifference) > 10f && !monsterBossAttack.isAttacking)
        {
            if (yRotationDifference > 0f)
            {
                animator.SetTrigger("TurnLeft");
            }
            else
            {
                animator.SetTrigger("TurnRight");
            }
        }*/
    }
}
