using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    protected Queue<GameObject> _obstacleQueue = new Queue<GameObject>();

    protected bool isInit = false;

    public virtual void Init() { }

    public virtual void Clear()
    {
        while(_obstacleQueue.Count > 0)
            ObjectPoolManager.Instance.ReturnObject(_obstacleQueue.Peek().name, _obstacleQueue.Dequeue());
    }
}
