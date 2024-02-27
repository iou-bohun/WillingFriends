using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletoneBase<GameManager>
{
    private void Awake()
    {
        Managers.InitEvent += Initialize;
    }

    public void Initialize()
    {
        Debug.Log("gameManger »ý¼º");
    }
}
