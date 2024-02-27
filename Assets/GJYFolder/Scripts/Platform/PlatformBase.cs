using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlatformType
{
    Land_01,
    LoadPair_01,
    LoadSingle,
}

public class PlatformBase : MonoBehaviour
{
    public PlatformType platformType;

    [field: SerializeField] public string Tag { get; private set; }    

    [SerializeField] protected BasePlatformSO _platformSO;    

    public virtual void Init() { }
}
