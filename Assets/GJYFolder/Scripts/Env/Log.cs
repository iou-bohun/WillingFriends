using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : MonoBehaviour
{
    private Rigidbody _rigid;
    private RiverFlatform _riverPlatform;
    private Vector3 _dir;
    private float _speed;
    private float _speedModifier = 1f;    

    private bool _isSetup = false;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    public void Setup(RiverFlatform riverPlatform, Vector3 dir, float speed)
    {
        _dir = dir;
        _riverPlatform = riverPlatform;
        _isSetup = true;
        _speed = speed;
    }

    private void FixedUpdate()
    {
        if (!_isSetup)
            return;

        _rigid.velocity = _dir * _speed * _speedModifier;
    }

    private void OnDisable()
    {
        StopAllCoroutines();

        _isSetup = false;
        _riverPlatform = null;

        _speed = 0;
        _rigid.velocity = Vector3.zero;
        transform.position = Vector3.down * 10;
        transform.rotation = Quaternion.identity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Torrent"))
            StartCoroutine(Co_SpeedControl(_speedModifier, 2));

        if (other.CompareTag("EndPoint"))
            _riverPlatform.DisableLog();

        if (other.TryGetComponent(out Player player) == true)
            player.EnterLog(transform);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Torrent"))
            StartCoroutine(Co_SpeedControl(_speedModifier, 1));

        if (other.TryGetComponent(out Player player) == true)
            player.ExitLog();
    }

    private IEnumerator Co_SpeedControl(float start, float end)
    {
        float current = 0;
        float percent = 0;
        float time = 0.5f;
        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            _speedModifier = Mathf.Lerp(start, end, percent);

            yield return null;
        }        
    }
}
