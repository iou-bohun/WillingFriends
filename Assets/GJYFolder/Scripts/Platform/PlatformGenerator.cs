using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class PlatformGenerator : MonoBehaviour
{
    public static PlatformGenerator Instance;

    public Action<int> OnGeneratePlatform; // Player와 연동 시 사용.

    private Queue<PlatformBase> _platformsQueue = new Queue<PlatformBase>();
    private PlatformBase _latestPlatform;

    [Header("# Init")]
    [Range(-10, 10)] public int _startPositionZ;
    [Range(0, 20)] public int _initPlatformsCount;
    public int _autoDisableIndex = 20;

    [Header("# Test")]
    public GameObject testPlayer;

    private Vector3 _latestPlatformPos;
    private int _currentStep = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        _latestPlatformPos = Vector3.forward * _startPositionZ;

        for (int i = 0; i < _initPlatformsCount; i++)
            GeneratePlatform();
    }

    private void Update()
    {
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
        }
    }

    #region 플랫폼 생성 Generate Platform
    private void GeneratePlatform()
    {
        if (!IsEssentialPlatform())
            GenerateRandomPlatform();
    }

    private void GenerateEssentialPlatform(string platformType)
    {
        DeployPlatform(platformType);
    }

    public void GenerateRandomPlatform()
    {
        string platformType = GetRandomTypeName();

        DeployPlatform(platformType);
    }

    private void DeployPlatform(string platformType)
    {
        GameObject go = ObjectPool.GetObject(platformType);
        go.transform.position = _latestPlatformPos;
        _latestPlatformPos += Vector3.forward;

        if (go.TryGetComponent(out PlatformBase platform) == false)
        {
            Debug.Log("PlatformBase 컴포넌트가 없습니다.");
            return;
        }

        _latestPlatform = platform;
        _platformsQueue.Enqueue(platform);
    }

    private string GetRandomTypeName()
    {
        // Enum으로 각 플랫폼의 1번 태그를 불러와 랜덤하게 지정
        string[] types = Enum.GetNames(typeof(PlatformType));
        string randType = types[Random.Range(0, types.Length)];        

        if (_latestPlatform != null)
        {
            ContinuousPlatform continuousPlatform = GetChildPlatform<ContinuousPlatform>();
            if (continuousPlatform != null)
            {
                // 마지막 플랫폼이 1번 플랫폼이면, 연속 플랫폼인지 확인 후 마지막이 아니면 다음 플랫폼 태그를 뽑는다.
                if (_latestPlatform.Tag == randType && continuousPlatform.IsLast == false)
                    return continuousPlatform.NextPair;

                // 마지막 플랫폼이 연속 끝번 플랫폼이고, 다음에 올 플랫폼이 마지막 플랫폼과 동일한 종류면 다시 뽑는다.
                if (continuousPlatform.IsLast && continuousPlatform.platformType.ToString() == randType)
                    return GetRandomTypeName();
            }
            
            // 싱글, 연속 끝번에 상관없이 같은 종류의 플랫폼이면 다시 뽑는다.
            if(_latestPlatform.platformType == CheckNextPlatformType(randType))
                return GetRandomTypeName();
        }
        
        // 그냥 다른거
        return randType;
    }

    // 제일 뒤의 플랫폼을 지우고 새 플랫폼 생성
    private void DisableOldestPlatform()
    {
        if (_platformsQueue.Count == 0)
        {
            Debug.Log("플랫폼 Queue 개수 0 개... 그럴리가?");
            return;
        }

        PlatformBase platform = _platformsQueue.Dequeue();
        ObjectPool.ReturnObject(platform.Tag, platform.gameObject);

        if (!IsEssentialPlatform())
            GenerateRandomPlatform();
    }
    #endregion

    #region Util
    private PlatformType CheckNextPlatformType(string platformType)
    {
        PlatformBase platform = ObjectPool.PeekObject(platformType).GetComponent<PlatformBase>();

        return platform.platformType;
    }

    // 마지막 플랫폼이 반드시 자신의 페어가 와야하는 플랫폼인지 검사
    private bool IsEssentialPlatform()
    {
        ContinuousPlatform continuousPlatform = GetChildPlatform<ContinuousPlatform>();

        // 자신의 페어가 와야하면 생성 후 true 반환
        if (continuousPlatform != null && continuousPlatform.IsEssential)
        {
            // 페어가 오긴 해야하는데 중간꺼라 중복이 가능하면 확률적
            if (continuousPlatform.IsMid)
            {
                string random = Random.Range(0, 10) < 5 ? continuousPlatform.NextPair : continuousPlatform.Tag;
                GenerateEssentialPlatform(random);
                return true;
            }

            // 중간이 없는 1, 2 페어면 바로 다음 플랫폼
            GenerateEssentialPlatform(continuousPlatform.NextPair);
            return true;
        }

        return false;
    }

    // Continuous 또는 Single Platform을 제너릭을 활용해 반환
    private T GetChildPlatform<T>() where T : PlatformBase
    {
        if (_latestPlatform == null || _latestPlatform.TryGetComponent(out T continuous) == false)
            return null;

        return continuous;
    }
    #endregion
}
