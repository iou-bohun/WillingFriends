using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemManager : SingletoneBase<DropItemManager>
{
    public AttackSO[] attackSOArray;
    int arrNum = 0;
    public void InstantiateRandomWeaponPrefab(Transform transform)
    {
        arrNum = Random.Range(0, attackSOArray.Length);
        Debug.Log($"{attackSOArray[arrNum].weaponType.ToString()}");
        GameObject go = ObjectPoolManager.Instance.GetObject(attackSOArray[arrNum].weaponType.ToString());
        Rigidbody goRigidbody = go.GetComponent<Rigidbody>();
        if(goRigidbody.useGravity == true)
            goRigidbody.useGravity = false;
        go.transform.position = new Vector3(transform.position.x,0.5f,transform.position.z);
        Collider goCollider = go.GetComponent<Collider>();
        goCollider.isTrigger = true;
    }
}
