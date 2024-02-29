using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] Transform _root;

    private void Start()
    {
        Init_StartUI();  
    }
    private void Init_StartUI()
    {
        GameObject go = ObjectPoolManager.GetObject("StartUI", _root);
        go.transform.position = GameManager.Instance.player.position + Vector3.up * 3.2f;
    }

    private void Init_Hud()
    {

    }
}
