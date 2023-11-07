using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStopManager : MonoBehaviour
{
    [SerializeField] float stopDurationScalar = 0.05f;

    public static HitStopManager Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void HitStop(GameObject obj, float damage)
    {
        HitStopCoroutine(obj, damage);
    }

    IEnumerator HitStopCoroutine(GameObject obj, float damage)
    {
        /*float stopDuration = GetHitStopDuration(damage);
        Vector3 stopPosition = obj.transform.position;

        float stopCounter = 0f;
        while (stopCounter <= stopDuration)
        {
            obj.transform.position = stopPosition;
            stopCounter += Time.deltaTime;
            yield return new WaitForSeconds(0.1f);
        }*/
        yield return null;
    }

    public void HitSlow(GameObject obj, float damage)
    {
        Debug.Log("hitslow");
        HitSlowCoroutine(obj, damage);
    }

    IEnumerator HitSlowCoroutine(GameObject obj, float damage)
    {
        float slowDuration = GetHitStopDuration(damage);
        float newAnimatorSpeed = GetHitStopDuration(damage);

        Animator animator = obj.GetComponent<Animator>();
        float oldAnimatorSpeed = animator.speed;
        animator.speed = newAnimatorSpeed;
        yield return new WaitForSeconds(slowDuration);
        animator.speed = oldAnimatorSpeed;
    }

    public float GetHitStopDuration(float damage)
    {
        return damage * stopDurationScalar;
    }
}
