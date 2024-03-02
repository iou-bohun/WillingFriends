using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform _target; // 플레이어의 Transform
    [SerializeField] Transform _camera;

    [Header("Follow Option")]
    [SerializeField] float _moveSpeed;
    [SerializeField] float _offsetX;
    [SerializeField] float _limitX;

    [Header("Shake Option")]
    [SerializeField] float _shakeDuration;
    [SerializeField] float _positionShakePower;
    [SerializeField] float _rotationShakePower;

    private float _followDistance;
    private float _followSpeed;
    private float _currentDistance;    

    private readonly float CORRECTION_OFFSET = 1f;
    private readonly float FOLLOW_OFFSET = 1f;

    private void Start()
    {
        _camera = Camera.main.transform;
        _followDistance = _target.position.z - transform.position.z + FOLLOW_OFFSET;
    }

    private void FixedUpdate()
    {
        // 게임오버 시 정지
        if (GameManager.Instance.player.IsDead)
            return;

        _currentDistance = _target.position.z - transform.position.z;

        // 플레이어의 z 좌표가 시작 오프셋 거리보다 멀어지면 스무스 이동
        if (_currentDistance > _followDistance)
        {
            _followSpeed = _currentDistance - _followDistance;
            Vector3 destPos = transform.position + Vector3.forward;
            transform.position = Vector3.Lerp(transform.position, destPos, _followSpeed * Time.fixedDeltaTime);
        }
        // 아니면 계속 앞으로 이동
        if (_currentDistance < _followDistance + CORRECTION_OFFSET)
        {
            transform.position += Vector3.forward * _moveSpeed * Time.fixedDeltaTime;
        }

        // 플레이어의 x 좌표에 따라 스무스 이동
        if (transform.position.x != _target.position.x)
        {
            // To Do - X 좌표도 제한걸기
            float minX = -_limitX + _offsetX;
            float maxX = _limitX + _offsetX;

            Vector3 destPos = new Vector3(Mathf.Clamp(_target.position.x + _offsetX, minX, maxX) , transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, destPos, Time.fixedDeltaTime);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) // 실제 폭발 이벤트에 맞게 수정
            StartShake();
    }

    // 쉐이크 시작
    public void StartShake()
    {
        _camera.DOComplete();
        _camera.DOShakePosition(0.2f, _positionShakePower);
        _camera.DOShakeRotation(0.2f, _rotationShakePower);

        //StartCoroutine(Co_ShakeCamera(duration));
    }
}
