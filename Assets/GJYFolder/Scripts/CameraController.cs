using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform _target; // 플레이어의 Transform
    [SerializeField] float _moveSpeed;
    [SerializeField] float _offsetX;
    [SerializeField] float _shakeMagnitude;

    private float _followDistance;
    private float _followSpeed;
    [SerializeField]private float _currentDistance;

    private readonly float CORRECTION_OFFSET = 1f;

    private void Start()
    {
        _followDistance = _target.position.z - transform.position.z;        
    }

    private void FixedUpdate()
    {        
        // 게임오버 시 정지        
        
        
        _currentDistance = _target.position.z - transform.position.z;

        // 플레이어의 z 좌표가 시작 오프셋 거리보다 멀어지면 스무스 이동
        if (_currentDistance > _followDistance)
        {
            _followSpeed = _currentDistance - _followDistance;
            Vector3 destPos = transform.position + Vector3.forward;
            transform.position = Vector3.Lerp(transform.position, destPos, _followSpeed * Time.fixedDeltaTime);
        }
        // 아니면 계속 앞으로 이동
        if(_currentDistance < _followDistance + CORRECTION_OFFSET)
        {
            transform.position += Vector3.forward * _moveSpeed * Time.fixedDeltaTime;
        }

        // 플레이어의 x 좌표에 따라 스무스 이동
        if (transform.position.x != _target.position.x)
        {
            Vector3 destPos = new Vector3(_target.position.x + _offsetX, transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, destPos, Time.fixedDeltaTime);
        }               
    }

    private void Update()
    {        
        if (Input.GetKeyDown(KeyCode.Space)) // 실제 폭발 이벤트에 맞게 수정
        {
            StartShake(0.5f);
        }
    }

    // 쉐이크 시작
    public void StartShake(float duration)
    {
        StartCoroutine(Co_ShakeCamera(duration));
    }

    // 너무 투박하니 나중에 두트윈을 넣자!
    private IEnumerator Co_ShakeCamera(float duration)
    {        
        Vector3 originPos = new Vector3(transform.position.x, transform.position.y, 0);

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * _shakeMagnitude;
            float y = Random.Range(-1f, 1f) * _shakeMagnitude;

            transform.localPosition += new Vector3(x, y, 0);         

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = originPos + Vector3.forward * transform.position.z;
    }
}
