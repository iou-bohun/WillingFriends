using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    private Vector3 distanse;
    public AttackSO _attackSO;

    private void OnEnable()
    {
        SoundManager.Instance.PlayAudioClip("player_attack", transform);
    }

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
        if (collision.gameObject.tag == "Tree")
        {
            Invoke("DestroyKnife", 1f);
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            ObjectPoolManager.Instance.ReturnObject("Enemy", collision.gameObject);
            GameObject enemyDie = ObjectPoolManager.Instance.GetObject("Goblin_01Broken");
            enemyDie.transform.position = collision.gameObject.transform.position;
            ObjectPoolManager.Instance.ReturnObject("Knife", gameObject);
        }
        else if (collision.gameObject.tag == "Weapon")
        {
            ObjectPoolManager.Instance.ReturnObject("Knife", gameObject);
        }
        else
        {
            Invoke("DestroyKnife", 2f);
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
