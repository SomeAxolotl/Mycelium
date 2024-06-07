using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationEvents : MonoBehaviour
{
    [SerializeField] private AttackTargetSolver leftATS;
    [SerializeField] private AttackTargetSolver rightATS;

    [SerializeField] private Transform leftShoulder;
    [SerializeField] private Transform rightShoulder;

    [HideInInspector] public bool isInDangerZone = false;

    private Transform player;

    private void Start()
    {
        player = GameObject.FindWithTag("currentPlayer").GetComponent<Transform>();
    }

    void Finish() //This is called in the Death animation by an animation event -ryan
    {
        GameObject.Find("CreditsPlayer").GetComponent<CreditsPlayer>().StartPlayCredits();
        GameObject boss = GameObject.Find("Rival Colony Leader");
        ParticleManager.Instance.SpawnParticleFlurry("NutrientParticles", GameObject.Find("Rival Colony Leader").GetComponent<BossHealth>().nutrientDrop, 0.1f, this.gameObject.transform.position, Quaternion.Euler(-90f, 0f, 0f));
        //boss.SetActive(false);
    }

    void PositionAttackTarget(string attack) //This is called in the various attack animations through animation events -ryan
    {
        //Debug.Log($"Danger Zone: {isInDangerZone}");
        //Debug.Log($"Left Dist: {Vector3.Distance(leftShoulder.position, player.position)} | Right Dist: {Vector3.Distance(rightShoulder.position, player.position)}");

        switch (attack)
        {
            case "left":
                if(isInDangerZone == true && Vector3.Distance(leftShoulder.position, player.position) < 8.8f)
                {
                    leftATS.GoToPlayer();
                }
                else
                {
                    leftATS.StartFollowArm();
                }
                break;

            case "right":
                if (isInDangerZone == true && Vector3.Distance(rightShoulder.position, player.position) < 10.4f)
                {
                    rightATS.GoToPlayer();
                }
                else
                {
                    rightATS.StartFollowArm();
                }
                break;

            case "smash":
                if (isInDangerZone == true && Vector3.Distance(leftShoulder.position, player.position) < 10.9f && Vector3.Distance(rightShoulder.position, player.position) < 11.1f)
                {
                    leftATS.GoToPlayerSmash(true);
                    rightATS.GoToPlayerSmash(false);
                }
                else
                {
                    leftATS.StartFollowArm();
                    rightATS.StartFollowArm();
                }
                break;

            default:
                break;
        }
    }
}