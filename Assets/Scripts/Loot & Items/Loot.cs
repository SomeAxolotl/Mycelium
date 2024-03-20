using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Loot : ScriptableObject
{
    [Tooltip("The prefab of the loot to spawn")]
    public GameObject LootPrefab;
    
    [Min(0)][Tooltip("The percent chance for this loot option to drop. MUST BE < 100!")]
    public int DropChance;
}
