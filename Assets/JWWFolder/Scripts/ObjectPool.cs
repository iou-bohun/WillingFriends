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
    public List<Pooling> pools;//Ǯ�� ����Ʈ
    private Pooling poolingRef = new Pooling();//Ǯ�� ����

    public static ObjectPool Instance;

    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

    private void Awake()
    {
        Instance = this;

        Initialize();
    }
    private void Initialize() // ������ ���� ��ŭ ������Ʈ ���� �ʱ�ȭ
    {
        foreach (var pool in pools)//����Ʈ���� ������
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
        obj.gameObject.SetActive(false);

        return obj;
    }
    public static GameObject GetObject(string tag)//������Ʈ�� Ǯ���� ������ ��ȯ
    {
        if (!Instance.poolDictionary.ContainsKey(tag))//��ųʸ��� ������ ��
            return null;

        if (Instance.poolDictionary[tag].Count > 0)//ť�� �����ִ� �� ���� �� 
        {
            GameObject obj = Instance.poolDictionary[tag].Dequeue();//ť���� ������.
            obj.transform.SetParent(null);//�������� ��ü�� �и�
            obj.gameObject.SetActive(true);//Ȱ��ȭ
            return obj;
        }
        else//ť�� �����ִ� �� ���� ��
        {
            GameObject newObj = Instance.CreateNewObject(tag);//�����
            newObj.transform.SetParent(null);//����
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
}
