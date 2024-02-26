using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBase : MonoBehaviour
{
    protected BasePlatformSO _platformSO;    

    public void Init()
    {
        // To Do - PlatformSO �� ������ �� ���� ���� ����� ��...

    }

    private void OnDisable()
    {
        Clear();
    }

    protected virtual void Clear()
    {
        _platformSO = null;
    }
}
