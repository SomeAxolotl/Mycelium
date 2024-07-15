using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IKBossArmSolver : MonoBehaviour
{
    private const int RAY_DIST = 20;
    
    [SerializeField] private LayerMask raycastLayer;
    [SerializeField] private Transform boss;
    [SerializeField] private IKBossArmSolver otherArmSolver;
    private float maxDist = 3f;

    [HideInInspector] public bool isMoving = false;
    private bool isCoolingDown = false;

    private Ray collisionRay;
    private RaycastHit hit;
    private float rayOffsetDist;
    private float rayOffsetAngle;

    private Vector3 currentPosition;

    private Animator bossAnimator;
    private CinemachineImpulseSource impulseSource;

    void Start()
    {
        bossAnimator = boss.GetComponent<Animator>();
        impulseSource = boss.GetComponent<CinemachineImpulseSource>();

        currentPosition = transform.position;
        rayOffsetDist = Vector3.Distance(transform.position, boss.position);
        //Debug.Log((transform.position - boss.position).normalized, this);

        rayOffsetAngle = TriangleMathStuff();
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
            Debug.DrawRay(collisionRay.origin, collisionRay.direction * RAY_DIST, name.StartsWith("R") ? Color.red : Color.green, 0f, false);

            if (Vector3.Distance(hit.point, currentPosition) > maxDist && isCoolingDown == false && isMoving == false && otherArmSolver.isMoving == false)
            {
                StartCoroutine(ChangePosition(0.2f, hit.point + (Vector3.up * 0.38f)));
            }
        }
        else
        {
            Debug.DrawRay(collisionRay.origin, collisionRay.direction * RAY_DIST, Color.yellow, 0f, false);
        }

        transform.position = currentPosition;

    }

    private IEnumerator ChangePosition(float time, Vector3 newPosition)
    {
        float elapsedTime = 0f;
        float t = 0f;

        Vector3 originalPosition = currentPosition;

        isMoving = true;
        isCoolingDown = true;

        while (elapsedTime < time)
        {
            t = elapsedTime / time;

            currentPosition = Vector3.Lerp(originalPosition, newPosition, t);
            currentPosition.y += Mathf.Sin(t * Mathf.PI) * 1f;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        CameraShakeManager.instance.ShakeCamera(impulseSource, 0.25f);

        currentPosition = newPosition;
        isMoving = false;

        yield return new WaitForSeconds(time * 2);
        isCoolingDown = false;
        
    }

    Vector3 CalcDirection()
    {
        Quaternion rotation = Quaternion.Euler(0, rayOffsetAngle + boss.eulerAngles.y, 0);

        Vector3 direction = rotation * Vector3.forward;

        return direction;
    }

    float TriangleMathStuff()
    {
        Vector3 direction = Quaternion.Euler(0, boss.eulerAngles.y, 0) * (transform.position - boss.position).normalized;

        float a = direction.z;
        float b = 1f;
        float c = direction.x;
        float angle = Mathf.Acos((Mathf.Pow(a, 2) + Mathf.Pow(b, 2) - Mathf.Pow(c, 2)) / (2 * a * b));

        if (c < 0)
        {
            angle *= -1;
        }

        return angle * Mathf.Rad2Deg;
    }
}
