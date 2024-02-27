using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private TDCharacterController _controller;

    private Vector2 _movementDirection = Vector2.zero;
    private Rigidbody _rigidbody;

    public float moveDistance = 1f;
    public float moveSpeed = 5f;
    // public float jumpForce = 10f;
    // public float groundCheckDistance = 0.1f;
    // public LayerMask groundLayer;

    private bool isMoving = false;

    private void Awake()
    {
        _controller = GetComponent<TDCharacterController>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _controller.OnMoveEvent += Move;
        _controller.OnAttackEvent += Attack;
    }

    void Update()
    {
        if (!isMoving)
        {
            if (_movementDirection != Vector2.zero)
            {
                isMoving = true;
                StartCoroutine(MoveCoroutine(_movementDirection));
            }
        }
    }
    /*

    private void FixedUpdate()
    {
        // Check if the character is grounded
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

        if (isGrounded)
        {
            // Apply jump force only when grounded
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    */

    private void Move(Vector2 direction)
    {
        _movementDirection = direction;
    }

    IEnumerator MoveCoroutine(Vector2 direction)
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + new Vector3(direction.x, 0f, direction.y) * moveDistance;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        // 이동 종료
        transform.position = endPosition;
        Invoke("MovingStateChange", 0.3f);
    }
    private void Attack(Vector3 position)
    {

    }

    public void MovingStateChange()
    {
        isMoving = false;
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