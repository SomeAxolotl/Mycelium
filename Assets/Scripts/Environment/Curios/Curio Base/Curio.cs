using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using TMPro;

public abstract class Curio : MonoBehaviour
{
    public List<TraversalTransform> traversalTransforms = new List<TraversalTransform>();
    public int maxUserCount = 1;
    public List<string> interactAnimationStrings = new List<string>();
    public float playerInteractRange = 2f;
    [HideInInspector] public bool canBeActivated = false;
    [HideInInspector] public List<WanderingSpore> currentUsers = new List<WanderingSpore>();
    [HideInInspector] public int currentUserCount = 0;
    GameObject interactCanvas;

    [Header("Curio")]
    [Tooltip("Whether only THIS object can see this curio, or only OTHER objects can see this curio")] public bool selfCurio = false;
    float interactTextHeightOffset = 2f;

    [Header("Happiness")]
    [Min(0f)][Range(0f, 1f)][SerializeField][Tooltip("How much happiness interacting with this curio grants")] public float happinessToIncrease = 0.1f;

    [Header("Personalities")]
    [Min(0f)][Range(0f, 1f)][SerializeField][Tooltip("Likelihood that energetic Spores will interact with this curio")] public float energeticAttraction = 0.5f;
    [Min(0f)][Range(0f, 1f)][SerializeField][Tooltip("Likelihood that lazy Spores will interact with this curio")] public float lazyAttraction = 0.5f;
    [Min(0f)][Range(0f, 1f)][SerializeField][Tooltip("Likelihood that friendly Spores will interact with this curio")] public float friendlyAttraction = 0.5f;
    [Min(0f)][Range(0f, 1f)][SerializeField][Tooltip("Likelihood that curious Spores will interact with this curio")] public float curiousAttraction = 0.5f;
    [Min(0f)][Range(0f, 1f)][SerializeField][Tooltip("Likelihood that playful Spores will interact with this curio")] public float playfulAttraction = 0.5f;

    [Header("Event")]
    [SerializeField] bool meow;

    public event Action OnPlayingDone;

    IEnumerator Start()
    {
        yield return null;

        if (!selfCurio && !gameObject.name.Contains("Spore") && !gameObject.name.Contains("Keeper") && !gameObject.name.Contains("DancePad"))
        {
            gameObject.SetActive(FurnitureManager.Instance.FurnitureIsUnlocked(gameObject.name));
        }
    }

    void OnEnable()
    {
        if (GlobalData.areaCleared && !selfCurio)
        {
            canBeActivated = true;

            Vector3 heightOffset = new Vector3(0, interactTextHeightOffset, 0);
            Vector3 newPosition = transform.position + heightOffset;
            interactCanvas = Instantiate(FurnitureManager.Instance.furnitureInteractCanvas, newPosition, Quaternion.identity);
            interactCanvas.GetComponentInChildren<TMP_Text>().text = InputManager.Instance.GetLatestController().attackHint.GenerateColoredHintString();
        }
    }

    void Update()
    {
        if (interactCanvas != null)
        {
            interactCanvas.transform.rotation = Quaternion.LookRotation(interactCanvas.transform.position - Camera.main.transform.position);
        }
    }

    public IEnumerator CurioEvent(WanderingSpore wanderingSpore)
    {
        currentUsers.Add(wanderingSpore);
        currentUserCount++;

        List<TraversalTransform> possibleTraversalTransforms = new List<TraversalTransform>();
        foreach (TraversalTransform traversalTransform in traversalTransforms)
        {
            if (traversalTransform.interactingSpore == null)
            {
                possibleTraversalTransforms.Add(traversalTransform);
            }
        }

        float lowestDistance = Vector3.Distance(wanderingSpore.transform.position, possibleTraversalTransforms[0].transform.position);
        TraversalTransform closestPossibleTraversalTransform = possibleTraversalTransforms[0];
        foreach (TraversalTransform possibleTraversalTransform in possibleTraversalTransforms)
        {
            if (Vector3.Distance(wanderingSpore.transform.position, possibleTraversalTransform.transform.position) < lowestDistance)
            {
                closestPossibleTraversalTransform = possibleTraversalTransform;
            }
        }

        closestPossibleTraversalTransform.interactingSpore = wanderingSpore;

        wanderingSpore.CalculatePath(wanderingSpore.transform.position, closestPossibleTraversalTransform.transform.position);

        yield return new WaitUntil(() => wanderingSpore.currentState == WanderingSpore.WanderingStates.Ready || wanderingSpore.interactingCurio == null);
        if (wanderingSpore.interactingCurio != null)
        {
            wanderingSpore.GetComponent<Animator>().SetBool(wanderingSpore.GetWalkAnimation(), false);

            if (wanderingSpore != null)
            {
                yield return wanderingSpore.StartCoroutine(DoEvent(wanderingSpore));
            }
        }
    }

    [System.Serializable]
    public class TraversalTransform
    {
        public Transform transform;
        [HideInInspector] public WanderingSpore interactingSpore = null;

        public TraversalTransform(Transform argTransform)
        {
            this.transform = argTransform;
        }
    }

    public bool CanUse()
    {
        return currentUserCount / maxUserCount < 1;
    }

    public virtual IEnumerator DoEvent(WanderingSpore wanderingSpore)
    {
        yield return null;
    }

    //Called by the WanderingSpore when it finishes the event OR if you swap to it while it's doing the event
    public void EndEvent(WanderingSpore wanderingSpore)
    {
        currentUsers.Remove(wanderingSpore);
        currentUserCount--;

        foreach (TraversalTransform traversalTransform in traversalTransforms)
        {
            if (traversalTransform.interactingSpore == wanderingSpore)
            {
                traversalTransform.interactingSpore = null;
            }
        }

        wanderingSpore.rb.constraints = RigidbodyConstraints.FreezeRotation;

        OnPlayingDone?.Invoke();
    }

    public void Activate(CharacterStats currentPlayerStats)
    {
        if (canBeActivated && !selfCurio)
        {
            StartCoroutine(HappinessInjection(currentPlayerStats));
        }

        LockActivatability();
    }

    IEnumerator HappinessInjection(CharacterStats currentPlayerStats)
    {
        float timeBetweenHappinessInjections = 0.35f;

        List<WanderingSpore> usersAtActivation = new List<WanderingSpore>();
        if (this is DanceCurio)
        {
            DanceCurio[] allDanceCurios = FindObjectsOfType<DanceCurio>();
            foreach (DanceCurio danceCurio in allDanceCurios)
            {
                usersAtActivation.AddRange(danceCurio.currentUsers);
            }
        }
        else
        {
            usersAtActivation = currentUsers;
        }

        foreach (WanderingSpore wanderingSpore in usersAtActivation)
        {
            CharacterStats characterStats = wanderingSpore.GetComponent<CharacterStats>();
            if (characterStats != null)
            {   
                characterStats.ModifyHappiness(happinessToIncrease);

                yield return new WaitForSeconds(timeBetweenHappinessInjections);
            }
        }

        currentPlayerStats.ModifyHappiness(happinessToIncrease);
    }

    void LockActivatability()
    {
        if (this is DanceCurio)
        {
            DanceCurio[] allDanceCurios = FindObjectsOfType<DanceCurio>();
            foreach (DanceCurio danceCurio in allDanceCurios)
            {
                danceCurio.canBeActivated = false;
                Destroy(danceCurio.interactCanvas);
            }
        }
        else
        {
            canBeActivated = false;
            Destroy(interactCanvas);
        }
    }
}
