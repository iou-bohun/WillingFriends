using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Platform/River", fileName = "RiverPlatform")]
public class RiverPlatformSO : BasePlatformSO
{
    [Header("River Lotus")]
    public GameObject lotusPrefab;

    [Header("Delay")]
    public float minSpawnDelay;
    public float maxSpawnDelay;

    [Header("Speed")]
    public float minSpeed;
    public float maxSpeed;
}
