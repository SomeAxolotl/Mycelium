using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IKBossArmSolver : MonoBehaviour
{
    private const int RAY_DIST = 20;

    [SerializeField] private LayerMask raycastLayer;
    [SerializeField] private Transform boss;

    private Ray collisionRay;
    private RaycastHit hit;

    private Vector3 currentPosition;
    private float rayOffsetDist;
    private float rayAngle;

    private Animator bossAnimator;

    void Start()
    {
        bossAnimator = boss.GetComponent<Animator>();

        currentPosition = transform.position;
        rayOffsetDist = Vector3.Distance(transform.position, boss.position);
        Debug.Log((transform.position - boss.position).normalized, this);

        rayAngle = TriangleMathStuff();
    }

    void Update()
    {
        if (bossAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") == false)
        {
            return;
        }

        collisionRay = new Ray(boss.position + (rayOffsetDist * CalcDirection() + (Vector3.up * 10)), Vector3.down);

        if (Physics.Raycast(collisionRay, out hit, RAY_DIST, raycastLayer, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawRay(collisionRay.origin, collisionRay.direction * RAY_DIST, Color.blue, 0f, false);
        }
        else
        {
            Debug.DrawRay(collisionRay.origin, collisionRay.direction * RAY_DIST, Color.red, 0f, false);
        }

        transform.position = currentPosition;

    }

    Vector3 CalcDirection()
    {
        Quaternion rotation = Quaternion.Euler(0, rayAngle + boss.eulerAngles.y, 0);

        Vector3 direction = rotation * Vector3.forward;

        return direction;
    }

    float TriangleMathStuff()
    {
        float a = (transform.position - boss.position).normalized.z;
        float b = 1f;
        float c = (transform.position - boss.position).normalized.x;
        float angle = Mathf.Acos((Mathf.Pow(a, 2) + Mathf.Pow(b, 2) - Mathf.Pow(c, 2)) / (2 * a * b));

        if (c < 0)
        {
            angle *= -1;
        }

        return angle * Mathf.Rad2Deg;
    }
}
