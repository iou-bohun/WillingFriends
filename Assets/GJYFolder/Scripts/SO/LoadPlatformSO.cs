using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Platform/Load", fileName = "LoadPlatform")]
public class LoadPlatformSO : BasePlatformSO
{
    [Header("Delay")]
    public float minSpawnDelay;
    public float maxSpawnDelay;

    [Header("Speed")]
    public float minSpeed;
    public float maxSpeed;
}
