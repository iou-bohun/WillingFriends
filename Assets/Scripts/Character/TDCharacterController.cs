using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDCharacterController : MonoBehaviour
{
    public event Action<Vector2> OnMoveEvent;
    public event Action<Vector3> OnAttackEvent;

    public bool moveable = true;


    public void CallMoveEvent(Vector2 direction)
    {
        OnMoveEvent?.Invoke(direction);
    }

    public void CallAttackEvent(Vector3 position)
    {
        OnAttackEvent?.Invoke(position);
    }
}
