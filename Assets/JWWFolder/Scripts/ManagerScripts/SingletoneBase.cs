using System;
using Unity.VisualScripting;
using UnityEngine;

public class SingletoneBase<T> : MonoBehaviour where T : MonoBehaviour
{
    // 프로퍼티
    protected static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)//객체가 없을 때만 생성
            {
                string typeName = typeof(T).FullName;

                T type = FindObjectOfType<T>();
                if (type == null)
                    type = new GameObject(typeName).AddComponent<T>();

                _instance = type;                
            }            

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (Instance != this)
            Destroy(gameObject);
    }

    public virtual void Clear()
    {
        
    }
    public virtual void Init()
    {
        
    }
}