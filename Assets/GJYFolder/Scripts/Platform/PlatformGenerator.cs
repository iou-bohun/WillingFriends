using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class PlatformGenerator : MonoBehaviour
{
    public Action<int> OnGeneratePlatform; // 굳이 액션으로 할 필요가...? 있을거 같음 ㅇㅇ;; 플레이어가 앞으로 가는 Vector3 z값을 받아와야됨

    public List<GameObject> _prefabs = new List<GameObject>();

    private Queue<GameObject> _platformsQueue = new Queue<GameObject>();    

    [Header("# Init")]
    [Range(-10, 10)] public int _startPositionZ;

    [Header("# Test")]
    [Range(0, 20)]
    public int _initPlatformsCount;
    public int _autoDisableIndex = 20;

    private Vector3 _lastPlatformPos;
    private WaitForSeconds _waitTime = new WaitForSeconds(1);
    
    private int _currentStep = 0;

    [Header("# CustomEditor")]
    public bool _isGenerating = false;

    private void Awake()
    {
        _lastPlatformPos = Vector3.forward * _startPositionZ;

        for (int i = 0; i < _initPlatformsCount; i++)
        {
            // To Do - 생성할 Type 넘겨주기
            PlatformType type = PlatformType.Land;

            GenerateRandomPlatform(type);
        }
    }

    // Temp
    public GameObject testPlayer;

    private void Update()
    {
        // To Do - 앞으로 갈 때 인덱스가 추가, 인덱스가 추가 될 때마다 현재 생성된 개수와 검사해서 생성 혹은 무시

        if (Input.GetKeyDown(KeyCode.W))
        {
            testPlayer.transform.position += Vector3.forward;            

            if (_currentStep > _autoDisableIndex)
            {
                DisableOldestPlatform();
                return;
            }                

            _currentStep++;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            testPlayer.transform.position += Vector3.back;
            _currentStep--;
            Debug.Log($"뒤로 : 스텝 {_currentStep}");
            Debug.Log($"Queue Count{_platformsQueue.Count}");
        }
    }
    
    public void GenerateRandomPlatform(PlatformType platformType)
    {
        GameObject go = Instantiate(_prefabs[(int)platformType], transform);
        go.transform.position = _lastPlatformPos;
        _lastPlatformPos += Vector3.forward;

        if (go.TryGetComponent(out PlatformBase platform) == false)
        {
            Debug.Log("PlatformBase 컴포넌트가 없습니다.");
            return;
        }

        _platformsQueue.Enqueue(go);
        platform.Init();        
    }

    private void DisableOldestPlatform()
    {
        // To Do - 파괴를 풀링으로 교체
        if (_platformsQueue.Count == 0)
        {
            Debug.Log("플랫폼 Queue 개수 0 개... 그럴리가?");
            return;
        }

        GameObject go = _platformsQueue.Dequeue().gameObject;
        Destroy(go);

        GenerateRandomPlatform(PlatformType.Land);
    }

    #region Editor - Inspector Custom Button Methods
    private IEnumerator AutoGenerate_Test()
    {
        _isGenerating = true;

        while (true)
        {
            yield return _waitTime;
            
            GenerateRandomPlatform(PlatformType.Land);
        }
    }

    public void StartGenerate()
    {
        if (_isGenerating)
        {
            Debug.Log("Already being Generated");
            return;
        }

        StartCoroutine(AutoGenerate_Test());
    }

    public void StopGenerate()
    {
        StopAllCoroutines();
        _isGenerating = false;
    }
    #endregion
}
