using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] GameObject[] _initCreatePrefabs;

    private void Awake()
    {
        for (int i = 0; i < _initCreatePrefabs.Length; i++)
            Instantiate(_initCreatePrefabs[i]);
    }
}
