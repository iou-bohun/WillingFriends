using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    private static ObjectPoolManager _instance;
    [System.Serializable]
    public struct Pooling
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
    public static ObjectPoolManager Instance { get { return _instance; } }

    public List<Pooling> pools;//인스펙터 창에서 설정할 풀 정보
    private Pooling poolingRef = new Pooling();//현재 참조하는 풀링

    private Dictionary<string, Queue<GameObject>> poolDictionary; 

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        Debug.Log("ObjectPoolmanager awake");
        //Initialize(); 
    }
    private void Start()
    {
        Managers.ClearEvent += ClearDictionary;
    }
    private void ClearDictionary()
    {
        poolDictionary.Clear();//사전 초기화

        int num = transform.childCount;//자식의 수
        for(int i = 0; i < num; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
            Destroy(transform.GetChild(i).gameObject);
            Debug.Log("제거 완료");
        }

        Initialize();//사전 재 생성
    }

    public void Initialize() //사전 초기화
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (var pool in pools)//풀 리스트에서 꺼내옴
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.name = pool.tag;
                obj.SetActive(false);
                obj.transform.SetParent(Instance.transform);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
        Debug.Log("ObjectPoolManager 생성");
    }
    private GameObject CreateNewObject(string tag)// 태그에 해당하는 새로운 오브젝트 생성
    {
        if (!poolDictionary.ContainsKey(tag))//사전에 태그가 존재하는지 비교
            return null;

        foreach (var pool in pools)//풀 리스트에서
        {
            if (pool.tag == tag)//태그가 일치하는 것이 있으면 
            {
                poolingRef = pool; //일치하는 풀링을 참조
                break;
            }
        }
        GameObject obj = Instantiate(poolingRef.prefab, transform);//객체를 생성
        obj.name = tag;
        obj.gameObject.SetActive(false);

        return obj;
    }
    public static GameObject GetObject(string tag, Transform parent = null) //태그에 해당하는 객체를 반환한다.
    {
        if (!Instance.poolDictionary.ContainsKey(tag))//태그가 존재하는지 비교
            return null;

        if (Instance.poolDictionary[tag].Count > 0)//태그에 해당하는 큐에 남아있는 객체가 있다면
        {
            GameObject obj = Instance.poolDictionary[tag].Dequeue();//꺼낸다
            obj.transform.SetParent(parent); //부모를 지정한다.
            obj.gameObject.SetActive(true);//활성화            
            return obj;
        }
        else//큐에 남아있는 것이 없다면
        {
            GameObject newObj = Instance.CreateNewObject(tag);//새로 생성한다.
            newObj.transform.SetParent(parent); //부모 지정.
            newObj.gameObject.SetActive(true);//활성화
            return newObj;
        }
    }

    public static void ReturnObject(string tag ,GameObject gameObject)//태그에 해당하는 게임 오브젝트를 사전에 넣는다.
    {
        gameObject.gameObject.SetActive(false); //비활성화
        gameObject.transform.SetParent(Instance.transform); //오브젝트 매니저 하위로 넣는다.
        Instance.poolDictionary[tag].Enqueue(gameObject); //큐에 집어 넣는다.
    }

    // 잠시 꺼내서 쓰는 용도
    public static GameObject PeekObject(string tag)
    {
        if (!Instance.poolDictionary.ContainsKey(tag))//태그 비교
            return null;

        if (Instance.poolDictionary[tag].Count > 0)//남아 있는 것이 있다면
            return Instance.poolDictionary[tag].Peek();

        //생성하고 큐에 넣는다.
        Instance.poolDictionary[tag].Enqueue(Instance.CreateNewObject(tag));
        return Instance.poolDictionary[tag].Peek();
    }
}
