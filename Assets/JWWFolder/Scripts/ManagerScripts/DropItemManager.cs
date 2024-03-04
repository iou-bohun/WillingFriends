using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    KnifeItem,
    BoomItem,
    BoomerangItem
}
public class DropItemManager : SingletoneBase<DropItemManager>
{
    public AttackSO[] attackSOArray;
    int arrNum = 0;
    private Vector3 position = new Vector3(0f, 0f, 0f);
    public void InstantiateRandomWeaponPrefab(Vector3 targetPostion)
    {
        position.x = targetPostion.x;
        position.y = 0.5f;
        position.z = targetPostion.z;
        arrNum = Random.Range(0, attackSOArray.Length);
        Debug.Log($"{attackSOArray[arrNum].itemType.ToString()}");
        GameObject go = ObjectPoolManager.Instance.GetObject(attackSOArray[arrNum].itemType.ToString());
        go.transform.position = position;
        //go.transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
    }
}
