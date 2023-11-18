using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingCyclone : Skill
{
    [SerializeField] private float extendFreezePoint = 0.5f;
    [SerializeField] private float baseSpinDuration = 1f;
    [SerializeField] private float speedAnimationScalar = 1f;

    private GameObject currentWeapon;
    private GameObject player;

    public override void DoSkill()
    {
        player = GameObject.FindWithTag("currentPlayer");
        playerController.EnableController();
        currentWeapon = GameObject.FindWithTag("currentWeapon");

        StartCoroutine(ExtendArm());
    }

    IEnumerator ExtendArm()
    {
        float storedAnimatorSpeed = currentAnimator.speed;
        currentAnimator.Play("Slash", 0, 0f);

        yield return null;

        while (currentAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < extendFreezePoint)
        {
            yield return null;
        }

        currentAnimator.speed = 0;
        playerController.looking = false;
        yield return StartCoroutine(Spin());
        yield return StartCoroutine(Spin());
        yield return StartCoroutine(Spin());
        playerController.looking = true;

        currentAnimator.speed = storedAnimatorSpeed;

        yield return new WaitUntil (() => !currentAnimator.GetCurrentAnimatorStateInfo(0).IsName("Slash"));
        playerController.canAct = true;
    }

    IEnumerator Spin()
    {
        currentWeapon.GetComponent<Collider>().enabled = true;
        currentWeapon.GetComponent<NewWeaponCollision>().ClearEnemyList();

        float spinCounter = 0f;
        float spinDuration = baseSpinDuration / (characterStats.animatorSpeed * speedAnimationScalar);
        while (spinCounter < spinDuration)
        {
            float rotationAmount = 360f / spinDuration * Time.deltaTime;
            player.transform.Rotate(Vector3.down, rotationAmount);
            
            spinCounter += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        currentWeapon.GetComponent<Collider>().enabled = false;
    }
}
