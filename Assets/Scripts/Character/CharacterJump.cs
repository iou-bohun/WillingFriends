using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJump : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Animator _anim;
    private float jumpForce = 3f;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Ground")
        {
            _anim.SetBool("Jump", true);
        }
    }

    public void JumpUp()
    {
        _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        _anim.SetBool("Jump", false);
    }
}
