using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Platform/Land", fileName = "LandPlatform")]
public class LandPlatformSO : BasePlatformSO
{
    [Range(0, 4)] public int minTrees;
    [Range(4, 8)] public int maxTrees;
}
