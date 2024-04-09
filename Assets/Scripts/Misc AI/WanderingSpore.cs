using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingSpore : MonoBehaviour
{
    [SerializeField][Tooltip("Radius at which Wandering Spores can notice curio")] float curioRadius = 20f;
    [SerializeField][Tooltip("Angle at which Wandering Spores can notice curio (360 for any angle)")] float curioAngle = 90f;

    [SerializeField][Tooltip("How many zoomies Wandering Spores perform")] int zoomieCount = 2;

    //List of possible Spore wandering states (and NoState to set previousState at start)
    private enum WanderState
    {
        Standing,
        Walking,
        Zooming,
        Rolling,
        Waving,
        Following,
        Laying,
        Sleeping,
        Dancing,
        Clapping,
        Casting,
        NoState
    }

    WanderState currentState = WanderState.Standing; //The current WanderState
    WanderState previousState = WanderState.NoState; //The previous WanderState

    void Update()
    {
        //Only runs the switch statement if currentState is a new state
        if (currentState != previousState)
        {
            previousState = currentState;

            //Fires the corresponding state coroutine ONCE (each has a while loop)
            switch (currentState)
            {
                case WanderState.Standing:
                    StartCoroutine(StandingState());
                    break;

                case WanderState.Walking:
                    StartCoroutine(WalkingState());
                    break;

                case WanderState.Zooming:
                    StartCoroutine(ZoomingState());
                    break;

                case WanderState.Rolling:
                    StartCoroutine(RollingState());
                    break;

                case WanderState.Waving:
                    StartCoroutine(WavingState());
                    break;

                case WanderState.Following:
                    StartCoroutine(FollowingState());
                    break;

                case WanderState.Laying:
                    StartCoroutine(LayingState());
                    break;

                case WanderState.Sleeping:
                    StartCoroutine(SleepingState());
                    break;

                case WanderState.Casting:
                    StartCoroutine(CastingState());
                    break;
            }
        }
    }

    //Calculates and switches to a new state
    void CalculateNextState()
    {
        //Flip flops between standing and another state
        if (currentState != WanderState.Standing)
        {
            currentState = WanderState.Standing;
        }
        else
        {
             foreach (GameObject curio in GetNearbyCurio())
             {
                if (curio.GetComponent<CurioStats>() != null)
                {

                }
             }
        }
    }

    //Returns all nearby fun stuff for a wandering Spore--the player, other Spores, and furniture
    List<GameObject> GetNearbyCurio()
    {
        int playerLayer = LayerMask.GetMask("Player");
        int furnitureLayer = LayerMask.GetMask("Furniture");
        int combinedLayers = playerLayer | furnitureLayer;

        List<GameObject> nearbyCurio = new List<GameObject>();

        Collider[] colliders = Physics.OverlapSphere(transform.position, curioRadius, combinedLayers);
        foreach (Collider col in colliders)
        {
            nearbyCurio.Add(col.gameObject);
        }

        return nearbyCurio;
    }

    //Stands idle for a period based on happiness + personality
    IEnumerator StandingState()
    {
        yield return null;
    }

    IEnumerator WalkingState()
    {
        yield return null;
    }

    //The Zoomies. Runs in a circle zoomieCount times
    IEnumerator ZoomingState()
    {
        yield return null;
    }

    //Performs a roll
    IEnumerator RollingState()
    {
        yield return null;
    }

    //Waves at the player or another Spore
    IEnumerator WavingState()
    {
        yield return null;
    }

    //Follows the player
    public IEnumerator FollowingState()//GameObject objectToFollow)
    {
        yield return null;
    }

    //Lays down for a period based on happiness + personality
    public IEnumerator LayingState()//Vector3 layPosition
    {
        yield return null;
    }

    //Sleeps for a period based on happiness + personality
    public IEnumerator SleepingState()//Vector3 sleepPosition
    {
        yield return null;
    }

    //Casts one of its equipped Skills
    IEnumerator CastingState()
    {
        GameObject skillLoadout = transform.Find("SkillLoadout").gameObject;
        
        int randomNumber = Random.Range(0, 3);
        Skill skillToUse = skillLoadout.transform.GetChild(randomNumber).gameObject.GetComponent<Skill>();
        if (skillToUse.canSkill)
        {
            skillToUse.ActivateSkill(randomNumber);
        }

        //Only casting doesn't transition into standing, since the Spore needs to move during some skills
        currentState = WanderState.Walking;

        yield return null;
    }

}
