using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : SingletoneBase<Managers>
{
    private GameManager gameManager = new GameManager();
    public GameManager GameManager {  get { return gameManager; } }

    public static Action InitEvent;
    public static Action ClearEvent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void CallInitEvent()
    {
        InitEvent?.Invoke();
    }
    public static void CallClearEvent()
    {
        ClearEvent?.Invoke();
    }
}
