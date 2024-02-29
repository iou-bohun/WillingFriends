using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempMonster : MonoBehaviour
{
    private Vector3 direction;

    private void Awake()
    {
    }
    public void MoveForward(Vector3 dir)
    {
        direction = dir;
        Invoke("DestoryBullet",5f);
    }
    private void DestoryBullet()
    {
        ObjectPoolManager.Instance.ReturnObject("TempMonster",this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * Time.deltaTime);
    }
}
