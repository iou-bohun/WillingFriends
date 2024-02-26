using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlatformType
{
    Land = 0,
    PairLoad = 2,
    SingleLoad = 4,
    River,
    Train,    
}

public class BasePlatformSO : ScriptableObject
{
    [Header("Info")]
    [Space(10)] public PlatformType type;

    public int maxContinuousIndex;
}
