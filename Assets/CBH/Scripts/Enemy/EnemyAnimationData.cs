using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class EnemyAnimationData 
{
    [SerializeField] private string jumpParameterName = "Jump";
    [SerializeField] private string idleParameterName = "Idle";
    public int JumParameterName { get; private set; }
    public int IdleParameterName { get; private set; }

    public void Initialize()
    {
        JumParameterName = Animator.StringToHash(jumpParameterName);
        IdleParameterName = Animator.StringToHash(idleParameterName);

    }
}
