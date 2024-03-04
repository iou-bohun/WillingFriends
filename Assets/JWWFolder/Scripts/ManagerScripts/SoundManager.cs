using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SoundInfo
{
    public string tag;
    public AudioClip clip;
    [Range(0,100)]public float volumePercent;
}
public class SoundManager : SingletoneBase<SoundManager>
{
    [Header("Background")]
    [SerializeField]
    private AudioClip backgroundClip;//배경 음악
    private AudioSource backgroundAudioSource;//배경 음악 오디오 소스

    [SerializeField]
    [Range(0, 100f)] private float bgmVolumePercent; //브금 0 ~ 100 조절
    [SerializeField]
    [Range(0.0f,1.0f)]private float maxBgmVolume = 0.5f; //최대 브금 0 ~ 1크기
    [SerializeField]
    private bool isPlayingBgm = true;

    [Header("SoundInfo")]
    public List<SoundInfo> soundEffectList; //할당할 효과음 리스트

    private Dictionary<string, SoundInfo> audioDictionary = new Dictionary<string, SoundInfo>();//해당하는 효과음들을 내부적으로 저장.
    private Queue<GameObject> audioQueue;//오디오 큐

    [SerializeField]
    private GameObject AudioSoundPrefab;
    [SerializeField]
    private float maxQueueCount = 10f;

    
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();

        backgroundAudioSource = GetComponent<AudioSource>();
        Initialize();
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
        obj.transform.localPosition = Vector3.zero;
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
        audioDictionary = new Dictionary<string, SoundInfo>();

        foreach (var soundInfo in soundEffectList)
        {
            if (soundInfo.tag != "")
            {
                SoundInfo newSoundInfo = new SoundInfo();
                newSoundInfo.tag = soundInfo.tag;
                newSoundInfo.volumePercent = soundInfo.volumePercent;
                newSoundInfo.clip = soundInfo.clip;

                audioDictionary[soundInfo.tag] = newSoundInfo; //사운드 정보로 사전에 tag값에 clip을 저장.                
            }
        }
    }
    void Start()
    {        
        //backgroundAudioSource.clip = backgroundClip;
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
            objAudioSource.clip = audioDictionary[tag].clip;
            objAudioSource.voulme = PercentToDegree(audioDictionary[tag].volumePercent);
            obj.transform.SetParent(parent);//부모의 자식으로 들어감.
            if (parent != null)
            {
                obj.transform.position = parent.position;//부모의 위치에서 소리가 들리게 부착함.
            }
            obj.gameObject.SetActive(true);//
        }
        else
        {
            GameObject newObj = CreateSoundObject();//새로 생성한다.
            AudioSourceObject objAudioSource = newObj.GetComponent<AudioSourceObject>();
            objAudioSource.clip = audioDictionary[tag].clip;
            objAudioSource.voulme = PercentToDegree(audioDictionary[tag].volumePercent);
            newObj.transform.SetParent(parent); //부모의 자식으로 들어감.
            if (parent != null)
            {
                newObj.transform.position = parent.position;//부모의 위치에서 소리가 들리게 부착함.
            }
            newObj.gameObject.SetActive(true);//활성화
        }
    }
    public void ReturnAudioSource(GameObject AudioObject)//떠났던 사운드 오브젝트를 반환 받는다.
    {
        AudioObject.transform.SetParent(Instance.transform);
        AudioObject.transform.position = Instance.transform.position;
        AudioObject.SetActive(false);
        audioQueue.Enqueue(AudioObject);
    }
    // Update is called once per frame
    void Update()
    {
        //SetBgmVolume();
    }

    public void SetBgmVolume() //브금 볼륨 bgmVolume 값에 따른 설정
    {
        if (isPlayingBgm)
            backgroundAudioSource.volume = PercentToDegree(bgmVolumePercent) * maxBgmVolume;
        else
            backgroundAudioSource.volume = 0.0f;
    }
    public void MuteBgmButtun() //브금 껐다 켰다
    {
        isPlayingBgm = !isPlayingBgm;
    }
    public float PercentToDegree(float percent)
    {
        return percent / 100f;
    }
}
