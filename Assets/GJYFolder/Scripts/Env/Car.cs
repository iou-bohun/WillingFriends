using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    private Rigidbody _rigid;
    private LoadPlatform _loadPlatform;
    private Vector3 _dir;
    private float _speed;

    private bool _isSetup = false;    

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    public void Setup(LoadPlatform loadPlatform, Vector3 dir, float speed)
    {
        _dir = dir;
        _loadPlatform = loadPlatform;
        _isSetup = true;
        _speed = speed;
    }

    private void FixedUpdate()
    {
        if (!_isSetup)
            return;

        _rigid.velocity = _dir * _speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EndPoint"))
            _loadPlatform.DisableCar();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Enemy enemy) == true)
            enemy.Die();
    }

    private void OnDisable()
    {
        _isSetup = false;
        _loadPlatform = null;

        _speed = 0;
        _rigid.velocity = Vector3.zero;
        transform.position = Vector3.down * 10;
        transform.rotation = Quaternion.identity;
    }
}
