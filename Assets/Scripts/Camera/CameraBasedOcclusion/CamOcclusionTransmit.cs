using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CamOcclusionTransmit : MonoBehaviour
{
    private Transform camTracker;

    private Ray occlusionCheckRay;
    private RaycastHit[] hits = new RaycastHit[100];
    private int numOfHits;

    private List<CamOcclusionReceive> latestReceiveScripts = new List<CamOcclusionReceive>();
    private List<CamOcclusionReceive> toBeFadedIn = new List<CamOcclusionReceive>();

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

        CastOcclusionRay();
    }

    void CastOcclusionRay()
    {
        numOfHits = Physics.SphereCastNonAlloc(occlusionCheckRay, 0.5f, hits, playerDistance, occludedLayers);



        if (numOfHits > 0)
        {
            Debug.DrawRay(occlusionCheckRay.origin, occlusionCheckRay.direction * playerDistance, Color.blue * 2, 0f, false);
            latestReceiveScripts = FindTheReceiver();
            foreach(CamOcclusionReceive script in latestReceiveScripts)
            {
                script.StartFadeOut();
                if(toBeFadedIn.Contains(script) == false)
                {
                    toBeFadedIn.Add(script);
                }
            }
        }
        else
        {
            Debug.DrawRay(occlusionCheckRay.origin, occlusionCheckRay.direction * playerDistance, Color.red * 2, 0f, false);
            foreach (CamOcclusionReceive script in toBeFadedIn)
            {
                script.StartFadeIn();
                toBeFadedIn.Remove(script);
            }
        }
    }

    List<CamOcclusionReceive> FindTheReceiver()
    {
        GameObject searchObject;
        List<GameObject> objectsWithScript = new List<GameObject>();
        List<CamOcclusionReceive> receiveScripts = new List<CamOcclusionReceive>();

        //Add objects to the list that contain the Receive Script (This WILL add duplicates)
        for(int i = 0; i < numOfHits; i++)
        {
            searchObject = hits[i].transform.gameObject;

            if (searchObject.GetComponent<CamOcclusionReceive>() != null)
            {
                objectsWithScript.Add(searchObject);
            }

            while (searchObject.transform.parent != null)
            {
                if (searchObject.GetComponent<CamOcclusionReceive>() != null)
                {
                    objectsWithScript.Add(searchObject);
                    break;
                }

                searchObject = searchObject.transform.parent.gameObject;
            }
        }

        //Remove the Duplicates
        objectsWithScript = objectsWithScript.Distinct().ToList();

        //Get the references to the Receive Scripts
        foreach(GameObject singleObject in objectsWithScript)
        {
            receiveScripts.Add(singleObject.GetComponent<CamOcclusionReceive>());
            //Debug.Log(singleObject.name, singleObject);
        }

        return receiveScripts;
    }
}
