using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject explosionEffect;
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
                if (cols[i].gameObject.tag == "Enemy" || cols[i].gameObject.tag == "Tree")
                    ObjectPoolManager.ReturnObject(cols[i].gameObject.tag, cols[i].gameObject);
            }

            ObjectPoolManager.ReturnObject("Bomb", gameObject);
        }
    }
}