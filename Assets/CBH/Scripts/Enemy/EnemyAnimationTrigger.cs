using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationTrigger : MonoBehaviour
{
    private Enemy enemy;
    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();    
    }
    public void BigJump()
    {
        enemy.BigJump();
    }

    public void SmallJump()
    {
        enemy.SmallJump();
    }

    public void Move()
    {
        enemy.MoveE();
    }
}
