using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject explosionEffect;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            GameObject exp = Instantiate(explosionEffect);
            exp.transform.position = transform.position;
            Destroy(exp, 2f);

            Collider[] cols = Physics.OverlapSphere(exp.transform.position, exp.transform.localScale.z / 2);
            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].gameObject.tag == "Enemy" || cols[i].gameObject.tag == "Tree_01coll" || cols[i].gameObject.tag == "Tree_02coll")
                { 
                    ObjectPoolManager.Instance.ReturnObject(cols[i].gameObject.tag, cols[i].gameObject);
                    if (cols[i].gameObject.tag == "Enemy")
                    {
                        GameObject enemyDie = ObjectPoolManager.Instance.GetObject("Goblin_01Broken");
                        enemyDie.transform.position = other.transform.position;
                    }
                }
            }
            ObjectPoolManager.Instance.ReturnObject("Bomb", gameObject);
        }
    }
}