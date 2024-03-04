using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    private Vector3 distanse;
    public AttackSO _attackSO;

    private void Update()
    {
        distanse = transform.position - GameManager.Instance.player.transform.position;
        if (distanse.sqrMagnitude > 50f)
        {
            //Destroy(gameObject);
            DestroyKnife();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Tree")))
        {
            Invoke("DestroyKnife", 1f);
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            ObjectPoolManager.Instance.ReturnObject("Knife", gameObject);
        }
        else if (collision.gameObject.tag == "Weapon")
        {
            ObjectPoolManager.Instance.ReturnObject("Knife", gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            AttackManager playerAttackManager = other.GetComponent<AttackManager>();
            playerAttackManager.AttackSOChange(_attackSO);
            Collider goCollider = gameObject.GetComponent<Collider>();
            goCollider.isTrigger = false;
            ObjectPoolManager.Instance.ReturnObject("Bomb", gameObject);
        }
    }
        public void DestroyKnife()
    {
        ObjectPoolManager.Instance.ReturnObject("Knife", gameObject);
    }
}
