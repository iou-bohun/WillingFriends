using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum sound
{
    Start,//���� ����
    Shop, //����
    MaxCount
}

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;

    [Header("Background")]
    [SerializeField]
    private AudioClip backgroundClip;//��� ����
    private AudioSource backgroundAudioSource;//��� ���� ����� �ҽ�
    

    private Queue<AudioSource> audioQueue;//����� ť
    [SerializeField]
    private float maxQueueCount = 10f;
    
    public static SoundManager Instance {  get { return _instance; } }
    // Start is called before the first frame update
    private void Awake()
    {
        if(_instance == null)
            _instance = this;

        backgroundAudioSource = GetComponent<AudioSource>();
        Debug.Log("���� �Ŵ��� ����");
    }
    public void Initialize()
    {
        audioQueue = new Queue<AudioSource>();
        for (int i = 0; i < maxQueueCount; i++)
        {
            GameObject obj = new GameObject("AudioSource");
            obj.transform.SetParent(transform);
            AudioSource source =  obj.AddComponent<AudioSource>();
            obj.SetActive(false);
            audioQueue.Enqueue(source);
            Debug.Log("����� ����");
        }
    }
    void Start()
    {
        backgroundAudioSource.clip = backgroundClip;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
