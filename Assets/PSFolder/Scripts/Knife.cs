using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    private Vector3 distanse;

    private void Update()
    {
        distanse = transform.position - GameManager.Instance.player.transform.position;
        if (distanse.sqrMagnitude > 50f)
        {
            Destroy(gameObject);
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

    public void DestroyKnife()
    {
        ObjectPoolManager.Instance.ReturnObject("Knife", gameObject);
    }
}
