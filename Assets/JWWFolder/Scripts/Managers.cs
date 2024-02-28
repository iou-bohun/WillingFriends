using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : SingletoneBase<Managers>
{
    public static Action InitEvent;
    public static Action ClearEvent;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        AddAllInitialize();
    }
    public void AddAllInitialize() //강제 구독
    {
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
