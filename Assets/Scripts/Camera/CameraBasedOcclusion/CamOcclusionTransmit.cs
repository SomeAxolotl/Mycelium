using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamOcclusionTransmit : MonoBehaviour
{
    private Transform camTracker;

    private Ray occlusionCheckRay;
    private RaycastHit[] hits = new RaycastHit[10];
    private Vector3 playerDirection;
    private float playerDistance;
    [SerializeField] private LayerMask occludedLayers;

    // Start is called before the first frame update
    void Start()
    {
        //direction of a towards b would be Direction = (b - a).normalized;
        camTracker = GameObject.FindGameObjectWithTag("Camtracker").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        playerDirection = (camTracker.position - transform.position).normalized;
        playerDistance = Vector3.Distance(camTracker.position, transform.position);

        occlusionCheckRay = new Ray(transform.position, playerDirection);

        if(Physics.Raycast(occlusionCheckRay, out RaycastHit hit, playerDistance, occludedLayers))
        {
            Debug.DrawRay(occlusionCheckRay.origin, occlusionCheckRay.direction * playerDistance, Color.blue, 0f, false);
            Debug.Log(hit.collider.gameObject.name, hit.collider.gameObject);
        }
        else
        {
            Debug.DrawRay(occlusionCheckRay.origin, occlusionCheckRay.direction * playerDistance, Color.red, 0f, false);
        }

        
    }
}
