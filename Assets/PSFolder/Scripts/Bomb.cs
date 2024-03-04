using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject explosionEffect;
    public AttackSO _attackSO;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            GameObject exp = Instantiate(explosionEffect);
            exp.transform.position = transform.position;
            Destroy(exp, 2f);

            Collider[] cols = Physics.OverlapSphere(exp.transform.position, exp.transform.localScale.z / 2);
            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].gameObject.tag == "Enemy" || cols[i].gameObject.tag == "Tree_01coll" || cols[i].gameObject.tag == "Tree_02coll")
                    ObjectPoolManager.Instance.ReturnObject(cols[i].gameObject.tag, cols[i].gameObject);
            }

            ObjectPoolManager.Instance.ReturnObject("Bomb", gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            AttackManager playerAttackManager = other.GetComponent<AttackManager>();
            playerAttackManager.AttackSOChange(_attackSO);
            Collider goCollider = gameObject.GetComponent<Collider>();
            Rigidbody goRigidbody = gameObject.GetComponent<Rigidbody>();
            goRigidbody.useGravity = true;
            goCollider.isTrigger = false;
            ObjectPoolManager.Instance.ReturnObject("Bomb", gameObject);
        }
    }
}