using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] LayerMask _logLayer;
    [SerializeField] LayerMask _borderLayer;
    [SerializeField] Transform _parent;

    private TDCharacterController _controller;
    private Player _player;
    private Rigidbody _rigid;

    private Vector2 _movementDirection = Vector2.zero;

    public float moveDistance = 1f;
    public float moveSpeed = 5f;
    public float raycastDistance = 0.5f;

    private bool _isMoving = false;

    private int curScore = 0;
    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
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

        GameManager.Instance.CallPlayerMove((int)direction.y);

        Vector3 startPosition = transform.localPosition;
        Vector3 movePosition = new Vector3(direction.x, 0.01f, direction.y);
        Vector3 endPosition = CalcEndPosition(ref startPosition, movePosition);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            transform.localPosition = Vector3.Lerp(startPosition, endPosition, t);
            if (Vector3.Distance(transform.localPosition, endPosition) < 0.05f)
                break;
            yield return null;
        }

        transform.localPosition = endPosition;
        AddScore(endPosition.z);
        Invoke("MovingStateChange", 0.1f);
    }

    private void AddScore(float distance)
    {
        if(distance > curScore)
        {
            curScore++;
        }
        Debug.Log(curScore);
        GameManager.Instance.Score = curScore;
        UIManager.Instance.uiUpdateEvent();
    }

    private Vector3 CalcEndPosition(ref Vector3 startPosition, Vector3 movePosition)
    {
        Vector3 endPosition = startPosition + movePosition * moveDistance;

        // 로컬 포지션 X 를 정수로 강제로 맞추기
        if (endPosition.x % 1 != 0)
        {
            int endPosX = Mathf.RoundToInt(endPosition.x);
            endPosition = new Vector3(endPosX, endPosition.y, endPosition.z);
        }

        return endPosition;
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
        if (Physics.Raycast(transform.position, dir, out hit, raycastDistance, _borderLayer))
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