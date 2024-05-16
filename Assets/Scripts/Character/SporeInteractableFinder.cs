using System.Collections;
using System.Collections.Generic;
using RonaldSunglassesEmoji.Interaction;
using UnityEngine;

public class SporeInteractableFinder : MonoBehaviour
{
    public List<GameObject> interactableObjects = new List<GameObject>();
    [SerializeField] private float interactAngle = 90f;
    public GameObject closestInteractableObject;// {get; private set;}
    IInteractable previousInteractable = null;

    void Update()
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
                if (interactable != previousInteractable)
                {
                    interactable?.CreateTooltip(closestInteractableObject);
                }
                previousInteractable = interactable;
            }
        }
        else
        {
            if (closestInteractableObject != null)
            {
                closestInteractableObject.GetComponent<IInteractable>().DestroyTooltip(closestInteractableObject);
                previousInteractable = null;
            }
            closestInteractableObject = null;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.GetType() != typeof(SphereCollider))
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Weapon") || other.gameObject.layer == LayerMask.NameToLayer("Interactable") || other.gameObject.tag == "Player" )
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
    
    void OnTriggerExit(Collider other)
    {
        if (other.GetType() != typeof(SphereCollider))
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Weapon") || other.gameObject.layer == LayerMask.NameToLayer("Interactable") || other.gameObject.tag == "Player" )
            {
                TriggerExited(other);
            }
        }
    }

    public void TriggerExited(Collider other, bool isFromInteracting = false)
    {
        if (closestInteractableObject != null)
        {
            closestInteractableObject.GetComponent<IInteractable>().DestroyTooltip(closestInteractableObject, isFromInteracting);
            previousInteractable = null;
        }
        interactableObjects.Remove(other.gameObject);
        closestInteractableObject = null;
    }
}
