using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class EnemyAnimationData 
{
    [SerializeField] private string jumpParameterName = "Jump";
    [SerializeField] private string idleParameterName = "Idle";
    [SerializeField] private string goundParameterName = "Ground";
    public int JumParameterName { get; private set; }
    public int IdleParameterName { get; private set; }
    public int GroundParameterName { get; private set; }

    public void Initialize()
    {
        JumParameterName = Animator.StringToHash(jumpParameterName);
        IdleParameterName = Animator.StringToHash(idleParameterName);
        GroundParameterName = Animator.StringToHash(goundParameterName);

    }
}
