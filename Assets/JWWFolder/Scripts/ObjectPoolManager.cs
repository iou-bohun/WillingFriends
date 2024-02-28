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

    public List<Pooling> pools;//Ǯ�� ����Ʈ
    private Pooling poolingRef = new Pooling();//Ǯ�� ����

    private Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        Debug.Log("ObjectPoolmanager awake");
        //Initialize();
        Managers.InitEvent += Initialize;
        Managers.ClearEvent += Clear;
        Initialize();
    }
    private void Start()
    {
        Managers.ClearEvent += ClearDictionary;
    }
    private void ClearDictionary()
    {
        poolDictionary.Clear();//���� �ʱ�ȭ

        int num = transform.childCount;//��Ȱ��ȭ �Ǿ��ִ� ������Ʈ ������ ��
        for(int i = 0; i < num; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
            Destroy(transform.GetChild(i).gameObject);
            Debug.Log("���� �Ϸ�");
        }

        Initialize();//�� ����
    }

    public void Initialize() // ������ ���� ��ŭ ������Ʈ ���� �ʱ�ȭ
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (var pool in pools)//����Ʈ���� ������
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
        Debug.Log("ObjectPoolManager ����");
    }
    private GameObject CreateNewObject(string tag)// ������Ʈ ���� �� ��ȯ //Get������ ���
    {
        if (!poolDictionary.ContainsKey(tag))//�ִ� ������ �����ϰڴ�!
            return null;

        foreach (var pool in pools) 
        {
            if (pool.tag == tag)//�±װ� ��ġ�ϸ�
            {
                poolingRef = pool;
                break;
            }
        }
        GameObject obj = Instantiate(poolingRef.prefab, transform);//������Ʈ ����
        obj.name = tag;
        obj.gameObject.SetActive(false);

        return obj;
    }
    public static GameObject GetObject(string tag, Transform parent = null) //������Ʈ�� Ǯ���� ������ ��ȯ
    {
        if (!Instance.poolDictionary.ContainsKey(tag))//��ųʸ��� ������ ��
            return null;

        if (Instance.poolDictionary[tag].Count > 0)//ť�� �����ִ� �� ���� �� 
        {
            GameObject obj = Instance.poolDictionary[tag].Dequeue();//ť���� ������.
            obj.transform.SetParent(parent); // parent �ڽ����� (��������?)
            obj.gameObject.SetActive(true);//Ȱ��ȭ            
            return obj;
        }
        else//ť�� �����ִ� �� ���� ��
        {
            GameObject newObj = Instance.CreateNewObject(tag);//�����
            newObj.transform.SetParent(parent); // parent �ڽ����� (��������?)
            newObj.gameObject.SetActive(true);//Ȱ��ȭ
            return newObj;
        }
    }

    public static void ReturnObject(string tag ,GameObject gameObject)//������Ʈ�� tag�� �ش��ϴ� Ǯ�� �������
    {
        gameObject.gameObject.SetActive(false); //��Ȱ��ȭ
        gameObject.transform.SetParent(Instance.transform); // Ǯ ������ ����
        Instance.poolDictionary[tag].Enqueue(gameObject); //���ư� ���� ������ ����
    }

    // ��� ������ ���⸸ �� ģ��
    public static GameObject PeekObject(string tag)
    {
        if (!Instance.poolDictionary.ContainsKey(tag))//��ųʸ��� ��ϵȰ� �ƴϸ� null
            return null;

        if (Instance.poolDictionary[tag].Count > 0)//ť�� �����ִ� �� ���� �� 
            return Instance.poolDictionary[tag].Peek();

        // ��Ʈ�� ������ ������ �¸� Peek
        Instance.poolDictionary[tag].Enqueue(Instance.CreateNewObject(tag));
        return Instance.poolDictionary[tag].Peek();
    }
}
