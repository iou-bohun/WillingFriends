using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;
using UnityEngine.TextCore.Text;
using TMPro;
using System.Security.Cryptography;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject _broken;

    [field:Header("Animations")]
    [field:SerializeField] public EnemyAnimationData AnimationData { get;private set; }

    [field:Header("Movement")]
    [SerializeField] float jumpForce;
    [SerializeField] public bool isGrounded = false;
    [SerializeField] private Vector3 currentPosition;
    [SerializeField] private Vector3 movedPosition;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float detectRange;

    private Animator _animator;
    private Rigidbody _rigid;

    private LandPlatform _land;
    private int _myIndex = -1;

    [SerializeField] private Transform player;

    private void Awake()
    {
        AnimationData.Initialize();

        _animator = GetComponentInChildren<Animator>();
        _rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        player = GameManager.Instance.player.transform;
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
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Vector3 groundNormal = collision.GetContact(0).normal; //충돌지점의 법선백터
            if(groundNormal == Vector3.up) //위에서 충돌
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
                //플레이어 킬
                GameManager.Instance.GameOver();
                Debug.Log("PlayerKill");
            }
            else if(playerNormal == Vector3.forward)
            {
                //적 뒤로 밀림
                StartCoroutine(PushEnemy());
                Debug.Log("EnemyPush");
            }
        }
        if (collision.gameObject.CompareTag("Tree"))
        {
            //적 죽음
            Die();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Border"))
            Die();
    }

    IEnumerator PushEnemy()
    {
        float elaspedTime = 0f;
        float duration = 0.3f;
        Vector3 targetPosition = transform.position + Vector3.forward + (Vector3.up*0.1f);

        while(elaspedTime < duration)
        {
            elaspedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, targetPosition, elaspedTime / duration);
            yield return null;
        }
        Debug.Log(targetPosition.z);
        transform.position = new Vector3(transform.position.x, transform.position.y, targetPosition.z);
    }

    public void MoveE()
    {
        StartCoroutine (MovePosition());
    }

    IEnumerator MovePosition()
    {
        float elaspedTime = 0f;
        float duration = 0.5f;
        Vector3 moveDir = GetDirection();
        Vector3 movedirection = transform.position + moveDir;
        transform.rotation = Quaternion.LookRotation(-moveDir);
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
        Vector3 direction = FallingSearch(directions[randomIndex]);

        return direction;
    }

    #region 주변 물체 탐지
    /// <summary>
    /// 전후좌우 장애물 체크
    /// </summary>
    /// <returns>장애물이 없는 방향들 </returns>
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
            if (!Physics.Raycast(ray, 1f, obstacleMask)) // 그 방향에 장애물이 업다면 
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

    private Vector3 FallingSearch(Vector3 direction)
    {
        Ray ray = new Ray(transform.position + direction, Vector3.down);        
        if (!Physics.Raycast(ray, 3f, groundMask))
            return Vector3.zero;

        return direction;
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
        transform.rotation = Quaternion.LookRotation(Vector3.forward);
    }

    public void SmallJump()
    {
        _rigid.AddForce(Vector3.up * (jumpForce / 2), ForceMode.Impulse);
    }

    public void Setup(LandPlatform land, int index)
    {
        _land = land;
        _myIndex = index;
    }

    public void Die()
    {
        GameObject broken = ObjectPoolManager.Instance.GetObject(_broken.name);
        broken.transform.position = transform.position;
        DropItemManager.Instance.InstantiateRandomWeaponPrefab(transform);//드롭 아이템

        _land.SelfRemove(_myIndex);
        _land = null;
        _myIndex = -1;
        ObjectPoolManager.Instance.ReturnObject(gameObject.name, gameObject);
    }
}
