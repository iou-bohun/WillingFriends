using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : SingletoneBase<Managers>
{
    public static Action InitEvent; //매니저들의 첫 초기화를
    public static Action ClearEvent; //매니저들의 기록 삭제 후 초기화
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        AddAllInitialize();
    }
    public void AddAllInitialize() //강제 구독
    {
        Debug.Log("Managers 구독");
        InitEvent += GameManager.Instance.Initialize;
        InitEvent += ObjectPoolManager.Instance.Initialize;
        InitEvent += SoundManager.Instance.Initialize;
    }
    public void CallInitEvent()
    {
        InitEvent?.Invoke();
    }
    public void CallClearEvent()
    {
        ClearEvent?.Invoke();
    }
}
