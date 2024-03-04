using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletoneBase<GameManager>
{
    [SerializeField] GameObject _reBtn;
    public Player player; //플레이어 transform

    public event Action OnPlayerDie;
    public event Action<int> OnPlayerMove;

    private int coin = 0;
    private int score = 0;
    public int Coin { get { return coin; } set { coin = value; } }
    public int Score { get { return score; } set { score = value; } }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public override void Init()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>(); // 태그로 플레이어 위치 가져옴.                
    }

    public void CallPlayerMove(int value)
    {
        OnPlayerMove?.Invoke(value);
    }

    public void GameOver()
    {
        // To Do - GameOver UI 띄우기
        OnPlayerDie?.Invoke();
        Instantiate(_reBtn.gameObject);

        Clear();
    }

    public override void Clear()
    {
        OnPlayerMove = null;
        OnPlayerDie = null;
    }
}
