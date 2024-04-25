using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedPosistion : MonoBehaviour
{
    // The position to lock the object to
    public Vector3 lockedPosition;

    void Update()
    {
        // Lock the object's position to the specified position
        transform.position = lockedPosition;
    }
}
