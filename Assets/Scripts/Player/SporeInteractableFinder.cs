using System.Collections;
using System.Collections.Generic;
using RonaldSunglassesEmoji.Interaction;
using UnityEngine;

public class SporeInteractableFinder : MonoBehaviour
{
    public List<GameObject> interactableObjects = new List<GameObject>();
    [SerializeField] private float interactAngle = 90f;
    public GameObject closestInteractableObject;// {get; private set;}

    void Update()
    {   
        if (gameObject.tag == "currentPlayer")
        {
            if (interactableObjects.Count > 0)
            {
                float lowestDistanceToPlayer = GetComponent<SphereCollider>().radius;
                foreach (GameObject interactableObject in interactableObjects)
                {
                    float distanceToPlayer = Vector3.Distance(transform.position, interactableObject.transform.position);
                    if (distanceToPlayer < lowestDistanceToPlayer)
                    {
                        lowestDistanceToPlayer = distanceToPlayer;
                        closestInteractableObject = interactableObject;
                    }
                }

                if (closestInteractableObject != null)
                {
                    IInteractable interactable = closestInteractableObject.GetComponent<IInteractable>();
                    if (interactable != null)
                    {
                        interactable.CreateTooltip(closestInteractableObject);
                    }
                }
            }
            else
            {
                if (closestInteractableObject != null)
                {
                    closestInteractableObject.GetComponent<IInteractable>().DestroyTooltip(closestInteractableObject);
                }
                closestInteractableObject = null;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.GetType() != typeof(SphereCollider))
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Weapon") || other.gameObject.layer == LayerMask.NameToLayer("Interactable") || other.gameObject.tag == "Player" )
            {
                //Debug.Log(other.gameObject.name);
                if (gameObject.tag == "currentPlayer")
                {
                    Vector3 directionToPlayer = (other.transform.position - transform.position).normalized;
                    float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

                    if (angleToPlayer < interactAngle)
                    {
                        if (!interactableObjects.Contains(other.gameObject))
                        {
                            interactableObjects.Add(other.gameObject);
                        }
                    }
                    else
                    {
                        interactableObjects.Remove(other.gameObject);
                    }
                }
            }
        }
    }
    
    //also being called from playercontroller when the interactable is destroyed
    public void OnTriggerExit(Collider other)
    {
        if (other.GetType() != typeof(SphereCollider))
        {
            if (closestInteractableObject != null)
            {
                closestInteractableObject.GetComponent<IInteractable>().DestroyTooltip(closestInteractableObject);
            }
            interactableObjects.Remove(other.gameObject);
            closestInteractableObject = null;
        }
    }
}
