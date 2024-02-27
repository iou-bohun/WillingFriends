using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Platform/Load", fileName = "LoadPlatform")]
public class LoadPlatformSO : BasePlatformSO
{
    [Header("Object")]
    public GameObject[] carPrefabs;

    [Header("Spawn")]
    public float minSpawnDelay;
    public float maxSpawnDelay;

    [Header("Spawn")]
    public float minSpeed;
    public float maxSpeed;
}
