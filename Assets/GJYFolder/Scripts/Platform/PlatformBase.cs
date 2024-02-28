using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlatformType
{
    Land_01,
    LoadPair_01,
    LoadSingle,
    River_Log,
    River_Lotus
}

public class PlatformBase : MonoBehaviour
{
    public PlatformType platformType;

    [field: SerializeField] public string Tag { get; private set; }

    public virtual void Init() { }
}
