using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

[System.Serializable]
public struct SoundInfo
{
    public string tag;
    public AudioClip clip;
    public float volume;
}
public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;

    [Header("Background")]
    [SerializeField]
    private AudioClip backgroundClip;//배경 음악
    private AudioSource backgroundAudioSource;//배경 음악 오디오 소스

    [SerializeField]
    [Range(0, 100f)] private float bgmVolume; //브금 0 ~ 100 조절
    private float maxBgmVolume = 0.5f; //최대 브금 크기
    [SerializeField]
    private bool isPlayingBgm = true;

    [Header("SoundInfo")]
    public List<SoundInfo> soundList; //할당할 효과음 리스트

    private Dictionary<string, AudioClip> audioDictionary = new Dictionary<string, AudioClip>();//해당하는 효과음들을 내부적으로 저장.
    private Queue<GameObject> audioQueue;//오디오 큐

    [SerializeField]
    private GameObject AudioSoundPrefab;
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
        InitializeQueue();
        InitializeDictionary();
    }
    public GameObject CreateSoundObject()
    {
        GameObject obj = Instantiate(AudioSoundPrefab);
        //GameObject obj = new GameObject("AudioSourceObject");
        obj.name = "AudioSourceObject";
        obj.transform.SetParent(transform);
        //obj.AddComponent<AudioSourceObject>();
        obj.SetActive(false);
        audioQueue.Enqueue(obj);
        return obj;
    }
    public void InitializeQueue()
    {
        audioQueue = new Queue<GameObject>();

        for (int i = 0; i < maxQueueCount; i++)
        {
            CreateSoundObject();//객체를 만들고 큐에 넣기
            Debug.Log("오디오 initilaize");
        }
    }
    public void InitializeDictionary()
    {
        audioDictionary = new Dictionary<string, AudioClip>();

        foreach (var soundInfo in soundList)
        {
            if (soundInfo.tag != "")
            {
                audioDictionary[soundInfo.tag] = soundInfo.clip; //사운드 정보로 사전에 tag값에 clip을 저장.
                Debug.Log($"{soundInfo.tag} 저장 성공");
            }
        }
    }
    void Start()
    {
        Debug.Log("오디오 start");
        backgroundAudioSource.clip = backgroundClip;
    }
    public void PlayAudioClip(string tag, Transform parent = null)
    {
        if (!audioDictionary.ContainsKey(tag)) //오디오 클립이 존재하는지 확인
        {
            Debug.Log("tag는 존재하지 않는 오디오 입니다.");
            return;
        }

        if (audioQueue.Count > 0)
        {
            GameObject obj = audioQueue.Dequeue();//큐에서 꺼낸다.
            AudioSourceObject objAudioSource = obj.GetComponent<AudioSourceObject>();
            objAudioSource.clip = audioDictionary[tag];
            obj.transform.SetParent(parent);//부모한테서 소리가 나게 지정.
            obj.gameObject.SetActive(true);//
        }
        else
        {
            GameObject newObj = CreateSoundObject();//새로 생성한다.
            AudioSourceObject objAudioSource = newObj.GetComponent<AudioSourceObject>();
            objAudioSource.clip = audioDictionary[tag];
            newObj.transform.SetParent(parent); //부모 지정.
            newObj.gameObject.SetActive(true);//활성화
            objAudioSource.clip = audioDictionary[tag];
        }
    }
    public void ReturnAudioSource(GameObject AudioObject)
    {
        AudioObject.SetActive(false);
        AudioObject.transform.SetParent(Instance.transform);
        audioQueue.Enqueue(AudioObject);
    }
    // Update is called once per frame
    void Update()
    {
        SetBgmVolume();
    }

    public void SetBgmVolume() //브금 볼륨 bgmVolume 값에 따른 설정
    {
        if (isPlayingBgm)
            backgroundAudioSource.volume = (bgmVolume * maxBgmVolume) / 100f;
        else
            backgroundAudioSource.volume = 0.0f;
    }
    public void MuteBgmButtun() //브금 껐다 켰다
    {
        isPlayingBgm = !isPlayingBgm;
    }
}
