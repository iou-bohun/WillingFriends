using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [field:Header("Animations")]
    [field:SerializeField] public EnemyAnimationData AnimationData { get;private set; }

    [field:Header("Movement")]
    [SerializeField] float jumpForce;
    [SerializeField] bool isGrounded = false;

    private Animator _animator;
    private Rigidbody _rigid;

    private void Awake()
    {
        AnimationData.Initialize();

        _animator = GetComponentInChildren<Animator>();
        _rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
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
            Debug.Log(collision.GetContact(0).point);
            Debug.Log(collision.GetContact(1).point);
            Debug.Log(collision.GetContact(2).point);
            Debug.Log(collision.GetContact(3).point);
            Vector3 normal = collision.GetContact(0).normal; //충돌지점의 법선백터
            if(normal == Vector3.up) //위에서 충돌
            {
                isGrounded = true;
            }
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 down = collision.GetContact(0).normal;
            if(down == Vector3.up)
            {
                Debug.Log("PlayerKill");
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
}
