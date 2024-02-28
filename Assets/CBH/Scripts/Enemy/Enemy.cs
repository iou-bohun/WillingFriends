using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;
using UnityEngine.TextCore.Text;
using TMPro;
using System.Security.Cryptography;
using UnityEngine.EventSystems;

public class Enemy : MonoBehaviour
{
    [field:Header("Animations")]
    [field:SerializeField] public EnemyAnimationData AnimationData { get;private set; }

    [field:Header("Movement")]
    [SerializeField] float jumpForce;
    [SerializeField] public bool isGrounded = false;
    [SerializeField] private Vector3 currentPosition;
    [SerializeField] private Vector3 movedPosition;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float detectRange;

    private Animator _animator;
    private Rigidbody _rigid;

    [SerializeField] private Transform player;

    private void Awake()
    {
        AnimationData.Initialize();

        _animator = GetComponentInChildren<Animator>();
        _rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
       //StartCoroutine(Move());
       movedPosition = transform.position; 
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, player.position) < detectRange)
        {
            _animator.SetBool(AnimationData.JumParameterName, true);
        }
        else
        {
            _animator.SetBool(AnimationData.JumParameterName,false);
        }

        ///Test
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(PushEnemy());
        }
    }


    private void JumpAnimationStart()
    {
        _animator.SetTrigger(AnimationData.JumParameterName);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Vector3 groundNormal = collision.GetContact(0).normal; //�浹������ ��������
            if(groundNormal == Vector3.up) //������ �浹
            {
                isGrounded = true;
                _animator.SetBool(AnimationData.GroundParameterName, isGrounded);
            }
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 playerNormal = collision.GetContact(0).normal;
            if(playerNormal == Vector3.up)
            {
                //�÷��̾� ų
                Debug.Log("PlayerKill");
            }
            else if(playerNormal == Vector3.forward)
            {
                //�� �ڷ� �и�
                StartCoroutine(PushEnemy());
                Debug.Log("EnemyPush");
               
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            _animator.SetBool(AnimationData.GroundParameterName, isGrounded);
        }
    }

    IEnumerator PushEnemy()
    {
        float elaspedTime = 0f;
        float duration = 0.3f;
        Vector3 targetPosition = transform.position + Vector3.forward;

        while(elaspedTime < duration)
        {
            elaspedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, targetPosition, elaspedTime / duration);
            yield return null;
        }
    }

    IEnumerator Move()
    {
        while (true)
        {
            if(Vector3.Distance(transform.position, player.position) > detectRange && isGrounded)
            {
                StartCoroutine(MovePosition());
            }
            yield return new WaitForSeconds(1);
        }
    }

    public void MoveE()
    {
        StartCoroutine (MovePosition());
    }

    IEnumerator MovePosition()
    {
        float elaspedTime = 0f;
        float duration = 0.5f;
        Vector3 movedirection = transform.position + GetDirection();

        while (elaspedTime < duration)
        {
            elaspedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, movedirection, elaspedTime / duration);
           yield return null;
        }
    }

    private Vector3 GetDirection()
    {
        Vector3[] directions = ObstacleSearch();
        int dirLength = directions.Length;
        int randomIndex = Random.Range(0, dirLength);
        Vector3 direction = directions[randomIndex];

        return direction;
    }

    #region �ֺ� ��ü Ž��
    /// <summary>
    /// �����¿� ��ֹ� üũ
    /// </summary>
    /// <returns>��ֹ��� ���� ����� </returns>
    private Vector3[] ObstacleSearch()
    {
        List<Vector3> safeDirections = new List<Vector3>();
        safeDirections.Add(Vector3.zero);

        Ray[] rays = new Ray[4]
        {
        new Ray(transform.position, Vector3.forward),
        new Ray(transform.position, Vector3.back),
        new Ray(transform.position,  Vector3.right),
        new Ray(transform.position, Vector3.left)
        };
        foreach (Ray ray in rays)
        {
            if (!Physics.Raycast(ray, 1f, obstacleMask)) // �� ���⿡ ��ֹ��� ���ٸ� 
            {
                if (ray.direction == Vector3.forward)
                    safeDirections.Add(Vector3.forward);
                else if (ray.direction == Vector3.back)
                    safeDirections.Add(Vector3.back);
                else if (ray.direction == Vector3.right)
                    safeDirections.Add(Vector3.right);
                else if (ray.direction == Vector3.left)
                    safeDirections.Add(Vector3.left);
            }
        }
        return safeDirections.ToArray();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, Vector3.forward);
        Gizmos.DrawRay(transform.position, Vector3.back);
        Gizmos.DrawRay(transform.position, Vector3.right);
        Gizmos.DrawRay(transform.position, Vector3.left);
    }
    #endregion

    public void BigJump()
    {
        _rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public void SmallJump()
    {
        _rigid.AddForce(Vector3.up * (jumpForce / 2), ForceMode.Impulse);
    }

}
