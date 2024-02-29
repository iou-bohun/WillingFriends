using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    private Vector3 distanse;

    private void Update()
    {
        distanse = transform.position - GameManager.Instance.player.position;
        if (distanse.sqrMagnitude > 50f)
        {
            Destroy(gameObject);
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
            ObjectPoolManager.ReturnObject("Knife", gameObject);
        }
        else if (collision.gameObject.tag == "Weapon")
        {
            ObjectPoolManager.ReturnObject("Knife", gameObject);
        }
    }

    public void DestroyKnife()
    {
        ObjectPoolManager.ReturnObject("Knife", gameObject);
    }
}
