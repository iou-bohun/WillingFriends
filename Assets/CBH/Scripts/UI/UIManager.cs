using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletoneBase<GameManager>
{
    //UI 업데이트용 Action
    public Action uiUpdateEvent;

    [SerializeField] Transform _root;
    private TextMeshProUGUI[] hudTexts;
    private GameObject hudUI;
    private Vector3 playerPosition;

    [SerializeField] private GameObject[] UIPrefabs;

    private void Start()
    {
        playerPosition = GameManager.Instance.player.position;
        Init_StartUI();  
        Init_Hud();

        uiUpdateEvent += UpdateUI;
    }

    private void UpdateUI()
    {
        Debug.Log("UpdateUI");
        hudTexts[0].text = GameManager.Instance.Score.ToString();
        hudTexts[1].text = GameManager.Instance.Coin.ToString();
    }

    private void Init_StartUI()
    {
        GameObject go = Instantiate(UIPrefabs[0], _root);
        go.transform.position = playerPosition + Vector3.up * 3.2f;
    }

    private void Init_Hud()
    {
        hudUI = Instantiate(UIPrefabs[1], _root);
        hudUI.transform.position = Vector3.zero;
        hudTexts = hudUI.GetComponentsInChildren<TextMeshProUGUI>();
        hudTexts[0].text = GameManager.Instance.Score.ToString();
        hudTexts[1].text = GameManager.Instance.Coin.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            uiUpdateEvent();
        }
    }

}
