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

                GameObject go = GameObject.Find(typeName);
                if(go == null)
                    go = new GameObject(typeName);
                _instance = go.GetOrAddComponent<T>();

                //DontDestroyOnLoad(go);
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        Init();
    }
    public virtual void Clear()
    {
        
    }
    public virtual void Init()
    {
        Debug.Log(transform.name + " is Init");
    }
}