using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlatformType
{
    Land,
    Load,
    River,
    Train,
}

public class BasePlatformSO : ScriptableObject
{
    [Space(10)] public PlatformType type;

    public int maxContinuousIndex;
}
