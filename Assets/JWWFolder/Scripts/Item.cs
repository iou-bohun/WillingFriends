using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public AttackSO _attackSO;
    // Start is called before the first frame update
    private void Start()
    {
        Invoke("Disappear", 10f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            AttackManager playerAttackManager = other.GetComponent<AttackManager>();
            playerAttackManager.AttackSOChange(_attackSO);
            Disappear();
        }
    }
    private void Disappear()
    {
        ObjectPoolManager.Instance.ReturnObject(_attackSO.itemType.ToString(), gameObject);
    }
}
