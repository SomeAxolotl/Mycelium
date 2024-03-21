using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossProcedualAnimation : MonoBehaviour
{
    // script and gameobject references
    private TempMovement tempMovement;
    private CharacterStats characterStats;
    private GameObject player;

    [Header("Transforms")]
    [SerializeField] private Transform leftArmTarget;
    [SerializeField] private Transform rightArmTarget;
    [SerializeField] private Transform origHeadParent;
    [SerializeField] private Transform head;
    [SerializeField] private Transform spine;

    [Header("MathStuff")]
    [SerializeField] private float delayTime;
    [SerializeField] private float startDelayTime;
    [Header("This is for the pausing while rotating")]
    [SerializeField] private float PauseTime;
    [SerializeField] private float maxRotation;
    [SerializeField] private float initialRotationSpeed;

    // bools to check if player is moving right or left
    private bool playerIsRight;
    private bool playerIsLeft;

    // start movement boss coroutine
    private Coroutine bossCoroutine;

    // original rotations
    private Quaternion originalHeadRotation;
    private Quaternion originalBodyRotation;
    private Quaternion originalSpineRotation;
    private Quaternion originalRightTargetRotation;
    private Quaternion originalLeftTargetRotation;

    // original positions
    private Vector3 originalRightTargetPosition;
    private Vector3 originalLeftTargetPosition;

    // Start is called before the first frame update
    void Start()
    {
        // making sure the game objects are assigned
        if (spine == null || head == null || origHeadParent == null)
        {
            Debug.LogError("One or more objects are not assigned!");
            return;
        }

        // script and object references
        player = GameObject.FindWithTag("currentPlayer");
        tempMovement = GetComponent<TempMovement>();
        characterStats = player.GetComponent<CharacterStats>();

        // objects rotations and positions
        originalHeadRotation = head.rotation;
        originalSpineRotation = spine.rotation;
        originalRightTargetPosition = rightArmTarget.position;
        originalRightTargetRotation = rightArmTarget.rotation;
        originalLeftTargetPosition = leftArmTarget.position;
        originalLeftTargetRotation = leftArmTarget.rotation;

        // start body coroutine with delay to account for transition into scene
        StartCoroutine(StartBossBehaviorWithDelay());
    }

    // starting start coroutine with delay
    private IEnumerator StartBossBehaviorWithDelay()
    {
        yield return new WaitForSeconds(startDelayTime);
        bossCoroutine = StartCoroutine(BossBehavior());
    }

    // starting boss coroutine
    IEnumerator BossBehavior()
    {
        while (true)
        {
            yield return new WaitForSeconds(delayTime);

            // check if player is left or right
            playerIsRight = tempMovement.playerIsRight;
            playerIsLeft = tempMovement.playerIsLeft;

            // if player is left or right
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

    // the player is to the right
    IEnumerator TurnHeadRight()
    {
        Debug.Log("Have started right");
        // move head
        yield return StartCoroutine(TurnHead());
        // move right arm
        yield return StartCoroutine(MoveRightFirstArm());
        yield return StartCoroutine(MoveRightFirst2Arm());
        // move left arm
        yield return StartCoroutine(MoveLeftSecondArm());
        yield return StartCoroutine(MoveLeftSecond2Arm());
        yield return StartCoroutine(MoveLeftSecond3Arm());
        // child head to body
        head.parent = null;
        head.parent = spine;
        // Lean the body
        yield return StartCoroutine(LeanBody());
        yield return StartCoroutine(LeanBody2());
        yield return StartCoroutine(LeanBody3());
        yield return StartCoroutine(LeanBody4());
        // child head to orginalParent
        head.parent = null;
        head.parent = origHeadParent;
        yield return new WaitForSeconds(PauseTime);
    }

    // the player is to the left
    IEnumerator TurnHeadLeft()
    {
        Debug.Log("Have started right");
        // move head
        yield return StartCoroutine(TurnHead());
        // move right arm
        yield return StartCoroutine(MoveLeftFirstArm());
        yield return StartCoroutine(MoveLeftFirst2Arm());
        // move left arm
        yield return StartCoroutine(MoveRightSecondArm());
        yield return StartCoroutine(MoveRightSecond2Arm());
        yield return StartCoroutine(MoveRightSecond3Arm());
        // child head to body
        head.parent = null;
        head.parent = spine;
        // Lean the body
        yield return StartCoroutine(LeanBody());
        yield return StartCoroutine(LeanBody2());
        yield return StartCoroutine(LeanBody3());
        yield return StartCoroutine(LeanBody4());
        // child head to orginalParent
        head.parent = null;
        head.parent = origHeadParent;
        yield return new WaitForSeconds(PauseTime);
    }

    IEnumerator TurnHead()
    {
        // do this over a certain amount of time
        float elapsedTime = 0.0f;
        while (elapsedTime < 1.0f)
        {
            // stuff of moving the head
            Vector3 directionToPlayer = (tempMovement.player.position - head.position).normalized;
            float playerSpeed = characterStats.moveSpeed;
            float rotationSpeedMultiplier = Mathf.Clamp(playerSpeed, 0.5f, 1.5f);
            float adjustedRotationSpeed = initialRotationSpeed * rotationSpeedMultiplier;
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);
            float yRotationDifference = targetRotation.eulerAngles.y - head.eulerAngles.y;
            float clampedYRotation = Mathf.Clamp(yRotationDifference, -maxRotation, maxRotation);
            Quaternion newRotation = Quaternion.Euler(originalHeadRotation.eulerAngles.x, head.rotation.eulerAngles.y + clampedYRotation, originalHeadRotation.eulerAngles.z);
            head.rotation = Quaternion.Lerp(head.rotation, newRotation, Time.deltaTime * adjustedRotationSpeed);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        head.rotation = Quaternion.Euler(originalHeadRotation.eulerAngles.x, head.rotation.eulerAngles.y, originalHeadRotation.eulerAngles.z);
    }

    IEnumerator MoveRightFirstArm()
    {
        // do this over a certain amount of time
        float elapsedTime = 0.0f;
        while (elapsedTime < 1.0f)
        {
            // stuff of moving the left arm
            // phase 1
            rightArmTarget.position = new Vector3(2.24f, 1.87f, -1.37f);

            // // phase 2
            // rightArmTarget.position = new Vector3(2.28f, 1.97f, -1.55f);
            // rightArmTarget.rotation = Quaternion.Euler(-4.23f, 2.97f, -159.89f);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator MoveRightFirst2Arm()
    {
        // do this over a certain amount of time
        float elapsedTime = 0.0f;
        while (elapsedTime < 1.0f)
        {
            // stuff of moving the left arm
            // phase 2
            rightArmTarget.position = new Vector3(2.28f, 1.97f, -1.55f);
            rightArmTarget.rotation = Quaternion.Euler(-4.23f, 2.97f, -159.89f);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator MoveLeftSecondArm()
    {
        // do this over a certain amount of time
        float elapsedTime = 0.0f;
        while (elapsedTime < 1.0f)
        {
            // stuff of moving the left arm
            // move arm
            leftArmTarget.position = new Vector3(-1.89f, 3.18f, 2.52f);
            leftArmTarget.rotation = Quaternion.Euler(-38.27f, 172.75f, 2.02f);

            // move body and reposition head
            // head.position = new Vector3(0.015999995172023774f, 4.004988193511963f, 0.04892468452453613f);
            // StartCoroutine(RotateBody());

            // move arm
            // leftArmTarget.position = new Vector3(-1.32f, 1.36f, 1.4f);
            // leftArmTarget.rotation = Quaternion.Euler(-65.15f, -2.57f, 202.42f);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator MoveLeftSecond2Arm()
    {
        // do this over a certain amount of time
        float elapsedTime = 0.0f;
        while (elapsedTime < 1.0f)
        {
            // stuff of moving the left arm

            // move body and reposition head
            head.position = new Vector3(0.015999995172023774f, 4.004988193511963f, 0.04892468452453613f);
            StartCoroutine(RotateBody());

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator MoveLeftSecond3Arm()
    {
        // do this over a certain amount of time
        float elapsedTime = 0.0f;
        while (elapsedTime < 1.0f)
        {
            // stuff of moving the left arm
            // move arm
            leftArmTarget.position = new Vector3(-1.32f, 1.36f, 1.4f);
            leftArmTarget.rotation = Quaternion.Euler(-65.15f, -2.57f, 202.42f);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }


    IEnumerator MoveLeftFirstArm()
    {
        // do this over a certain amount of time
        float elapsedTime = 0.0f;
        while (elapsedTime < 1.0f)
        {
            // stuff of moving the left arm
            // phase 1
            leftArmTarget.position = new Vector3(-2.24f, 1.87f, -1.37f);

            // phase 2
            // leftArmTarget.position = new Vector3(2.28f, 1.97f, -1.55f);
            // leftArmTarget.rotation = Quaternion.Euler(-4.23f, 2.97f, -159.89f);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator MoveLeftFirst2Arm()
    {
        // do this over a certain amount of time
        float elapsedTime = 0.0f;
        while (elapsedTime < 1.0f)
        {
            // stuff of moving the left arm
            // phase 2
            leftArmTarget.position = new Vector3(2.28f, 1.97f, -1.55f);
            leftArmTarget.rotation = Quaternion.Euler(-4.23f, 2.97f, -159.89f);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }


    IEnumerator MoveRightSecondArm()
    {
        // do this over a certain amount of time
        float elapsedTime = 0.0f;
        while (elapsedTime < 1.0f)
        {
            // stuff of moving the right arm
            // move arm
            rightArmTarget.position = new Vector3(1.89f, 3.18f, 2.52f);
            rightArmTarget.rotation = Quaternion.Euler(-38.27f, 172.75f, 2.02f);

            // move body and reposition head
            // head.position = new Vector3(0.015999995172023774f, 4.004988193511963f, 0.04892468452453613f);
            // StartCoroutine(RotateBody());

            // move arm
            // rightArmTarget.position = new Vector3(1.32f, 1.36f, 1.4f);
            // rightArmTarget.rotation = Quaternion.Euler(-65.15f, -2.57f, 202.42f);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator MoveRightSecond2Arm()
    {
        // do this over a certain amount of time
        float elapsedTime = 0.0f;
        while (elapsedTime < 1.0f)
        {
            // stuff of moving the right arm
            // move body and reposition head
            head.position = new Vector3(-0.015999995172023774f, 4.004988193511963f, 0.04892468452453613f);
            StartCoroutine(RotateBody());

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator MoveRightSecond3Arm()
    {
        // do this over a certain amount of time
        float elapsedTime = 0.0f;
        while (elapsedTime < 1.0f)
        {
            // stuff of moving the right arm
            // move arm
            rightArmTarget.position = new Vector3(1.32f, 1.36f, 1.4f);
            rightArmTarget.rotation = Quaternion.Euler(-65.15f, -2.57f, 202.42f);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    // turning body based on head
    IEnumerator RotateBody()
    {
        // do this over a certain amount of time
        float elapsedTime = 0.0f;
        while (elapsedTime < 1.0f)
        {
            // stuff of moving the body
            Vector3 directionToPlayer = (tempMovement.player.position - head.position).normalized;
            float playerSpeed = characterStats.moveSpeed;
            float rotationSpeedMultiplier = Mathf.Clamp(playerSpeed, 0.5f, 1.5f);
            float adjustedRotationSpeed = initialRotationSpeed * rotationSpeedMultiplier;
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);
            float yRotationDifference = targetRotation.eulerAngles.y - head.eulerAngles.y;
            float clampedYRotation = Mathf.Clamp(yRotationDifference, -maxRotation, maxRotation);
            Quaternion newRotation = Quaternion.Euler(originalBodyRotation.eulerAngles.x, head.rotation.eulerAngles.y + clampedYRotation, originalBodyRotation.eulerAngles.z);
            spine.rotation = Quaternion.Lerp(spine.rotation, newRotation, Time.deltaTime * adjustedRotationSpeed);
            // spine.rotation = Quaternion.Euler(originalBodyRotation.eulerAngles.x, -1.6f, originalBodyRotation.eulerAngles.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        spine.rotation = Quaternion.Euler(originalBodyRotation.eulerAngles.x, head.rotation.eulerAngles.y, originalBodyRotation.eulerAngles.z);
    }

    IEnumerator LeanBody()
    {
        // do this over a certain amount of time
        float elapsedTime = 0.0f;
        while (elapsedTime < 1.0f)
        {
            // stuff for leaning and moving the body/spine
            // phase 1
            spine.rotation = Quaternion.Euler(10f, originalSpineRotation.eulerAngles.y, originalSpineRotation.eulerAngles.z);
            rightArmTarget.position = new Vector3(3.77f, 2.03f, 1.25f);
            rightArmTarget.rotation = Quaternion.Euler(-61.67f, 19.2f, -178.18f);
            leftArmTarget.position = new Vector3(-3.77f, 2.03f, 1.25f);
            leftArmTarget.rotation = Quaternion.Euler(-61.67f, 19.2f, -178.18f);

            // phase 2
            // spine.rotation = Quaternion.Euler(20f, originalSpineRotation.eulerAngles.y, originalSpineRotation.eulerAngles.z);
            // rightArmTarget.position = new Vector3(2.35f, 0.1f, -1.94f);
            // rightArmTarget.rotation = Quaternion.Euler(-20.17f, 14.51f, 189.82f);
            // leftArmTarget.position = new Vector3(-2.35f, 0.1f, -1.94f);
            // leftArmTarget.rotation = Quaternion.Euler(-20.17f, 14.51f, 189.82f);

            // // phase 3
            // rightArmTarget.position = new Vector3(2.29f, 0.83f, -1.37f);
            // rightArmTarget.rotation = Quaternion.Euler(-36.79f, 10.96f, 191.53f);
            // leftArmTarget.position = new Vector3(-2.29f, 0.83f, -1.37f);
            // leftArmTarget.rotation = Quaternion.Euler(-36.79f, 10.96f, 191.53f);

            // // phase 4
            // spine.rotation = Quaternion.Euler(-16f, originalSpineRotation.eulerAngles.y, originalSpineRotation.eulerAngles.z);
            // rightArmTarget.position = new Vector3(3.02f, 1.78f, -0.72f);
            // rightArmTarget.rotation = Quaternion.Euler(-36.79f, 1.96f, 184.37f);
            // leftArmTarget.position = new Vector3(-3.02f, 1.78f, -0.72f);
            // leftArmTarget.rotation = Quaternion.Euler(-36.79f, 1.96f, 184.37f);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator LeanBody2()
    {
        // do this over a certain amount of time
        float elapsedTime = 0.0f;
        while (elapsedTime < 1.0f)
        {
            // stuff for leaning and moving the body/spine
            // phase 2
            spine.rotation = Quaternion.Euler(20f, originalSpineRotation.eulerAngles.y, originalSpineRotation.eulerAngles.z);
            rightArmTarget.position = new Vector3(2.35f, 0.1f, -1.94f);
            rightArmTarget.rotation = Quaternion.Euler(-20.17f, 14.51f, 189.82f);
            leftArmTarget.position = new Vector3(-2.35f, 0.1f, -1.94f);
            leftArmTarget.rotation = Quaternion.Euler(-20.17f, 14.51f, 189.82f);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator LeanBody3()
    {
        // do this over a certain amount of time
        float elapsedTime = 0.0f;
        while (elapsedTime < 1.0f)
        {
            // stuff for leaning and moving the body/spine
            // phase 3
            rightArmTarget.position = new Vector3(2.29f, 0.83f, -1.37f);
            rightArmTarget.rotation = Quaternion.Euler(-36.79f, 10.96f, 191.53f);
            leftArmTarget.position = new Vector3(-2.29f, 0.83f, -1.37f);
            leftArmTarget.rotation = Quaternion.Euler(-36.79f, 10.96f, 191.53f);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator LeanBody4()
    {
        // do this over a certain amount of time
        float elapsedTime = 0.0f;
        while (elapsedTime < 1.0f)
        {
            // stuff for leaning and moving the body/spine
            // phase 4
            spine.rotation = Quaternion.Euler(-16f, originalSpineRotation.eulerAngles.y, originalSpineRotation.eulerAngles.z);
            rightArmTarget.position = new Vector3(3.02f, 1.78f, -0.72f);
            rightArmTarget.rotation = Quaternion.Euler(-36.79f, 1.96f, 184.37f);
            leftArmTarget.position = new Vector3(-3.02f, 1.78f, -0.72f);
            leftArmTarget.rotation = Quaternion.Euler(-36.79f, 1.96f, 184.37f);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}