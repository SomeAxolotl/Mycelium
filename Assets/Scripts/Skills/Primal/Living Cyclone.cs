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

        if(GameObject.FindWithTag("currentWeapon") != null) 
        {
            currentWeapon = GameObject.FindWithTag("currentWeapon");
        }

        StartCoroutine(ExtendArm());
    }

    IEnumerator ExtendArm()
    {
        //string animationName = currentWeapon.GetComponent<WeaponStats>().weaponType.ToString();

        float storedAnimatorSpeed = currentAnimator.speed;
        currentAnimator.Play("Slash", 0, 0f);

        yield return null;

        yield return new WaitUntil (() => currentAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > extendFreezePoint);

        currentAnimator.speed = 0;
        if (isPlayerCurrentPlayer())
        {
            playerController.looking = false;
        }
        
        if (isPlayerCurrentPlayer())
        {
            currentWeapon.GetComponent<WeaponCollision>().isCycloning = true;
        }
        yield return StartCoroutine(Spin());
        yield return StartCoroutine(Spin());
        yield return StartCoroutine(Spin());
        if (isPlayerCurrentPlayer())
        {
            currentWeapon.GetComponent<WeaponCollision>().isCycloning = false;
        }
        
        if (isPlayerCurrentPlayer())
        {
            playerController.looking = true;
        }
        
        currentAnimator.speed = characterStats.animatorSpeed;

        yield return new WaitUntil (() => !currentAnimator.GetCurrentAnimatorStateInfo(0).IsName("Slash"));

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
