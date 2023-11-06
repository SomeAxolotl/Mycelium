using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleWalking : MonoBehaviour
{
    private bool canWander = true;
    private float patrolCooldown = 1f;
    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;
    private bool isWalking = false;
    public LayerMask obstacleLayer;
    IEnumerator wander;
    IEnumerator restartWander;
    // Start is called before the first frame update
    void Start()
    {
        wander = this.Wander();
        restartWander = this.RestartWander();
    }

    // Update is called once per frame
    void Update()
    {

        if (gameObject.tag == "Player" && canWander)
        {
            patrolCooldown -= Time.deltaTime;
            if (patrolCooldown <= 0)
            {
                StartCoroutine(Wander());
            }
        }

        if (isRotatingRight == true)
        {
            transform.Rotate(transform.up * Time.deltaTime * 100f);
        }

        if (isRotatingLeft == true)
        {
            transform.Rotate(transform.up * Time.deltaTime * -100f);
        }

        if (isWalking == true)
        {
            if (!Physics.Raycast(transform.position, transform.forward, 2f, obstacleLayer))
            {
                transform.position += transform.forward * 5f * Time.deltaTime;
            }
            else
            {
                StartCoroutine(RestartWander());
            }
        }
    }


    IEnumerator Wander()
    {
        canWander = false;
        int rotTime = Random.Range(1, 3);
        int rotateWait = Random.Range(1, 3);
        int rotateLorR = Random.Range(1, 2);
        int walkWait = Random.Range(1, 3);
        int walkTime = Random.Range(1, 4);

        yield return new WaitForSeconds(walkWait);
        isWalking = true;
        yield return new WaitForSeconds(walkTime);
        isWalking = false;
        yield return new WaitForSeconds(rotateWait);
        if (rotateLorR == 1)
        {
            isRotatingRight = true;
            yield return new WaitForSeconds(rotTime);
            isRotatingRight = false;
        }
        if (rotateLorR == 2)
        {
            isRotatingLeft = true;
            yield return new WaitForSeconds(rotTime);
            isRotatingLeft = false;
        }
        patrolCooldown = 1f;
        canWander = true;
}
    IEnumerator RestartWander()
    {
        StopCoroutine(wander);
        wander = Wander();
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y + 180f * Time.deltaTime, transform.rotation.z);
        yield return new WaitForSeconds(1f);
        canWander = true;
        patrolCooldown = 1f;
    }
}
