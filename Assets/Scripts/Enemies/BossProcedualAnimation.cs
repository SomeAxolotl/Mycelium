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
    [SerializeField] private Transform head;
    [SerializeField] private Transform body;

    [Header("MathStuff")]
    [SerializeField] private float delayTime;
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

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("currentPlayer");
        tempMovement = GetComponent<TempMovement>();
        characterStats = player.GetComponent<CharacterStats>();
        originalHeadRotation = head.rotation;
        originalBodyRotation = body.rotation;
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
        //isTurningHead = true;
        yield return StartCoroutine(TurnHead());
        // move right arm
        // Rotate Body
        yield return StartCoroutine(RotateBody());
        // move left arm 
        // rotate body
        // put left arm in its final place
        //isTurningHead = false;
        yield return new WaitForSeconds(PauseTime);
    }

    IEnumerator TurnHeadLeft()
    {
        //isTurningHead = true;
        yield return StartCoroutine(TurnHead());
        // move right arm
        // Rotate Body
        yield return StartCoroutine(RotateBody());
        // move left arm 
        // rotate body
        // put left arm in its final place
        //isTurningHead = false;
        yield return new WaitForSeconds(PauseTime);
    }

    IEnumerator TurnHead()
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < 1.0f)
        {
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

    IEnumerator RotateBody()
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < 1.0f)
        {
            Vector3 directionToPlayer = (tempMovement.player.position - head.position).normalized;
            float playerSpeed = characterStats.moveSpeed;
            float rotationSpeedMultiplier = Mathf.Clamp(playerSpeed, 0.5f, 1.5f);
            float adjustedRotationSpeed = initialRotationSpeed * rotationSpeedMultiplier;
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);
            float yRotationDifference = targetRotation.eulerAngles.y - head.eulerAngles.y;
            float clampedYRotation = Mathf.Clamp(yRotationDifference, -maxRotation, maxRotation);
            Quaternion newRotation = Quaternion.Euler(originalBodyRotation.eulerAngles.x, head.rotation.eulerAngles.y + clampedYRotation, originalBodyRotation.eulerAngles.z);
            body.rotation = Quaternion.Lerp(body.rotation, newRotation, Time.deltaTime * adjustedRotationSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        body.rotation = Quaternion.Euler(originalBodyRotation.eulerAngles.x, head.rotation.eulerAngles.y, originalBodyRotation.eulerAngles.z);
    }
}
