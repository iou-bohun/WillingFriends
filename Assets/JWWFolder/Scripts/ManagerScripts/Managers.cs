using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : SingletoneBase<Managers>
{
    public static Action InitEvent; //매니저들의 첫 초기화를
    public static Action ClearEvent; //매니저들의 기록 삭제 후 초기화

    protected override void Awake()
    {
        base.Awake();

    }

    public void CallInitEvent()
    {
        InitEvent?.Invoke();
    }

    public void CallClearEvent()
    {
        ClearEvent?.Invoke();
        _reset = true;
    }

    public bool _reset = false;
}
