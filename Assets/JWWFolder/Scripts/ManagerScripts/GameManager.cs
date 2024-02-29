using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletoneBase<GameManager>
{
    public Transform player; //플레이어 transform

    public Action OnPlayerDie;

    private int coin = 0;
    private int score = 0;
    public int Coin { get { return coin; } set { coin = value; } }
    public int Score { get { return score; } set { score = value; } }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        player = GameObject.FindWithTag("Player").transform; // 태그로 플레이어 위치 가져옴.                
    }

    public void GameOver()
    {
        // To Do - GameOver UI 띄우기
        OnPlayerDie?.Invoke();

        // Temp - SceneLoad 즉발        
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
