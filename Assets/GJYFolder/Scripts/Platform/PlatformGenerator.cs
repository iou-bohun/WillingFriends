using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class PlatformGenerator : MonoBehaviour
{
    public static PlatformGenerator Instance;

    public Action<int> OnPlayerMove; // Player와 연동 시 사용.

    private Queue<PlatformBase> _platformsQueue = new Queue<PlatformBase>();
    private PlatformBase _latestPlatform;

    [Header("# Init")]
    [SerializeField] Transform _root;
    [Range(-10, 10)] public int _startPositionZ;
    [Range(0, 30)] public int _initRandPlatformsCount;
    [Range(0, 30)] public int _initLandPlatformsCount;
    public int _autoDisableIndex = 20;

    [Header("# Test")]
    public GameObject testPlayer;

    private Vector3 _latestPlatformPos;
    private int _currentStep = 0;

    public bool IsInit { get; private set; } = false;

    private string[] _platformTypes = Enum.GetNames(typeof(PlatformType));

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        _latestPlatformPos = Vector3.forward * _startPositionZ;

        for (int i = 0; i < _initLandPlatformsCount; i++)
            InitialPlatformGenerate();

        IsInit = true;

        for (int i = 0; i < _initRandPlatformsCount; i++)
            if (!IsEssentialPlatform())
                GeneratePlatform();
    }

    // Test
    private void Update()
    {
        if (testPlayer == null)
            return;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (_currentStep > _autoDisableIndex)
            {
                DisableOldestPlatform();
                return;
            }

            _currentStep++;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {            
            _currentStep--;
        }
    }

    // 게임 시작 시 페어 LandPlatform 안전구역 만들기
    private void InitialPlatformGenerate()
    {
        string platformType = _platformTypes[0];

        if (_latestPlatform == null)
        {
            DeployPlatform(platformType);
            return;
        }

        if (_latestPlatform.Tag != platformType)
        {
            DeployPlatform(platformType);
            return;
        }

        if (_latestPlatform.TryGetComponent(out ContinuousPlatform continuous) == false)
            Debug.Log($"ContinuousPlatform 이 없습니다. 잘못 설정한듯? : {platformType}");

        DeployPlatform(continuous.NextPair);
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
        GameObject go = ObjectPoolManager.GetObject(platformType, _root);
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
        string randType = _platformTypes[Random.Range(0, _platformTypes.Length)];

        ContinuousPlatform continuousPlatform = GetChildPlatform<ContinuousPlatform>();
        if (continuousPlatform != null)
        {
            // 마지막 플랫폼이 1번 플랫폼이면, 연속 플랫폼인지 확인 후 마지막이 아니면 다음 플랫폼 태그를 뽑는다.
            if (_latestPlatform.Tag == randType && continuousPlatform.IsLast == false)
                return continuousPlatform.NextPair;

            // 마지막 플랫폼의 타입이 생성될 타입과 같고, 순환이 가능하면 바로 생성한다.
            if (_latestPlatform.platformType.ToString() == randType && continuousPlatform.IsCyclable)
                return randType;
        }

        // 순환이 안 되거나, 같은 종류의 플랫폼이면 다시 뽑는다.
        if (_latestPlatform.platformType == CheckNextPlatformType(randType))
            return GetRandomTypeName();

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
        ObjectPoolManager.ReturnObject(platform.Tag, platform.gameObject);

        if (!IsEssentialPlatform())
            GenerateRandomPlatform();
    }
    #endregion

    #region Util
    private PlatformType CheckNextPlatformType(string platformType)
    {
        PlatformBase platform = ObjectPoolManager.PeekObject(platformType).GetComponent<PlatformBase>();

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
