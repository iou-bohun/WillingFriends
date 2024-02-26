using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public struct Pooling
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
    public List<Pooling> pools;//풀링 리스트
    private Pooling poolingRef = new Pooling();//풀링 참조

    public static ObjectPool Instance;

    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

    private void Awake()
    {
        Instance = this;

        Initialize();
    }
    private void Initialize() // 정해진 숫자 만큼 오브젝트 생성 초기화
    {
        foreach (var pool in pools)//리스트에서 꺼내서
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }
    private GameObject CreateNewObject(string tag)// 오브젝트 생성 후 반환 //Get에서만 사용
    {
        if (!poolDictionary.ContainsKey(tag))//있는 종류만 생성하겠다!
            return null;

        foreach (var pool in pools) 
        {
            if (pool.tag == tag)//태그가 일치하면
            {
                poolingRef = pool;
                break;
            }
        }
        GameObject obj = Instantiate(poolingRef.prefab, transform);//오브젝트 생성
        obj.gameObject.SetActive(false);

        return obj;
    }
    public static GameObject GetObject(string tag)//오브젝트를 풀에서 꺼내서 반환
    {
        if (!Instance.poolDictionary.ContainsKey(tag))//딕셔너리에 없으면 널
            return null;

        if (Instance.poolDictionary[tag].Count > 0)//큐에 남아있는 게 있을 때 
        {
            GameObject obj = Instance.poolDictionary[tag].Dequeue();//큐에서 꺼낸다.
            obj.transform.SetParent(null);//독립적인 객체로 분리
            obj.gameObject.SetActive(true);//활성화
            return obj;
        }
        else//큐에 남아있는 게 없을 때
        {
            GameObject newObj = Instance.CreateNewObject(tag);//만들고
            newObj.transform.SetParent(null);//독립
            newObj.gameObject.SetActive(true);//활성화
            return newObj;
        }
    }
    public static void ReturnObject(string tag ,GameObject gameObject)//오브젝트를 tag에 해당하는 풀에 집어넣음
    {
        gameObject.gameObject.SetActive(false); //비활성화
        gameObject.transform.SetParent(Instance.transform); // 풀 하위에 넣음
        Instance.poolDictionary[tag].Enqueue(gameObject); //돌아갈 때만 사전에 넣음
    }
}
