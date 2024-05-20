using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndergrowthMovement : MonoBehaviour
{
    public float moveSpeed = 4.5f;
    public float lifeTime = 3;
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.1f;

    public Collider coll;
    private Rigidbody rb;
    Vector3 forward;

    [HideInInspector] public bool hitSomething = false;

    void Start(){
        rb = GetComponent<Rigidbody>();
        forward = transform.TransformDirection(Vector3.forward);
        StartCoroutine(Timer());
    }

    void MoveVine(){
        RaycastHit groundHit;
        if(Physics.Raycast(transform.position, Vector3.down, out groundHit, groundCheckDistance, groundLayer)){
            rb.velocity = forward * moveSpeed;
            return;
        }
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }

    IEnumerator Timer(){
        float currTime = lifeTime;
        while(currTime > 0 && hitSomething == false){
            currTime -= Time.deltaTime;
            MoveVine();
            yield return null;
        }
        StartCoroutine(Destroy());
    }

    IEnumerator Destroy(){
        coll.enabled = false;
        yield return new WaitForSeconds(5);
        transform.parent.GetComponent<UndergrowthManager>().roots.Remove(gameObject);
        Destroy(gameObject);
    }
}
