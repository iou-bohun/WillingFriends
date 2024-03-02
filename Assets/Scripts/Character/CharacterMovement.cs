using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CharacterMovement : MonoBehaviour
{
    private TDCharacterController _controller;
    private Player _player;

    private Vector2 _movementDirection = Vector2.zero;

    public float moveDistance = 1f;
    public float moveSpeed = 5f;
    public float raycastDistance = 0.5f;

    private bool _isMoving = false;

    private void Awake()
    {
        _controller = GetComponent<TDCharacterController>();
        _player = GetComponent<Player>();
    }

    private void Start()
    {
        _controller.OnMoveEvent += Move;
    }

    void Update()
    {
        if (!_isMoving && _movementDirection != Vector2.zero)
        {
            if (!IsObstacleInPath())
            {
                _isMoving = true;
                StartCoroutine(MoveCoroutine(_movementDirection));
            }
        }
    }

    private void Move(Vector2 direction)
    {
        _movementDirection = direction;        
    }

    IEnumerator MoveCoroutine(Vector2 direction)
    {
        if (_player.IsDead)
            yield break;

        GameManager.Instance.OnPlayerMove((int)direction.y);

        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + new Vector3(direction.x, 0.01f, direction.y) * moveDistance;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        transform.position = endPosition;
        Invoke("MovingStateChange", 0.1f);
    }

    public void MovingStateChange()
    {
        _isMoving = false;
    }

    public void NowMoving()
    {
        _isMoving = true;
    }

    private bool IsObstacleInPath()
    {
        RaycastHit hit;
        Vector3 dir = new Vector3(_movementDirection.x, 0f, _movementDirection.y);
        if (Physics.Raycast(transform.position, dir, out hit, raycastDistance, 1 << LayerMask.NameToLayer("Tree")))
        {
            _isMoving = false;
            return true;
        }
        return false;
    }
}


/*
 * 

    private void FixedUpdate()
    {
        if(_controller.moveable == false)
        {
            ApplyMovment(Vector2.zero);
        }
        else 
        {
            ApplyMovment(_movementDirection);
        }
    }

    private void Move(Vector2 direction)
    {
        _movementDirection = direction;
    }

    private void ApplyMovment(Vector2 direction)
    {
        direction = direction * 5;

        _rigidbody.velocity = direction;
    }

    private void Attack(Vector3 position)
    {

    }


*/