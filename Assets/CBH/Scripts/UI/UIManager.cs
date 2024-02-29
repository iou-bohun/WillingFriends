using System;
using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
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
        GameObject go = ObjectPoolManager.GetObject("StartUI", _root);
        go.transform.position = GameManager.Instance.player.position + Vector3.up * 3.2f;
    }

    private void Init_Hud()
    {
        hudUI = ObjectPoolManager.GetObject("HUD", _root);
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
