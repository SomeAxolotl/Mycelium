using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : MonoBehaviour
{
    [SerializeField] float shootingDistance = 7f;
    [SerializeField] float speedArrow = 5f;
    [SerializeField] float fireRate = 3f;
    public GameObject arrow;
    GameObject target;
    bool canShoot = true;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        if (canShoot) 
        {
            canShoot = false;
            //Coroutine for delay between shooting
            StartCoroutine("AllowToShoot");
            //array with enemies
            //you can put in start, iff all enemies are in the level at beginn (will be not spawn later)
            GameObject[] allTargets = GameObject.FindGameObjectsWithTag("Player");
            if (allTargets != null)
            {
                target = allTargets[0];
                //look for the closest
                foreach (GameObject tmpTarget in allTargets)
                {
                    if (Vector3.Distance(transform.position, tmpTarget.transform.position) < Vector3.Distance(transform.position, target.transform.position))
                    {
                        target = tmpTarget;
                    }
                }
                //shoot if the closest is in the fire range
                if (Vector3.Distance(transform.position, target.transform.position) < shootingDistance)
                {
                    Fire();
                }
            }
        }
         
    }

    void Fire ()
    {
        Vector3 direction = new Vector3(target.transform.position.x, target.transform.position.y + 2, target.transform.position.z) - transform.position;
        //link to spawned arrow, you dont need it, if the arrow has own moving script
        GameObject tmpArrow = Instantiate(arrow, transform.position, transform.rotation);
        tmpArrow.transform.right = direction;
        tmpArrow.GetComponent<Rigidbody>().velocity = direction.normalized * speedArrow;
    }
 
    IEnumerator AllowToShoot ()
    {
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }
}
