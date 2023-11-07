using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleWalking : MonoBehaviour
{
    bool canWander = true;
    float wanderCooldown = 1f;
    bool isRotatingLeft = false;
    bool isRotatingRight = false;
    bool isWalking = false;
    int rotTime;
    int walkTime;
    public LayerMask obstacleLayer;
    public IEnumerator wander;
    IEnumerator restartWander;
    IEnumerator stopWander;
    // Start is called before the first frame update
    void Start()
    {
        wander = this.Wander();
        restartWander = this.RestartWander();
        stopWander = this.StopWander();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == "Player" && canWander)
        {
            wanderCooldown -= Time.deltaTime;
            if (wanderCooldown <= 0)
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
        rotTime = Random.Range(1, 3);
        int rotateWait = Random.Range(1, 3);
        int rotateLorR = Random.Range(1, 2);
        int walkWait = Random.Range(1, 3);
        walkTime = Random.Range(1, 4);

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
        wanderCooldown = 1f;
        canWander = true;
    }
    IEnumerator RestartWander()
    {
        StopCoroutine(wander);
        wander = Wander();
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y + 180f * Time.deltaTime, transform.rotation.z);
        yield return new WaitForSeconds(1f);
        canWander = true;
        wanderCooldown = 1f;
    }
    public IEnumerator StopWander()
    {
        StopCoroutine(wander);
        wander = Wander();
        isWalking = false;
        isRotatingLeft = false;
        isRotatingRight = false;
        rotTime = 0;
        walkTime = 0;
        canWander = true;
        wanderCooldown = 1f;
        yield return null;
    }
}
