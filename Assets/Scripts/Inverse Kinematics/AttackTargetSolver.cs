using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTargetSolver : MonoBehaviour
{
    [SerializeField] Transform armTarget;

    private Transform player;
    private Transform boss;
    private Animator bossAnimator;

    void Start()
    {
        player = GameObject.FindWithTag("currentPlayer").GetComponent<Transform>();
        boss = GameObject.Find("Rival Colony Leader").GetComponent<Transform>();
        bossAnimator = GameObject.Find("Rival Colony Leader").GetComponent<Animator>();
    }

    public void GoToPlayer()
    {
        transform.position = player.position + (Vector3.up * 0.38f);
        StartCoroutine(FollowArmHeight());
    }

    public void GoToPlayerSmash(bool isLeft)
    {
        if(isLeft == true)
        {
            transform.position =  player.position + (Quaternion.Euler(0, boss.eulerAngles.y, 0) * (Vector3.left * 1f)) + (Vector3.up * 0.38f);
        }
        else
        {
            transform.position = player.position + (Quaternion.Euler(0, boss.eulerAngles.y, 0) * (Vector3.right * 1f)) + (Vector3.up * 0.38f);
        }

        StartCoroutine(FollowArmHeight());
    }

    public void StartFollowArm()
    {
        StartCoroutine(FollowArm());
    }

    private IEnumerator FollowArm()
    {
        while (bossAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") == false)
        {
            transform.position = armTarget.position;
            yield return null;
        }
    }

    private IEnumerator FollowArmHeight()
    {
        while (bossAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") == false)
        {
            transform.position = new Vector3(transform.position.x, armTarget.position.y, transform.position.z);
            yield return null;
        }
    }
}
