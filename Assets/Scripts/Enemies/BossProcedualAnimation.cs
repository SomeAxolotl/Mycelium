using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossProcedualAnimation : MonoBehaviour
{
    private TempMovement tempMovement;
    private CharacterStats characterStats;
    private GameObject player;

    [Header("Transforms")]
    [SerializeField] private Transform leftArmTarget;
    [SerializeField] private Transform rightArmTarget;
    [SerializeField] private Transform origHeadParent;
    [SerializeField] private Transform head;
    [SerializeField] private Transform spine;
    [SerializeField] private Transform body;

    [Header("MathStuff")]
    [SerializeField] private float delayTime;
    [SerializeField] private float startDelayTime;
    [Header("This is for the pausing while rotating")]
    [SerializeField] private float PauseTime;
    [SerializeField] private float maxRotation;
    [SerializeField] private float initialRotationSpeed;

    private bool playerIsRight;
    private bool playerIsLeft;
    //private bool isTurningHead;

    private Coroutine bossCoroutine;

    private Quaternion originalHeadRotation;
    private Quaternion originalBodyRotation;
    private Quaternion originalSpineRotation;

    // Start is called before the first frame update
    void Start()
    {
        if (spine == null || head == null || origHeadParent == null)
        {
            Debug.LogError("One or more objects are not assigned!");
            return;
        }
        player = GameObject.FindWithTag("currentPlayer");
        tempMovement = GetComponent<TempMovement>();
        characterStats = player.GetComponent<CharacterStats>();
        originalHeadRotation = head.rotation;
        originalBodyRotation = body.rotation;
        originalSpineRotation = spine.rotation;
        StartCoroutine(StartBossBehaviorWithDelay());
    }

    private IEnumerator StartBossBehaviorWithDelay()
    {
        yield return new WaitForSeconds(startDelayTime);
        bossCoroutine = StartCoroutine(BossBehavior());
    }

    IEnumerator BossBehavior()
    {
        while (true)
        {
            yield return new WaitForSeconds(delayTime);
            playerIsRight = tempMovement.playerIsRight;
            playerIsLeft = tempMovement.playerIsLeft;
            if (playerIsRight)
            {
                yield return StartCoroutine(TurnHeadRight());
            }
            if (playerIsLeft)
            {
                yield return StartCoroutine(TurnHeadLeft());
            }
        }
    }

    IEnumerator TurnHeadRight()
    {
        Debug.Log("Have started right");
        // move head
        yield return StartCoroutine(TurnHeadRightTemp());
        // move right arm
        yield return StartCoroutine(MoveRightFirstArm());
        // move left arm
        yield return StartCoroutine(MoveLeftSecondArm());
        // child head to body
        head.parent = null;
        head.parent = spine;
        // Lean the body
        yield return StartCoroutine(LeanBody());
        // child head to orginalParent
        head.parent = null;
        head.parent = origHeadParent;
        yield return new WaitForSeconds(PauseTime);
    }

    IEnumerator TurnHeadLeft()
    {
        Debug.Log("Have started right");
        // move head
        yield return StartCoroutine(TurnHeadLeftTemp());
        // move right arm
        yield return StartCoroutine(MoveLeftFirstArm());
        // move left arm
        yield return StartCoroutine(MoveRightSecondArm());
        // child head to body
        head.parent = null;
        head.parent = spine;
        // Lean the body
        yield return StartCoroutine(LeanBody());
        // child head to orginalParent
        head.parent = null;
        head.parent = origHeadParent;
        yield return new WaitForSeconds(PauseTime);
    }

    // IEnumerator TurnHead()
    // {
    //     float elapsedTime = 0.0f;
    //     while (elapsedTime < 1.0f)
    //     {
    //         Vector3 directionToPlayer = (tempMovement.player.position - head.position).normalized;
    //         float playerSpeed = characterStats.moveSpeed;
    //         float rotationSpeedMultiplier = Mathf.Clamp(playerSpeed, 0.5f, 1.5f);
    //         float adjustedRotationSpeed = initialRotationSpeed * rotationSpeedMultiplier;
    //         Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);
    //         float yRotationDifference = targetRotation.eulerAngles.y - head.eulerAngles.y;
    //         float clampedYRotation = Mathf.Clamp(yRotationDifference, -maxRotation, maxRotation);
    //         Quaternion newRotation = Quaternion.Euler(originalHeadRotation.eulerAngles.x, head.rotation.eulerAngles.y + clampedYRotation, originalHeadRotation.eulerAngles.z);
    //         head.rotation = Quaternion.Lerp(head.rotation, newRotation, Time.deltaTime * adjustedRotationSpeed);
    //         elapsedTime += Time.deltaTime;
    //         yield return null;
    //     }
    //     head.rotation = Quaternion.Euler(originalHeadRotation.eulerAngles.x, head.rotation.eulerAngles.y, originalHeadRotation.eulerAngles.z);
    // }

    IEnumerator TurnHeadRightTemp()
    {
        // y = 25
        head.rotation = Quaternion.Euler(originalHeadRotation.eulerAngles.x, 25f, originalHeadRotation.eulerAngles.z);

        yield return null;
    }

    IEnumerator TurnHeadLeftTemp()
    {
        // y = -25
        head.rotation = Quaternion.Euler(originalHeadRotation.eulerAngles.x, -25f, originalHeadRotation.eulerAngles.z);

        yield return null;
    }

    IEnumerator MoveRightFirstArm()
    {
        // phase 1
        rightArmTarget.position = new Vector3(2.24f, 1.87f, -1.37f);

        // phase 2
        rightArmTarget.position = new Vector3(2.28f, 1.97f, -1.55f);
        rightArmTarget.rotation = Quaternion.Euler(-4.23f, 2.97f, -159.89f);

        yield return null;
    }

    IEnumerator MoveLeftSecondArm()
    {
        // move arm
        leftArmTarget.position = new Vector3(-1.89f, 3.18f, 2.52f);
        leftArmTarget.rotation = Quaternion.Euler(-38.27f, 172.75f, 2.02f);

        // move body
        StartCoroutine(RotateBodyRight());

        // move arm
        leftArmTarget.position = new Vector3(-1.32f, 1.36f, 1.4f);
        leftArmTarget.rotation = Quaternion.Euler(-65.15f, -2.57f, 202.42f);

        yield return null;
    }

    IEnumerator MoveLeftFirstArm()
    {
        // phase 1
        leftArmTarget.position = new Vector3(-2.24f, 1.87f, -1.37f);

        // phase 2
        leftArmTarget.position = new Vector3(2.28f, 1.97f, -1.55f);
        leftArmTarget.rotation = Quaternion.Euler(-4.23f, 2.97f, -159.89f);

        yield return null;
    }

    IEnumerator MoveRightSecondArm()
    {
        // move arm
        rightArmTarget.position = new Vector3(1.89f, 3.18f, 2.52f);
        rightArmTarget.rotation = Quaternion.Euler(-38.27f, 172.75f, 2.02f);

        // move body
        StartCoroutine(RotateBodyLeft());

        // move arm
        rightArmTarget.position = new Vector3(1.32f, 1.36f, 1.4f);
        rightArmTarget.rotation = Quaternion.Euler(-65.15f, -2.57f, 202.42f);

        yield return null;
    }

    IEnumerator RotateBodyRight()
    {
        body.rotation = Quaternion.Euler(originalBodyRotation.eulerAngles.x, 25f, originalBodyRotation.eulerAngles.z);

        yield return null;
    }

    IEnumerator RotateBodyLeft()
    {
        body.rotation = Quaternion.Euler(originalBodyRotation.eulerAngles.x, -25f, originalBodyRotation.eulerAngles.z);

        yield return null;
    }

    IEnumerator LeanBody()
    {
        // phase 1
        spine.rotation = Quaternion.Euler(10f, originalSpineRotation.eulerAngles.y, originalSpineRotation.eulerAngles.z);
        rightArmTarget.position = new Vector3(3.77f, 2.03f, 1.25f);
        rightArmTarget.rotation = Quaternion.Euler(-61.67f, 19.2f, -178.18f);
        leftArmTarget.position = new Vector3(-3.77f, 2.03f, 1.25f);
        leftArmTarget.rotation = Quaternion.Euler(-61.67f, 19.2f, -178.18f);

        // phase 2
        spine.rotation = Quaternion.Euler(20f, originalSpineRotation.eulerAngles.y, originalSpineRotation.eulerAngles.z);
        rightArmTarget.position = new Vector3(2.35f, 0.1f, -1.94f);
        rightArmTarget.rotation = Quaternion.Euler(-20.17f, 14.51f, 189.82f);
        leftArmTarget.position = new Vector3(-2.35f, 0.1f, -1.94f);
        leftArmTarget.rotation = Quaternion.Euler(-20.17f, 14.51f, 189.82f);

        // phase 3
        rightArmTarget.position = new Vector3(2.29f, 0.83f, -1.37f);
        rightArmTarget.rotation = Quaternion.Euler(-36.79f, 10.96f, 191.53f);
        leftArmTarget.position = new Vector3(-2.29f, 0.83f, -1.37f);
        leftArmTarget.rotation = Quaternion.Euler(-36.79f, 10.96f, 191.53f);

        // phase 4
        spine.rotation = Quaternion.Euler(-16f, originalSpineRotation.eulerAngles.y, originalSpineRotation.eulerAngles.z);
        rightArmTarget.position = new Vector3(3.02f, 1.78f, -0.72f);
        rightArmTarget.rotation = Quaternion.Euler(-36.79f, 1.96f, 184.37f);
        leftArmTarget.position = new Vector3(-3.02f, 1.78f, -0.72f);
        leftArmTarget.rotation = Quaternion.Euler(-36.79f, 1.96f, 184.37f);

        yield return null;
    }

    // IEnumerator RotateBody()
    // {
    //     float elapsedTime = 0.0f;
    //     while (elapsedTime < 1.0f)
    //     {
    //         Vector3 directionToPlayer = (tempMovement.player.position - head.position).normalized;
    //         float playerSpeed = characterStats.moveSpeed;
    //         float rotationSpeedMultiplier = Mathf.Clamp(playerSpeed, 0.5f, 1.5f);
    //         float adjustedRotationSpeed = initialRotationSpeed * rotationSpeedMultiplier;
    //         Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);
    //         float yRotationDifference = targetRotation.eulerAngles.y - head.eulerAngles.y;
    //         float clampedYRotation = Mathf.Clamp(yRotationDifference, -maxRotation, maxRotation);
    //         Quaternion newRotation = Quaternion.Euler(originalBodyRotation.eulerAngles.x, head.rotation.eulerAngles.y + clampedYRotation, originalBodyRotation.eulerAngles.z);
    //         body.rotation = Quaternion.Lerp(body.rotation, newRotation, Time.deltaTime * adjustedRotationSpeed);
    //         elapsedTime += Time.deltaTime;
    //         yield return null;
    //     }
    //     body.rotation = Quaternion.Euler(originalBodyRotation.eulerAngles.x, head.rotation.eulerAngles.y, originalBodyRotation.eulerAngles.z);
    // }
}
