using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    // 일단 필요 없어 보임.
    [field: SerializeField] public string Tag { get; private set; }

    public void DestroyTree()
    {
        gameObject.SetActive(false);
    }
}
