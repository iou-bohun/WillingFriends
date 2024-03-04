using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Knife,
    Bomb,
    Boomerang
}

[CreateAssetMenu(menuName = "Data", fileName = "Attack")]
public class AttackSO : ScriptableObject
{
    public WeaponType weaponType;
    public ItemType itemType;
    public GameObject weaponPrefab;
    public int maxCount;
}