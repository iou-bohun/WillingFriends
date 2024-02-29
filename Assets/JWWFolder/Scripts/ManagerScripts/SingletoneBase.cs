using System;
using UnityEngine;

public class SingletoneBase<T> : MonoBehaviour where T : MonoBehaviour
{
    // 프로퍼티
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)//객체가 없을 때만 생성
            {
                string typeName = typeof(T).FullName;
                GameObject go = new GameObject(typeName);
                _instance = go.AddComponent<T>();

                DontDestroyOnLoad(go);
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        Init();
    }

    public virtual void Init()
    {
        Debug.Log(transform.name + " is Init");
    }
}