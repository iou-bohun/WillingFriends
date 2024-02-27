using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [Range(1, 2)][SerializeField] float _minSpeed;
    [Range(2, 5)][SerializeField] float _maxSpeed;

    private Rigidbody _rigid;
    private LoadPlatform _loadPlatform;
    private Vector3 _dir;
    private float _speed;

    private bool _isSetup = false;    

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    public void Setup(Vector3 dir, LoadPlatform loadPlatform)
    {
        _dir = dir;
        _loadPlatform = loadPlatform;
        _isSetup = true;
        _speed = Random.Range(_minSpeed, _maxSpeed);
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

    private void OnDisable()
    {
        _isSetup = false;
        _loadPlatform = null;
        transform.position = Vector3.down * 10;
        transform.rotation = Quaternion.identity;
    }
}
