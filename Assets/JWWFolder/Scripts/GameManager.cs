using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletoneBase<GameManager>
{
    protected override void Awake()
    {
        base.Awake();
        //Managers.InitEvent += Initialize;
    }

    public void Initialize()
    {
        Debug.Log("gameManger »ý¼º");
    }
}
