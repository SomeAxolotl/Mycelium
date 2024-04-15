using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RonaldSunglassesEmoji.Personalities;
using UnityEngine.Scripting.APIUpdating;

public class WanderingSpore : MonoBehaviour
{
    [Header("curio Detection")]
    [SerializeField][Tooltip("Radius at which Wandering Spores can notice curio")] float curioRadius = 20f;

    [Header("Wandering")]
    [SerializeField][Tooltip("How fast a wandering Spore moves (Scalar with its speed stat)")] float wanderSpeed = 1f;
    [SerializeField][Tooltip("How fast a wandering Spore can rotate")] float maxRotationSpeed = 300f;
    [SerializeField][Tooltip("Minimum radius for the random wandering state")] float minWanderRadius = 5f;
    [SerializeField][Tooltip("Maximum radius for the random wandering state")] float maxWanderRadius = 10f;
    [SerializeField][Tooltip("Rigidbody mass when they're standing or doing something")] int rbActiveMass = 10000;
    [SerializeField][Tooltip("Rigidbody mass when they're walking or disabled")] int rbInactiveMass = 1;
    [SerializeField][Tooltip("How close is considered arrived for the Spore")] float arrivedDistance = 0.1f;

    [Header("Avoidance")]
    [SerializeField][Tooltip("How much a Spore moves to avoid before trying to go to the waypoint again")] float avoidRadius = 1f;
    [SerializeField][Tooltip("The angle offset for how much it moves out of the way")] float avoidAngleOffset = -45f;
    [SerializeField][Tooltip("How much time blocked until the Spore reroutes")] float rerouteTime = 0.75f;
    [SerializeField][Tooltip("How much speed until the reroute timer starts ticking up")] float rerouteSpeedThreshold = 1f;
    //[SerializeField][Tooltip("Multiplies with the reroute speed check to scale with speed and avoid properly")] float rerouteSpeedThresholdMultiplier = 0.5f;
    [SerializeField][Tooltip("How much time a Spore will try to run against an obstacle until giving up with the task altogether")] float timeUntilGivingUp = 1.5f;
    [SerializeField][Tooltip("How much time a Spore will spend stuck until poofing away and respawning")] float timeUntilRespawn = 5f;
    float wanderRadius;

    [Header("Standing")]
    [SerializeField][Tooltip("Minimum for how long a spore stands at minimum happiness")] float minHappinessMinStandingTime = 8f;
    [SerializeField][Tooltip("Maximum for how long a spore stands at minimum happiness")] float minHappinessMaxStandingTime = 12f;
    [SerializeField][Tooltip("Minimum for how long a spore stands at maximum happiness")] float maxHappinessMinStandingTime = 2f;
    [SerializeField][Tooltip("Maximum for how long a spore stands at maximum happiness")] float maxHappinessMaxStandingTime = 6f;

    CharacterStats characterStats;
    public Animator animator;
    Rigidbody rb;
    [SerializeField] Transform center;

    Vector3 moveDirection;
    public Vector3 lookTarget;
    Vector3 currentWaypoint;
    private List<Vector3> waypoints = new List<Vector3>();

    float rerouteTimer = 0f;
    int calculatePathAttempts = 0;

    Curio previousCurio = null;
    public Curio interactingCurio = null;

    Vector3 previousPosition;
    public float currentSpeed;

    [Header("Testing")]
    [SerializeField] GameObject waypointVisual;
    List<GameObject> currentWaypointVisuals = new List<GameObject>();

    public enum WanderingStates
    {
        Standing,
        Traveling,
        Avoiding,
        Ready,
        Avoided,
        Interacting
    }
    public WanderingStates currentState;

    //Initializes
    void OnEnable()
    {
        characterStats = GetComponent<CharacterStats>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        StartCoroutine(StartingCoroutine());
    }

    IEnumerator StartingCoroutine()
    {
        currentState = WanderingStates.Standing;

        float randomStartingTime = Random.Range(1f, 2f);

        yield return new WaitForSeconds(randomStartingTime);

        CalculateNextState();
    }

    void OnDisable()
    {
        animator.SetBool("HappyWalk", false);
        animator.SetBool("SadWalk", false);

        rb.mass = rbInactiveMass;

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            animator.Rebind();
        }

        EndInteractingCurioEvent();

        StopAllCoroutines();
    }

    public void SetWaypoints(List<Vector3> waypointPositions)
    {
        waypoints = waypointPositions;
    }

    void EndInteractingCurioEvent()
    {
        if (interactingCurio != null)
        {
            interactingCurio.EndEvent(this);
        }
        interactingCurio = null;
    }

    //Calculates and switches to a new state
    void CalculateNextState()
    {   
        EndInteractingCurioEvent();

        StopAllCoroutines();

        //Flip flops between standing and interacting/wandering
        if (currentState != WanderingStates.Standing)
        {
            rb.mass = rbActiveMass;

            StartCoroutine(Stand());
        }
        else
        {
            rb.mass = rbInactiveMass;

            //Makes a list of the CurioAttraction class, sending in this Spore's personality to get the corresponding weight
            SporePersonalities sporePersonality = GetComponent<CharacterStats>().sporePersonality;
            List<CurioAttraction> curioAttractions = new List<CurioAttraction>();
            foreach (Curio curio in GetNearbyCurio())
            {
                if (curio != null && curio.CanUse() && curio != previousCurio && ((curio.selfCurio && this.gameObject == curio.gameObject) || (!curio.selfCurio && this.gameObject != curio.gameObject)))
                {
                    CurioAttraction curioAttraction = new CurioAttraction(curio, sporePersonality);
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

                Curio selectedCurio = possibleCurios[randomCurioNumber].stats;
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

        if (currentSpeed <= rerouteSpeedThreshold && (currentState == WanderingStates.Traveling || currentState == WanderingStates.Avoiding))
        {
            rerouteTimer += Time.deltaTime;
        }
        else
        {
            rerouteTimer = 0f;
        }

        if (rerouteTimer > timeUntilRespawn)
        {
            RespawnSpore();
        }
        else if (rerouteTimer > timeUntilGivingUp)
        {
            CalculateNextState();
        }
        else if (rerouteTimer > rerouteTime && waypoints.Count > 0 && currentState == WanderingStates.Traveling)
        {
            StartCoroutine(AvoidObstacle());

            rerouteTimer = 0f;
        }
    }

    //Calculates a detour path, then reroutes back to the waypoint path when the Spore is in the ready state
    IEnumerator AvoidObstacle()
    {
        Vector3 originalWaypoint = waypoints[0];

        CalculatePath(transform.position,  GetPerpendicularPointNearbyNavMesh(transform.position, moveDirection, avoidRadius), true);

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
        else if (moveDirection != Vector3.zero)
        {
            LookAtTarget(moveDirection);
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

                UpdateWaypointVisuals(waypoints);
            }
        }
    }

    //Returns true if the current position is close to the current waypoint
    bool HasArrivedAtWaypoint()
    {
        if (Vector3.Distance(transform.position, currentWaypoint) <= arrivedDistance)
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
        Quaternion desiredRotation = Quaternion.LookRotation(lookDirection);
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

            UpdateWaypointVisuals(waypoints);
        }
        //Else...
        else
        {
            calculatePathAttempts++;

            if (calculatePathAttempts > 10)
            {
                RespawnSpore();
            }

            CalculateNextState();
        }
    }

    void RespawnSpore()
    {
        SoundEffectManager.Instance.PlaySound("Pickup", transform.position);
        ParticleManager.Instance.SpawnParticles("TrophicCascadePoof", transform.position, Quaternion.Euler(-90, 0, 0));
        GameManager.Instance.PlaceSpore(gameObject);
        ParticleManager.Instance.SpawnParticles("TrophicCascadePoof", transform.position, Quaternion.Euler(-90, 0, 0));
    }

    public void UpdateWaypointVisuals(List<Vector3> waypointPositions)
    {
        foreach (GameObject currentWayPointVisual in currentWaypointVisuals)
        {
            Destroy(currentWayPointVisual);
        }
        
        currentWaypointVisuals.Clear();

        foreach (Vector3 waypointPosition in waypoints)
        {
            currentWaypointVisuals.Add(Instantiate(waypointVisual, waypointPosition, Quaternion.identity));
        }
    }

    //Custom class just to get a specialized personality weight for each curio
    class CurioAttraction
    {
        public Curio stats;
        public float attraction;

        public CurioAttraction(Curio curio, SporePersonalities sporePersonality)
        {
            stats = curio;

            switch(sporePersonality)
            {
                case SporePersonalities.Curious:
                    attraction = curio.curiousAttraction;
                    break;
                case SporePersonalities.Playful:
                    attraction = curio.playfulAttraction;
                    break;
                case SporePersonalities.Friendly:
                    attraction = curio.friendlyAttraction;
                    break;
                case SporePersonalities.Lazy:
                    attraction = curio.lazyAttraction;
                    break;
                case SporePersonalities.Energetic:
                    attraction = curio.energeticAttraction;
                    break;
            }
        }
    }

    //Returns all nearby fun stuff for a wandering Spore--the player, other Spores, and furniture
    List<Curio> GetNearbyCurio()
    {
        int playerLayer = LayerMask.GetMask("Player");
        int furnitureLayer = LayerMask.GetMask("Furniture");
        int combinedLayers = playerLayer | furnitureLayer;

        List<Curio> nearbyCurio = new List<Curio>();

        Collider[] colliders = Physics.OverlapSphere(transform.position, curioRadius, combinedLayers);
        
        foreach (Collider col in colliders)
        {
            foreach (Curio curio in col.gameObject.GetComponents<Curio>())
            {
                nearbyCurio.Add(curio);
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

        float happiness = characterStats.sporeHappiness;
        float minStandingTime = Mathf.Lerp(minHappinessMinStandingTime, maxHappinessMinStandingTime, happiness);
        float maxStandingTime = Mathf.Lerp(minHappinessMaxStandingTime, maxHappinessMaxStandingTime, happiness);
        float randomStandingTime = Random.Range(minStandingTime, maxStandingTime);
        //Debug.Log(randomStandingTime);

        yield return new WaitForSeconds(randomStandingTime);

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
    IEnumerator InteractWithCurio(Curio curio)
    {
        yield return null;

        rb.mass = rbActiveMass;

        previousCurio = curio;

        currentState = WanderingStates.Interacting;

        interactingCurio = curio;
        yield return StartCoroutine(curio.CurioEvent(this));

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
                Debug.LogError("Nowhere for " + gameObject.GetComponent<CharacterStats>().sporeName + " to move in a radius of 100f");
                return Vector3.zero;
            }

            //Recursion :O
            return GetRandomPointNearbyNavMesh(center, radius + 1f);
        }
    }

    Vector3 GetPerpendicularPointNearbyNavMesh(Vector3 center, Vector3 direction, float radius)
    {
        Vector3 perpendicular = new Vector3(-direction.z, 0, direction.x).normalized;

        float randomDistance = (Random.Range(0, 2) == 0) ? -radius : radius;

        Quaternion rotation = Quaternion.AngleAxis(avoidAngleOffset * randomDistance, Vector3.up);
        perpendicular = rotation * perpendicular;

        Vector3 newPoint = center + (perpendicular * randomDistance);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(newPoint, out hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            // If the point is not within the navmesh, handle it accordingly
            if (radius == 100f)
            {
                Debug.LogError("Nowhere perpendicular for " + gameObject.GetComponent<CharacterStats>().sporeName + " to move in a radius of 100f");
                return Vector3.zero;
            }

            // Recursively try again with a larger radius
            return GetPerpendicularPointNearbyNavMesh(center, direction, radius + 1f);
        }
    }
}
