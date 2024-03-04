using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    //public AttackManager attackManager;
    private Rigidbody rb;
    public float time;
    private bool returned;

    private void Start()
    {
        time = 0f;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (!returned)
        {
            if (time > 2f)
            {
                time = 0;
                returned = true;
                rb.velocity = Vector3.zero;
                rb.AddForce(-transform.forward * 5f, ForceMode.Impulse);
            }
        }
        else
        {
            if (time > 3f)
            {
                ObjectPoolManager.Instance.ReturnObject("Boomerang", gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log(other.tag);
            ObjectPoolManager.Instance.ReturnObject("Boomerang", gameObject);
            //attackManager.attackData.count++;
        }
        else if (other.tag == "Enemy")
        {
            ObjectPoolManager.Instance.ReturnObject(other.tag, other.gameObject);
        }
    }
}
