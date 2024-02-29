using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletoneBase<GameManager>
{
    public Transform player; //플레이어 transform

    private int coin = 0;
    private int score = 0;
    public int Coin { get { return coin; } set { coin = value; } }  
    public int Score { get { return score; } set { score = value; } }   
    
    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindWithTag("Player").transform; // 태그로 플레이어 위치 가져옴.
        DontDestroyOnLoad(gameObject);
        Debug.Log("GameManager awake");
    }
    public void Start()
    {
        Debug.Log("gameManager start");
    }

    public void Initialize()
    {
        Debug.Log("gameManger ");
    }

    public void GameOver()
    {
        // To Do - GameOver UI 띄우기

        // Temp - SceneLoad 즉발
        Managers.Instance.CallClearEvent();     
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        //Managers.Instance.AddAllInitialize();
    }
}
