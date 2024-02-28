using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    private TDCharacterController _controller;

    public GameObject weaponFac;

    private void Awake()
    {
        _controller = GetComponent<TDCharacterController>();
    }

    private void Start()
    {
        _controller.OnAttackEvent += Attack;
    }

    private void Attack(Vector3 position)
    {
        GameObject weapon = Instantiate(weaponFac);
        weapon.transform.position = transform.position + new Vector3(0, 0, 0.5f);
        Rigidbody rb = weapon.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 5f, ForceMode.Impulse);
    }
}
