using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletoneBase<GameManager>
{
    public Transform player; //플레이어 transform
    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindWithTag("Player").transform; // 태그로 플레이어 위치 가져옴.
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
