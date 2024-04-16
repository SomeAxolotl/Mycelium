using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomiesCurio : Curio
{
    [SerializeField] int zoomiesCount = 3;
    [SerializeField] float radius = 5f;
    [SerializeField] int numPoints = 10; 

    public override IEnumerator DoEvent(WanderingSpore wanderingSpore)
    {
        Debug.Log("ZOOMIES");
        
        // Calculate the angle between each point
        float angleIncrement = 360f / numPoints;

        List<Vector3> points = new List<Vector3>();
        for (int z = 0; z < zoomiesCount; z++)
        {
            for (int i = 0; i < numPoints; i++)
            {
                float angle = i * angleIncrement;

                float radians = angle * Mathf.Deg2Rad;

                float x = transform.position.x + radius * Mathf.Cos(radians);
                float y = transform.position.y + radius * Mathf.Sin(radians);

                points.Add(new Vector3(x, y, 0f));
            }
        }

        yield return new WaitForSeconds(1.5f * zoomiesCount);

    }
}
