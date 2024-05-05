using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingCyclone : Skill
{
    [SerializeField] private float extendFreezePoint = 0.5f;
    [SerializeField] private float baseSpinDuration = 1f;
    [SerializeField] private float speedAnimationScalar = 1f;

    private GameObject currentWeapon;

    int spinNumber = 0;

    public override void DoSkill()
    {   
        spinNumber = 0;

        if (isPlayerCurrentPlayer())
        {
            playerController.EnableController();
            playerController.canAct = false;
        }

        currentWeapon = GameObject.FindWithTag("currentWeapon");

        StartCoroutine(ExtendArm());
    }

    IEnumerator ExtendArm()
    {
        if (isPlayerCurrentPlayer())
        {
            if (currentWeapon != null)
            {
                currentWeapon.GetComponent<WeaponCollision>().isCycloning = true;
            }
        }

        float storedAnimatorSpeed = currentAnimator.speed;
        currentAnimator.Play("Slash", 0, 0f);

        yield return null;

        yield return new WaitUntil (() => currentAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > extendFreezePoint);

        if (isPlayerCurrentPlayer())
        {
            playerController.looking = false;
        }
        currentAnimator.speed = 0;

        yield return StartCoroutine(Spin());
        yield return StartCoroutine(Spin());
        yield return StartCoroutine(Spin());

        if (isPlayerCurrentPlayer())
        {
            playerController.looking = true;
            if (currentWeapon != null)
            {
                currentWeapon.GetComponent<WeaponCollision>().isCycloning = false;
            }
        }
        
        currentAnimator.speed = characterStats.animatorSpeed;

        EndSkill();
    }

    IEnumerator Spin()
    {
        spinNumber++;

        if(currentWeapon != null) 
        {
            WeaponCollision weaponCollision = currentWeapon.GetComponent<WeaponCollision>();

            if (spinNumber > 1)
            {
                SoundEffectManager.Instance.PlaySound("Slash", player.transform.position);
            }
            
            weaponCollision.ClearEnemyList();
            currentWeapon.GetComponent<Collider>().enabled = true;
            weaponCollision.sentienceBonusDamage = finalSkillValue;

            float spinCounter = 0f;
            float spinDuration = baseSpinDuration / (characterStats.animatorSpeed * speedAnimationScalar);
            while (spinCounter < spinDuration)
            {
                float rotationAmount = 360f / spinDuration * Time.deltaTime;
                if (!weaponCollision.hitStopping)
                {
                    player.transform.Rotate(Vector3.down, rotationAmount);
                    spinCounter += Time.deltaTime;
                }

                yield return new WaitForFixedUpdate();
            }
            currentWeapon.GetComponent<Collider>().enabled = false;
            weaponCollision.sentienceBonusDamage = 0f;
        }
        else
        {
            float spinCounter = 0f;
            float spinDuration = baseSpinDuration / (characterStats.animatorSpeed * speedAnimationScalar);
            while (spinCounter < spinDuration)
            {
                float rotationAmount = 360f / spinDuration * Time.deltaTime;
                player.transform.Rotate(Vector3.down, rotationAmount);

                spinCounter += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
