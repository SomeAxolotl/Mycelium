using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CurioStats : MonoBehaviour
{
    [SerializeField] protected Transform traversalTransform;
    public bool inUse = false;

    [Min(0f)][Range(0f, 1f)][SerializeField][Tooltip("How much happiness interacting with this curio grants")] public float happinessToIncrease = 0.1f;

    [Min(0f)][Range(0f, 1f)][SerializeField][Tooltip("Likelihood that eccentric Spores will interact with this curio")] public float energeticAttraction = 0.5f;
    [Min(0f)][Range(0f, 1f)][SerializeField][Tooltip("Likelihood that lazy Spores will interact with this curio")] public float lazyAttraction = 0.5f;
    [Min(0f)][Range(0f, 1f)][SerializeField][Tooltip("Likelihood that friendly Spores will interact with this curio")] public float friendlyAttraction = 0.5f;
    [Min(0f)][Range(0f, 1f)][SerializeField][Tooltip("Likelihood that curious Spores will interact with this curio")] public float curiousAttraction = 0.5f;
    [Min(0f)][Range(0f, 1f)][SerializeField][Tooltip("Likelihood that playful Spores will interact with this curio")] public float playfulAttraction = 0.5f;

    public IEnumerator CurioEvent(WanderingSpore wanderingSpore)
    {
        if (traversalTransform == null)
        {
            traversalTransform = transform;
        }

        inUse = true;

        wanderingSpore.CalculatePath(wanderingSpore.transform.position, traversalTransform.position);

        yield return new WaitUntil(() => wanderingSpore.currentState == WanderingSpore.WanderingStates.Ready);
        wanderingSpore.GetComponent<Animator>().SetBool(wanderingSpore.GetWalkAnimation(), false);

        yield return StartCoroutine(DoEvent(wanderingSpore));
    }

    public virtual IEnumerator DoEvent(WanderingSpore wanderingSpore)
    {
        yield return null;
    }

    protected void EndEvent()
    {
        inUse = false;
    }

    protected void IncreaseHappiness(WanderingSpore wanderingSpore)
    {
        wanderingSpore.gameObject.GetComponent<CharacterStats>().ModifyHappiness(happinessToIncrease);
    }
}
