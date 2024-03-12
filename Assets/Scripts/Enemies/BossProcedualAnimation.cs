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
    [SerializeField] private Transform neck;

    [Header("MathStuff")]
    [SerializeField] private float delayTime;
    [Header("This is for the pausing while rotating")]
    [SerializeField] private float PauseTime;
    [SerializeField] private float maxRotation;
    [SerializeField] private float maxBodyRotate;
    [SerializeField] private float initialHeadRotationSpeed;
    [SerializeField] private float initialBodyRotationSpeed;

    private bool playerIsRight;
    private bool playerIsLeft;
    private bool isTurningHead;

    private Coroutine bossCoroutine;

    private Quaternion originalHeadRotation;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("currentPlayer");
        tempMovement = GetComponent<TempMovement>();
        characterStats = player.GetComponent<CharacterStats>();
        originalHeadRotation = head.rotation;
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
        isTurningHead = true;
        yield return StartCoroutine(TurnHead());
        //Rotate Body
        isTurningHead = false;
        yield return new WaitForSeconds(5f);
    }

    IEnumerator TurnHeadLeft()
    {
        isTurningHead = true;
        yield return StartCoroutine(TurnHead());
        //Rotate Body
        isTurningHead = false;
        yield return new WaitForSeconds(5f);
    }

    IEnumerator TurnHead()
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < 1.0f)
        {
            Vector3 directionToPlayer = (tempMovement.player.position - head.position).normalized;
            float playerSpeed = characterStats.moveSpeed;
            float rotationSpeedMultiplier = Mathf.Clamp(playerSpeed, 0.5f, 1.5f);
            float adjustedRotationSpeed = initialHeadRotationSpeed * rotationSpeedMultiplier;
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);
            float clampedYRotation = Mathf.Clamp(targetRotation.eulerAngles.y, head.eulerAngles.y - maxRotation, head.eulerAngles.y + maxRotation);
            Quaternion newRotation = Quaternion.Euler(originalHeadRotation.eulerAngles.x, Mathf.Lerp(head.rotation.eulerAngles.y, clampedYRotation, Time.deltaTime * adjustedRotationSpeed), originalHeadRotation.eulerAngles.z);
            head.rotation = newRotation;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        head.rotation = Quaternion.Euler(originalHeadRotation.eulerAngles.x, head.rotation.eulerAngles.y, originalHeadRotation.eulerAngles.z);
    }
}
