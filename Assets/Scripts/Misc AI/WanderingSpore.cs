using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RonaldSunglassesEmoji.Personalities;

public class WanderingSpore : MonoBehaviour
{
    [SerializeField][Tooltip("Radius at which Wandering Spores can notice curio")] float curioRadius = 20f;
    [SerializeField][Tooltip("Angle at which Wandering Spores can notice curio (360 for any angle)")] float curioAngle = 90f;

    [SerializeField][Tooltip("How many zoomies Wandering Spores perform")] int zoomieCount = 2;

    [SerializeField][Tooltip("How fast a wandering Spore moves (Scalar with its speed stat)")] float wanderSpeed = 1f;
    [SerializeField][Tooltip("How fast a wandering Spore can rotate")] float maxRotationSpeed = 300f;
    [SerializeField][Tooltip("Minimum radius for the random wandering state")] float minWanderRadius = 5f;
    [SerializeField][Tooltip("Maximum radius for the random wandering state")] float maxWanderRadius = 10f;
    [SerializeField][Tooltip("How much a Spore moves to avoid before trying to go to the waypoint again -- Only if it can't find a raycast target")] float avoidRadius = 1f;
    [SerializeField][Tooltip("How much time a Spore will try to run against an obstacle until giving up with the task altogether")] float timeUntilGivingUp = 1.5f;
    float wanderRadius;

    public List<GameObject> NEARBYCURIO;

    CharacterStats characterStats;
    Animator animator;
    Rigidbody rb;
    [SerializeField] Transform center;

    Vector3 moveDirection;
    public Vector3 lookTarget;
    Vector3 currentWaypoint;
    private List<Vector3> waypoints = new List<Vector3>();

    float rerouteTimer = 0f;
    int calculatePathAttempts = 0;

    CurioStats previousCurio = null;

    Vector3 previousPosition;
    public float currentSpeed;

    public enum WanderingStates
    {
        Standing,
        Traveling,
        Avoiding,
        Ready,
        Avoided,
        Interacting,
    }
    public WanderingStates currentState;

    //Initializes
    void OnEnable()
    {
        characterStats = GetComponent<CharacterStats>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        currentState = WanderingStates.Standing;
        StartCoroutine(StartingCoroutine());
    }

    void OnDisable()
    {
        animator.SetBool("HappyWalk", false);
        animator.SetBool("SadWalk", false);

        StopAllCoroutines();
    }

    IEnumerator StartingCoroutine()
    {
        yield return new WaitForEndOfFrame();

        CalculateNextState();
    }

    //Calculates and switches to a new state
    void CalculateNextState()
    {   
        StopAllCoroutines();

        //Flip flops between standing and interacting/wandering
        if (currentState != WanderingStates.Standing)
        {
            StartCoroutine(Stand());
        }
        else
        {
            //Makes a list of the CurioAttraction class, sending in this Spore's personality to get the corresponding weight
            SporePersonalities sporePersonality = GetComponent<CharacterStats>().sporePersonality;
            List<CurioAttraction> curioAttractions = new List<CurioAttraction>();
            foreach (CurioStats curioStats in GetNearbyCurio())
            {
                if (curioStats != null && !curioStats.inUse && curioStats != previousCurio)
                {
                    CurioAttraction curioAttraction = new CurioAttraction(curioStats, sporePersonality);
                    curioAttractions.Add(curioAttraction);
                }
            }

            //Uses a weight algorithm (like the enemy spawners) to fill a list of possible CurioAttractions
            List<CurioAttraction> possibleCurios = new List<CurioAttraction>();
            float randomNumber = Random.Range(0f, 1f);
            foreach (CurioAttraction curioAttraction in curioAttractions)
            {
                if (randomNumber < curioAttraction.attraction)
                {
                    possibleCurios.Add(curioAttraction);
                }
            }

            //Select a random CurioAttraction from the possible ones, grab its stats, then call the curio event state, flopping to the other state
            if (possibleCurios.Count > 0)
            {
                int randomCurioNumber = Random.Range(0, possibleCurios.Count);

                CurioStats selectedCurio = possibleCurios[randomCurioNumber].stats;
                StartCoroutine(InteractWithCurio(selectedCurio));
            }
            else
            {
                StartCoroutine(Wander());
            }
        }
    }

    //Ticks up reroute timer to avoid obstacles if the speed is below 0.5
    void Update()
    {
        Vector3 currentVelocity = (transform.position - previousPosition) / Time.deltaTime;
        previousPosition = transform.position;
        currentSpeed = currentVelocity.magnitude;

        if (currentSpeed <= 2f && (currentState == WanderingStates.Traveling || currentState == WanderingStates.Avoiding))
        {
            rerouteTimer += Time.deltaTime;
        }
        else
        {
            rerouteTimer = 0f;
        }

        if (rerouteTimer > timeUntilGivingUp)
        {
            CalculateNextState();
        }
        else if (rerouteTimer > 0.5f && waypoints.Count > 0 && currentState == WanderingStates.Traveling)
        {
            StartCoroutine(AvoidObstacle());

            rerouteTimer = 0f;
        }
    }

    //Calculates a detour path, then reroutes back to the waypoint path when the Spore is in the ready state
    IEnumerator AvoidObstacle()
    {
        Vector3 originalWaypoint = waypoints[0];

        CalculatePath(transform.position,  GetRandomPointNearbyNavMesh(transform.position, avoidRadius), true);

        yield return new WaitUntil(() => currentState == WanderingStates.Avoided);

        CalculatePath(transform.position, originalWaypoint);
    }

    //Experimental Raycast Object Detection
    /*Vector3 GetAvoidanceDirection()
    {
        int environmentLayer = LayerMask.GetMask("Environment");
        int playerLayer = LayerMask.GetMask("Player");
        int combinedLayerMask = environmentLayer | playerLayer;

        Vector3 avoidanceDirection;
        RaycastHit hit;
        if (Physics.Raycast(center.position, transform.forward, out hit, 1f, combinedLayerMask))
        {
            avoidanceDirection = Vector3.Cross(Vector3.up, hit.normal);
        }
        else
        {
            avoidanceDirection = GetRandomPointNearbyNavMesh(transform.position, avoidRadius);
        }
        

        return avoidanceDirection;
    }*/

    //Where the Spore actually moves
    void FixedUpdate()
    {
        rb.AddForce(new Vector3(0f, -10f, 0f), ForceMode.Acceleration);

        if (lookTarget != Vector3.zero)
        {
            LookAtTarget(lookTarget);
        }

        //Only move and rotate if there's a waypoint
        if (waypoints.Count > 0)
        {
            //Movement - Constantly move towards the current waypoint
            moveDirection = (currentWaypoint - transform.position).normalized;
            rb.velocity = new Vector3((moveDirection * wanderSpeed * characterStats.moveSpeed).x, rb.velocity.y, (moveDirection * wanderSpeed * characterStats.moveSpeed).z);

            //Rotation - Smoothly look towards the current waypoint. This variable is used in LookAtTarget()
            lookTarget = moveDirection;

            //If the wandering Spore has arrived at the waypoint
            if (HasArrivedAtWaypoint())
            {
                //Run the logic of reaching the destination
                if (waypoints.Count == 1)
                {
                    //If the Spore was avoiding, the finished state is avoided,
                    if (currentState == WanderingStates.Avoiding)
                    {
                        currentState = WanderingStates.Avoided;
                    }
                    //else it is ready (for an event or something)
                    else
                    {
                        currentState = WanderingStates.Ready;
                    }
                }
                //Else set the current waypoint to the next one in the waypoints list if there's more waypoints
                else
                {
                    currentWaypoint = waypoints[1];
                }

                //Remove the current waypoint
                waypoints.RemoveAt(0);
            }
        }
    }

    //Returns true if the current position is close to the current waypoint
    bool HasArrivedAtWaypoint()
    {
        if (Vector3.Distance(transform.position, currentWaypoint) <= 0.5f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    float GetWanderRadius()
    {
        wanderRadius = Random.Range(minWanderRadius, maxWanderRadius);

        return wanderRadius;
    }

    //Smoothly looks at a target position
    public void LookAtTarget(Vector3 lookDirection)
    {
        Quaternion desiredRotation = Quaternion.LookRotation(moveDirection);
        Quaternion targetRotation = Quaternion.Euler(0f, desiredRotation.eulerAngles.y, 0f);

        float angleToTarget = Quaternion.Angle(transform.rotation, targetRotation);
        float maxAngleThisFrame = maxRotationSpeed * Time.fixedDeltaTime;

        float fractionOfTurn = Mathf.Clamp01(maxAngleThisFrame / angleToTarget); 

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, fractionOfTurn);
    }
    
    //Finds a path to the target position
    public void CalculatePath(Vector3 from, Vector3 to, bool avoiding = false)
    {
        NavMeshPath path = new NavMeshPath();
        //If the "to" is a valid point on the NavMesh,
        if (NavMesh.CalculatePath(from, to, NavMesh.AllAreas, path))
        {
            calculatePathAttempts = 0;

            //Then fill the waypoints list with that path
            waypoints.Clear();

            for (int i = 0; i < path.corners.Length; i++)
            {
                waypoints.Add(path.corners[i]);
            }

            currentWaypoint = waypoints[0];

            animator.SetBool(GetWalkAnimation(), true);

            if (avoiding)
            {
                currentState = WanderingStates.Avoiding;
            }
            else
            {   
                currentState = WanderingStates.Traveling;
            }
        }
        //Else...
        else
        {
            calculatePathAttempts++;

            if (calculatePathAttempts > 10)
            {
                SoundEffectManager.Instance.PlaySound("Pickup", transform.position);
                ParticleManager.Instance.SpawnParticles("TrophicCascadePoof", transform.position, Quaternion.Euler(-90, 0, 0));
                GameManager.Instance.PlaceSpore(gameObject);
                ParticleManager.Instance.SpawnParticles("TrophicCascadePoof", transform.position, Quaternion.Euler(-90, 0, 0));
            }

            CalculateNextState();
        }
    }

    //Custom class just to get a specialized personality weight for each curio
    class CurioAttraction
    {
        public CurioStats stats;
        public float attraction;

        public CurioAttraction(CurioStats curioStats, SporePersonalities sporePersonality)
        {
            stats = curioStats;

            switch(sporePersonality)
            {
                case SporePersonalities.Curious:
                    attraction = curioStats.curiousAttraction;
                    break;
                case SporePersonalities.Playful:
                    attraction = curioStats.playfulAttraction;
                    break;
                case SporePersonalities.Friendly:
                    attraction = curioStats.friendlyAttraction;
                    break;
                case SporePersonalities.Lazy:
                    attraction = curioStats.lazyAttraction;
                    break;
                case SporePersonalities.Energetic:
                    attraction = curioStats.energeticAttraction;
                    break;
            }
        }
    }

    //Returns all nearby fun stuff for a wandering Spore--the player, other Spores, and furniture
    List<CurioStats> GetNearbyCurio()
    {
        NEARBYCURIO.Clear();

        int playerLayer = LayerMask.GetMask("Player");
        int furnitureLayer = LayerMask.GetMask("Furniture");
        int combinedLayers = playerLayer | furnitureLayer;

        List<CurioStats> nearbyCurio = new List<CurioStats>();

        Collider[] colliders = Physics.OverlapSphere(transform.position, curioRadius, combinedLayers);
        
        foreach (Collider col in colliders)
        {
            NEARBYCURIO.Add(col.gameObject);
            foreach (CurioStats curioStats in col.gameObject.GetComponents<CurioStats>())
            {
                nearbyCurio.Add(curioStats);
            }
        }

        return nearbyCurio;
    }

    public string GetWalkAnimation()
    {
        float happiness = GetComponent<CharacterStats>().sporeHappiness;
        string walkAnimation = "Walk";

        if (happiness > 0.7f)
        {
            walkAnimation = "HappyWalk";
        }
        else if (happiness > 0.3f)
        {
            walkAnimation = "Walk";
        }
        else
        {
            walkAnimation = "SadWalk";
        }

        return walkAnimation;
    }

    //Stands idle for a period based on happiness + personality
    IEnumerator Stand()
    {
        currentState = WanderingStates.Standing;

        animator.SetBool(GetWalkAnimation(), false);

        yield return new WaitForSeconds(5f);

        CalculateNextState();
    }

    //Paths towards a random point near the Spore
    IEnumerator Wander()
    {
        CalculatePath(transform.position, GetRandomPointNearbyNavMesh(transform.position, GetWanderRadius()));

        yield return new WaitUntil(() => currentState == WanderingStates.Ready);

        CalculateNextState();
    }

    //Calls and yields the respective curio event before going to the next state
    IEnumerator InteractWithCurio(CurioStats curioStats)
    {
        yield return null;

        currentState = WanderingStates.Interacting;

        yield return StartCoroutine(curioStats.CurioEvent(this));

        CalculateNextState();
    }

    Vector3 GetRandomPointNearbyNavMesh(Vector3 center, float radius)
    {
        Vector2 randomPoint = Random.insideUnitCircle * radius;
        Vector3 worldPoint = center + new Vector3(randomPoint.x, 0, randomPoint.y);
        
        NavMeshHit hit;
        if (NavMesh.SamplePosition(worldPoint, out hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            if (radius == 100f)
            {
                Debug.LogError("Nowhere for " + gameObject.name + " to move in a radius of 100f");
                return Vector3.zero;
            }

            //Recursion :O
            return GetRandomPointNearbyNavMesh(center, radius + 1f);
        }
    }
}
