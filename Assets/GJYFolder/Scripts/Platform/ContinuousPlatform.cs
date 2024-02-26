using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousPlatform : PlatformBase
{
    [field: SerializeField] public bool IsEssential { get; private set; }
    [field: SerializeField] public bool IsMid { get; private set; }
    [field: SerializeField] public bool IsLast { get; private set; }    

    [field: SerializeField] public string NextPair { get; private set; }
}
