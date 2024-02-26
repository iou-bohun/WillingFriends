using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPlatformSO : BasePlatformSO
{
    [Header("Object")]
    public GameObject[] carPrefabs;

    [Header("Spawn")]
    public float minSpawnDelay;
    public float maxSpawnDelay;
}
