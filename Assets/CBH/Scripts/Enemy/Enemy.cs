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

    }

    private void Update()
    {
        ///Test
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(PushEnemy());    
            Debug.Log(PushedPosition().z);
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
            Vector3 groundNormal = collision.GetContact(0).normal; //�浹������ ��������
            if(groundNormal == Vector3.up) //������ �浹
            {
                isGrounded = true;
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
                //�÷��̾� �ڷ� �и�
                Debug.Log("PlayerPush");
                _rigid.AddForce(Vector3.forward * pushForce, ForceMode.Impulse);
               
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
        return new Vector3(0, 0, 1);
    }

    IEnumerator PushEnemy()
    {
        float elaspedTime = 0f;
        float duration = 0.5f;

        while(elaspedTime < duration)
        {
            elaspedTime += Time.deltaTime;
            transform.position = Vector3.Slerp(transform.position, PushedPosition(), elaspedTime / duration) ;
            yield return null;
        }

    }

}
