using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class EnemyAnimationData 
{
    [SerializeField] private string jumpParameterName = "Jump";

    public int JumParameterName { get; private set; }

    public void Initialize()
    {
        JumParameterName = Animator.StringToHash(jumpParameterName);
    }
}
