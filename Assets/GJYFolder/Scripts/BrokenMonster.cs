using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenMonster : MonoBehaviour
{
    private Vector3[] _piecesPosition;

    private void Awake()
    {
        _piecesPosition = new Vector3[transform.childCount];

        for(int i = 0; i < _piecesPosition.Length; i++)
            _piecesPosition[i] = transform.GetChild(i).localPosition;
    }

    private void OnEnable()
    {
        ResetPositions();

        Invoke("ReturnPool", 3f);
    }

    private void ResetPositions()
    {
        for(int i = 0; i< _piecesPosition.Length; i++)
            transform.GetChild(i).localPosition = _piecesPosition[i];
    }

    private void ReturnPool()
    {
        ObjectPoolManager.ReturnObject(gameObject.name, gameObject);
    }
}
