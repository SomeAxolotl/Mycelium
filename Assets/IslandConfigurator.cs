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

        // 2. Minibosses or Treasure Chests
        if (Random.value > 0.5f)
        {
            SetRandomMiniboss();
            SetAllInactive(treasureChests);
        }
        else
        {
            SetRandomActive(treasureChests);
            SetAllInactive(minibosses);
        }

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

    private void SetRandomMiniboss()
    {
        if (minibosses != null && minibosses.Length > 0)
        {
            int randomIndex = Random.Range(0, minibosses.Length);
            minibosses[randomIndex].SetActive(true);
        }
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
