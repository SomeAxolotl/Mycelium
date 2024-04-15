using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomiesCurio : Curio
{
    public int zoomiesCount = 3;
    public float radius = 5f;
    public int numPoints = 10; 

    public override IEnumerator DoEvent(WanderingSpore wanderingSpore)
    {
        
        // Calculate the angle between each point
        float angleIncrement = 360f / numPoints;

        for (int z = 0; z < zoomiesCount; z++)
        {
            List<Vector3> points = new List<Vector3>();
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
