using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum sound
{
    Start,//개임 시작
    Shop, //상점
    MaxCount
}

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;

    [Header("Background")]
    [SerializeField]
    private AudioClip backgroundClip;//배경 음악
    private AudioSource backgroundAudioSource;//배경 음악 오디오 소스
    

    private Queue<AudioSource> audioQueue;//오디오 큐
    [SerializeField]
    private float maxQueueCount = 10f;
    
    public static SoundManager Instance {  get { return _instance; } }
    // Start is called before the first frame update
    private void Awake()
    {
        if(_instance == null)
            _instance = this;

        backgroundAudioSource = GetComponent<AudioSource>();
        Debug.Log("사운드 매니저 awake");
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
            Debug.Log("오디오 initilaize");
        }
    }
    void Start()
    {
        Debug.Log("오디오 start");
        backgroundAudioSource.clip = backgroundClip;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
