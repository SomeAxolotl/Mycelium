using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandConfigurator : MonoBehaviour
{
    // References to island objects
    [SerializeField] private GameObject activeVolcano;
    [SerializeField] private GameObject[] minibosses;
    [SerializeField] private GameObject[] treasureChests;
    [SerializeField] private Material[] colorMaterials;
    [SerializeField] private GameObject[] trees;
    [SerializeField] private GameObject[] hittableMushrooms;
    private Terrain terrain;

    void Start()
    {
        terrain = GetComponent<Terrain>();
        ConfigureIslandObjects();
    }

    private void ConfigureIslandObjects()
    {
        // 1. Active Volcano
        if (activeVolcano != null)
        {
            activeVolcano.SetActive(Random.value > 0.5f);
        }

        // 2. Minibosses and Treasure Chests
        ActivateRandomItems(minibosses, treasureChests, 5);

        // 3. Color Code
        if (terrain != null && colorMaterials.Length > 0)
        {
            Material randomMaterial = colorMaterials[Random.Range(0, colorMaterials.Length)];
            AddMaterialLayerToTerrain(randomMaterial);
        }

        // 4. Trees
        SetRandomActive(trees);

        // 5. Hittable Mushrooms
        SetRandomActive(hittableMushrooms);
    }

    private void SetRandomActive(GameObject[] objects)
    {
        if (objects != null && objects.Length > 0)
        {
            foreach (var obj in objects)
            {
                obj.SetActive(Random.value > 0.5f);
            }
        }
    }

    private void SetAllInactive(GameObject[] objects)
    {
        if (objects != null && objects.Length > 0)
        {
            foreach (var obj in objects)
            {
                obj.SetActive(false);
            }
        }
    }

    private void ActivateRandomItems(GameObject[] array1, GameObject[] array2, int count)
    {
        List<GameObject> combinedList = new List<GameObject>();
        if (array1 != null)
        {
            combinedList.AddRange(array1);
        }
        if (array2 != null)
        {
            combinedList.AddRange(array2);
        }

        SetAllInactive(combinedList.ToArray());

        if (combinedList.Count > 0)
        {
            int itemsToActivate = Mathf.Min(count, combinedList.Count);
            List<GameObject> selectedItems = new List<GameObject>();

            while (selectedItems.Count < itemsToActivate)
            {
                int randomIndex = Random.Range(0, combinedList.Count);
                GameObject selectedItem = combinedList[randomIndex];
                if (!selectedItems.Contains(selectedItem))
                {
                    selectedItems.Add(selectedItem);
                }
            }

            foreach (var item in selectedItems)
            {
                item.SetActive(true);
            }
        }
    }

    private bool AnyActive(GameObject[] objects)
    {
        if (objects != null && objects.Length > 0)
        {
            foreach (var obj in objects)
            {
                if (obj.activeSelf)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void AddMaterialLayerToTerrain(Material material)
    {
        if (terrain != null && material != null)
        {
            TerrainLayer[] terrainLayers = terrain.terrainData.terrainLayers;
            List<TerrainLayer> newTerrainLayers = new List<TerrainLayer>(terrainLayers);

            TerrainLayer newLayer = new TerrainLayer
            {
                diffuseTexture = (Texture2D)material.mainTexture,
                tileSize = new Vector2(10, 10) // Adjust this as needed
            };
            newTerrainLayers.Add(newLayer);

            terrain.terrainData.terrainLayers = newTerrainLayers.ToArray();
        }
    }
}
