using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavEnabler : MonoBehaviour
{
    Transform player;
    ReworkedEnemyNavigation reworkedEnemyNav;
    [SerializeField] private float enableDistance = 75f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("currentPlayer").transform;
        reworkedEnemyNav = GetComponent<ReworkedEnemyNavigation>();
        InvokeRepeating("CheckDistance", 1f, 1f);
    }
    void CheckDistance()
    {
        if(Vector3.Distance(transform.position, player.position) <= enableDistance)
        {
            reworkedEnemyNav.enabled = true;
        }
        else if(Vector3.Distance(transform.position, player.position) > enableDistance)
        {
            reworkedEnemyNav.enabled = false;
        }
    }
}
