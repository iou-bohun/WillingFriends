using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class PlatformGenerator : MonoBehaviour
{
    // To Do - µñ¼Å³Ê¸®·Î ¹Ù²Ù±â (Ç®¸µ & Resources¶û ¿¬°è)
    // private Dictionary<Type, Platform[]> _platforms = new Dictionary<Type, Platform[]>();

    [Header("# Collection_Temp")]
    public List<GameObject> _prefabs;
    public List<Platform> _platforms;

    [Header("# Init")]
    [Range(0, 10)] public int _startPositionZ;

    [Header("# Test")]
    [Range(0, 10)] public int _testInitPlatformsCount;
    

    private Vector3 _lastPlatformPos;
    private WaitForSeconds _waitTime = new WaitForSeconds(1);

    [Header("# CustomEditor")]
    public bool _isGenerating = false;

    private void Awake()
    {
        _lastPlatformPos = Vector3.forward * _startPositionZ;        

        for (int i = 0; i < _testInitPlatformsCount; i++)
        {
            int randomTemp = Random.Range(0, _prefabs.Count);
            _platforms.Add(GenerateRandomPlatform(randomTemp));
        }
        
        StartCoroutine(AutoGenerate_Test());
    }

    public Platform GenerateRandomPlatform(int index)
    {
        GameObject clone = Instantiate(_prefabs[index], transform);

        Platform platform = clone.GetComponent<Platform>();
        platform.transform.position = _lastPlatformPos + Vector3.forward;
        _lastPlatformPos = platform.transform.position;

        return platform;
    }

    public Platform GenerateRandomPlatform(PlatformType platformType)
    {
        GameObject clone = Instantiate(_prefabs[(int)platformType], transform);

        Platform platform = clone.GetComponent<Platform>();
        platform.transform.position = _lastPlatformPos + Vector3.forward;

        return platform;
    }

    private IEnumerator AutoGenerate_Test()
    {
        _isGenerating = true;

        while (true)
        {
            yield return _waitTime;

            int randomTemp = Random.Range(0, _prefabs.Count);
            _platforms.Add(GenerateRandomPlatform(randomTemp));
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
}
