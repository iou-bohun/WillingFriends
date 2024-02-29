using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackData
{
    public AttackSO attackSO;
    public int count;
}

public class AttackManager : MonoBehaviour
{
    private TDCharacterController _controller;
    public AttackData attackData;
    public Transform weaponRoot;

    private void Awake()
    {
        _controller = GetComponent<TDCharacterController>();
    }

    private void Start()
    {
        _controller.OnAttackEvent += Attack;
    }
    public void AttackSOChange(AttackSO attackSO)
    {
        if (attackData.attackSO != attackSO)
        {
            attackData.attackSO = attackSO;
            attackData.count = attackData.attackSO.maxCount;
        }
        else
        {
            attackData.count = attackData.attackSO.maxCount;
        }
    }

    public void Attack(Vector3 position)
    {
        if (attackData.attackSO != null)
        {
            if (attackData.count == 0)
            {
                attackData.attackSO = null;
            }
            attackData.count--;

            if (attackData.attackSO.weaponType == WeaponType.Knife) KnifeAttack();
            else if (attackData.attackSO.weaponType == WeaponType.Bomb) BombAttack();
            else if (attackData.attackSO.weaponType == WeaponType.Boomerang) BoomerangAttack();
        }
    }

    public void KnifeAttack()
    {
        GameObject weapon = ObjectPoolManager.Instance.GetObject("Knife", weaponRoot);
        weapon.transform.position = transform.position + new Vector3(0, 0, 0.5f);
        Rigidbody rb = weapon.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 5f, ForceMode.Impulse);
    }
    public void BombAttack()
    {
        GameObject weapon = ObjectPoolManager.Instance.GetObject("Bomb", weaponRoot);
        weapon.transform.position = transform.position + new Vector3(0, 0.5f, 0.5f);
        Rigidbody rb = weapon.GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(0f, 1f, 1f) * 5f, ForceMode.Impulse);
    }
    public void BoomerangAttack()
    {

    }
}
