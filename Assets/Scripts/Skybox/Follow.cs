using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField] Transform followThis;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FindThePlayer());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(followThis.transform.position.x, 0, followThis.transform.position.z);
    }

    IEnumerator FindThePlayer()
    {
        if (followThis == null)
        {
            followThis = GameObject.FindWithTag("currentPlayer").GetComponent<Transform>();
            yield return null;
        }
    }
}
