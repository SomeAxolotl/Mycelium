using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour
{
    public GameObject chunk1;
    public GameObject chunk2;
    public GameObject chunk3;
    public int chunk;
    // Start is called before the first frame update
    void Start()
    {
      chunk = Random.Range(1, 4);  
    }

    // Update is called once per frame
    void Update()
    {
        if(chunk == 1)
        {
            chunk1.SetActive(true);
        }
                  
        if(chunk == 2)
        {
            chunk2.SetActive(true);
        }

        if(chunk == 3)
        {
            chunk3.SetActive(true);
        }
    }
}
