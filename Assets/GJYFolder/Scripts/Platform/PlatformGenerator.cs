using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class PlatformGenerator : MonoBehaviour
{
    public static PlatformGenerator Instance;

    public Action<int> OnGeneratePlatform; // Player�� ���� �� ���.

    private Queue<PlatformBase> _platformsQueue = new Queue<PlatformBase>();
    private PlatformBase _latestPlatform;

    [Header("# Init")]
    [Range(-10, 10)] public int _startPositionZ;
    [Range(0, 30)] public int _initPlatformsCount;
    public int _autoDisableIndex = 20;

    [Header("# Test")]
    public GameObject testPlayer;

    private Vector3 _latestPlatformPos;
    private int _currentStep = 0;

    private string[] _platformTypes = Enum.GetNames(typeof(PlatformType));

    private int spawnCount;

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

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.W))
    //    {
    //        testPlayer.transform.position += Vector3.forward;

    //        if (_currentStep > _autoDisableIndex)
    //        {
    //            DisableOldestPlatform();
    //            return;
    //        }

    //        _currentStep++;
    //    }
    //    if (Input.GetKeyDown(KeyCode.S))
    //    {
    //        testPlayer.transform.position += Vector3.back;
    //        _currentStep--;
    //    }
    //}

    #region �÷��� ���� Generate Platform
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
        GameObject go = ObjectPoolManager.GetObject(platformType);
        go.transform.position = _latestPlatformPos;
        _latestPlatformPos += Vector3.forward;

        if (go.TryGetComponent(out PlatformBase platform) == false)
        {
            Debug.Log("PlatformBase ������Ʈ�� �����ϴ�.");
            return;
        }

        // To Do - �ش� �÷����� Init �Լ� ȣ���Ű��. ������ ������Ʈ ���� �����??

        _latestPlatform = platform;
        _platformsQueue.Enqueue(platform);
    }

    private string GetRandomTypeName()
    {
        // Enum���� �� �÷����� 1�� �±׸� �ҷ��� �����ϰ� ����        
        string randType = _platformTypes[Random.Range(0, _platformTypes.Length)];        

        if (_latestPlatform == null)
            return randType;

        ContinuousPlatform continuousPlatform = GetChildPlatform<ContinuousPlatform>();
        if (continuousPlatform != null)
        {
            // ������ �÷����� 1�� �÷����̸�, ���� �÷������� Ȯ�� �� �������� �ƴϸ� ���� �÷��� �±׸� �̴´�.
            if (_latestPlatform.Tag == randType && continuousPlatform.IsLast == false)
                return continuousPlatform.NextPair;

            // ������ �÷����� Ÿ���� ������ Ÿ�԰� ����, ��ȯ�� �����ϸ� �ٷ� �����Ѵ�.
            if (_latestPlatform.platformType.ToString() == randType && continuousPlatform.IsCyclable)
                return randType;
        }

        // ��ȯ�� �� �ǰų�, ���� ������ �÷����̸� �ٽ� �̴´�.
        if (_latestPlatform.platformType == CheckNextPlatformType(randType))
            return GetRandomTypeName();

        // �׳� �ٸ���
        return randType;
    }    

    // ���� ���� �÷����� ����� �� �÷��� ����
    private void DisableOldestPlatform()
    {
        if (_platformsQueue.Count == 0)
        {
            Debug.Log("�÷��� Queue ���� 0 ��... �׷�����?");
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

    // ������ �÷����� �ݵ�� �ڽ��� �� �;��ϴ� �÷������� �˻�
    private bool IsEssentialPlatform()
    {
        ContinuousPlatform continuousPlatform = GetChildPlatform<ContinuousPlatform>();

        // �ڽ��� �� �;��ϸ� ���� �� true ��ȯ
        if (continuousPlatform != null && continuousPlatform.IsEssential)
        {
            // �� ���� �ؾ��ϴµ� �߰����� �ߺ��� �����ϸ� Ȯ����
            if (continuousPlatform.IsMid)
            {
                string random = Random.Range(0, 10) < 5 ? continuousPlatform.NextPair : continuousPlatform.Tag;
                GenerateEssentialPlatform(random);
                return true;
            }

            // �߰��� ���� 1, 2 ���� �ٷ� ���� �÷���
            GenerateEssentialPlatform(continuousPlatform.NextPair);
            return true;
        }

        return false;
    }

    // Continuous �Ǵ� Single Platform�� ���ʸ��� Ȱ���� ��ȯ
    private T GetChildPlatform<T>() where T : PlatformBase
    {
        if (_latestPlatform == null || _latestPlatform.TryGetComponent(out T continuous) == false)
            return null;

        return continuous;
    }
    #endregion
}
