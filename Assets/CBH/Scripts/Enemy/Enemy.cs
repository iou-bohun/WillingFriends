using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;
using UnityEngine.TextCore.Text;

public class Enemy : MonoBehaviour
{
    [field:Header("Animations")]
    [field:SerializeField] public EnemyAnimationData AnimationData { get;private set; }

    [field:Header("Movement")]
    [SerializeField] float jumpForce;
    [SerializeField] bool isGrounded = false;
    [SerializeField] float pushForce;
    [SerializeField] private Vector3 currentPosition;
    [SerializeField] private Vector3 movedPosition;

    private Animator _animator;
    private Rigidbody _rigid;

    private void Awake()
    {
        AnimationData.Initialize();

        _animator = GetComponentInChildren<Animator>();
        _rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        movedPosition = transform.position; 
    }

    private void Update()
    {
        ///Test
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space");
            StartCoroutine(PushEnemy());
        }
        ///


        if(isGrounded)
        {
            JumpAnimationStart();
        }
    }


    private void JumpAnimationStart()
    {
        _animator.SetBool(AnimationData.JumParameterName,true);
    }

    public void BigJump()
    {
        _rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Vector3 groundNormal = collision.GetContact(0).normal; //충돌지점의 법선백터
            if(groundNormal == Vector3.up) //위에서 충돌
            {
                isGrounded = true;
            }
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 playerNormal = collision.GetContact(0).normal;
            if(playerNormal == Vector3.up)
            {
                //플레이어 킬
                Debug.Log("PlayerKill");
            }
            else if(playerNormal == Vector3.forward)
            {
                //플레이어 뒤로 밀림
                StartCoroutine(PushEnemy());
                Debug.Log("PlayerPush");
               
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private Vector3 PushedPosition()
    {
        movedPosition += Vector3.forward;
        return movedPosition;
    }

    IEnumerator PushEnemy()
    {
        float elaspedTime = 0f;
        float duration = 0.5f;
        Vector3 targetPosition = PushedPosition();

        while(elaspedTime < duration)
        {
            elaspedTime += Time.deltaTime;
            transform.position = Vector3.Slerp(transform.position, new Vector3(transform.position.x, transform.position.y, targetPosition.z), elaspedTime / duration) ;
            yield return null;
        }

    }

}
