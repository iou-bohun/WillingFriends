using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameManager()
    {
        GameObject go = new GameObject("GameManager");
        go.AddComponent<GameManager>();
    }
    private void Awake()
    {
        Managers.InitEvent += Initialize;
    }

    private void Initialize()
    {
        Debug.Log("gameManger »ý¼º");
    }
}
