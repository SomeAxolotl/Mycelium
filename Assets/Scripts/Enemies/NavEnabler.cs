using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavEnabler : MonoBehaviour
{
    Transform player;
    ReworkedEnemyNavigation reworkedEnemyNav;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("currentPlayer").transform;
        reworkedEnemyNav = GetComponent<ReworkedEnemyNavigation>();
        InvokeRepeating("CheckDistance", 1f, 1f);
    }
    void CheckDistance()
    {
        if(Vector3.Distance(transform.position, player.position) <= 750f)
        {
            reworkedEnemyNav.enabled = true;
        }
        else if(Vector3.Distance(transform.position, player.position) > 750f)
        {
            reworkedEnemyNav.enabled = false;
        }
    }
}
