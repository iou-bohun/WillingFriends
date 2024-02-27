using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : SingletoneBase<Managers>
{
    public static Action InitEvent;
    public static Action ClearEvent;
    // Start is called before the first frame update
    void Start()
    {
        
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
