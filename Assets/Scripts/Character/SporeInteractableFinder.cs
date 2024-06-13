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
                if (interactable != previousInteractable && IsAcceptableInteractable(closestInteractableObject) && GlobalData.canShowTooltips == true)
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
            if (IsAcceptableInteractable(other.gameObject))
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

    void OnTriggerEnter(Collider other)
    {
        Curio otherCurio = other.GetComponent<Curio>();
        if (otherCurio != null)
        {
            otherCurio.isPlayerInInteractTextRange = true;
            if (otherCurio is not DanceCurio || (otherCurio is DanceCurio && transform.parent.GetComponent<PlayerController>().canDance))
            {
                otherCurio.StartCoroutine(otherCurio.PopInteractCanvas(true));
            }
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.GetType() != typeof(SphereCollider))
        {
            if (IsAcceptableInteractable(other.gameObject))
            {
                TriggerExited(other);
            }
        }

        Curio otherCurio = other.GetComponent<Curio>();
        if (otherCurio != null)
        {
            otherCurio.StartCoroutine(otherCurio.PopInteractCanvas(false));
            otherCurio.isPlayerInInteractTextRange = false;
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

    bool IsAcceptableInteractable(GameObject interactableObject)
    {
        if (interactableObject.tag == "Weapon" || interactableObject.layer == LayerMask.NameToLayer("Interactable") || interactableObject.tag == "Player" )
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
