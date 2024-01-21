using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> mountainPrefab;

    [SerializeField] private List<GameObject> mountains;

    [SerializeField] private float scaleWidth, scaleHeight, distance;

    [ContextMenu("CreateMountains")]
    private void CreateMountains()
    {
        foreach(var mountain in mountains)
        {
            DestroyImmediate(mountain);
        }
        mountains = new List<GameObject>();

        int prefabIndex = 0;
        float angle = 0f;
        for (int i = 0; i < 8; i++)
        {
            mountains.Add(Instantiate(mountainPrefab[prefabIndex], transform));
            mountains[i].transform.Rotate(new Vector3(0f, angle, 0f));

            angle += 45f;

            prefabIndex++;
            if(prefabIndex>=mountainPrefab.Count)
                prefabIndex=0;
        }
        SetMountainScale();
    }

    [ContextMenu("SetMountainScale")]
    private void SetMountainScale()
    {
        foreach (var mt in mountains)
        {
            mt.transform.localScale = new Vector3(scaleWidth, scaleHeight, 1f);
            float spriteLength = mountains[0].GetComponent<SpriteRenderer>().bounds.size.x;
            distance = spriteLength/2f + (Mathf.Sqrt(2)/2) * spriteLength;
            mt.transform.position = distance * mt.transform.forward;
        }
    }

    private void OnValidate()
    {
        if(mountains.Count == 8)
            SetMountainScale();
    }

}
