using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBase : MonoBehaviour
{
    protected BasePlatformSO _platformSO;    

    public void Init()
    {
        // To Do - PlatformSO 도 생성될 때 마다 갱신 해줘야 함...

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
