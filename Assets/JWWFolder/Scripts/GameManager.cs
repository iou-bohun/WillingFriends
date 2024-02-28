using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletoneBase<GameManager>
{
    protected override void Awake()
    {
        base.Awake();
        Debug.Log("GameManager awake");
        //Managers.InitEvent += Initialize;
    }
    public void Start()
    {
        Debug.Log("gameManager start");
    }

    public void Initialize()
    {
        Debug.Log("gameManger ");
    }
}
