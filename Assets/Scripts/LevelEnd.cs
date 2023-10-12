using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "currentPlayer")
        {
            SceneManager.LoadScene("HubWorldPlaceholder");
        }

        other.gameObject.transform.position = new Vector3(0, 1.5f, 1.0f);
    }
}
