using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverFlatform : Platform
{
    [SerializeField] RiverPlatformSO _riverSO;
    [SerializeField] Transform _startTransform;

    private ContinuousPlatform _platform;

    private readonly int START_POS_ABS = 12;
    private readonly int SPAWN_COUNT = 8;
    private readonly int LIMIT_STACK = 2;

    private static int CrossCounter;

    private void OnEnable()
    {
        if (!isInit)
        {
            isInit = true;
            _platform = GetComponent<ContinuousPlatform>();
            return;
        }

        Clear();
        
        if (!_platform.IsCyclable)
            Setup();
        else
            StartCoroutine(Co_SpawnLogs());
    }

    protected override void Setup()
    {
        Vector3 spawnPos = _startTransform.position;
        int stack = 0;

        for (int i = 0; i < SPAWN_COUNT; i++)
        {            
            int randPercentage = Random.Range(0, 10);

            if (randPercentage > 2 || stack == LIMIT_STACK)
            {
                spawnPos += Vector3.right;
                stack = 0;
                if (i == SPAWN_COUNT - 1 && _obstacleQueue.Count == 0)
                {
                    GameObject essentiallotus = ObjectPoolManager.Instance.GetObject(_riverSO.lotusPrefab.name, transform);
                    Vector3 randPos = _startTransform.position + Vector3.right * Random.Range(1, LIMIT_STACK - 1);
                    essentiallotus.transform.position = randPos;
                    _obstacleQueue.Enqueue(essentiallotus);
                }
                continue;
            }            

            GameObject lotus = ObjectPoolManager.Instance.GetObject(_riverSO.lotusPrefab.name, transform);
            lotus.transform.position = spawnPos;
            spawnPos += Vector3.right;
            stack++;

            _obstacleQueue.Enqueue(lotus);
        }
    }

    private IEnumerator Co_SpawnLogs()
    {
        CrossCounter++;

        int randLog = Random.Range(0, _riverSO.spawnDefaultPrefabs.Length);
        float randSpeed = Random.Range(_riverSO.minSpeed, _riverSO.maxSpeed);

        int flowDir = CrossCounter % 2 == 0 ? -1 : 1;

        while (true)
        {
            GameObject go = ObjectPoolManager.Instance.GetObject(_riverSO.spawnDefaultPrefabs[randLog].name, transform);
            go.transform.position = transform.position + (Vector3.left * flowDir * START_POS_ABS);            

            Log log = go.GetComponent<Log>();
            log.Setup(this, transform.right * flowDir, randSpeed);

            _obstacleQueue.Enqueue(go);

            float randomDelay = Random.Range(_riverSO.minSpawnDelay, _riverSO.maxSpawnDelay);
            yield return new WaitForSeconds(randomDelay);
        }
    }

    public void DisableLog()
    {
        if (_obstacleQueue.Count == 0)
            return;

        GameObject firstLog = _obstacleQueue.Dequeue();
        ObjectPoolManager.Instance.ReturnObject(firstLog.name, firstLog);
    }

    public override void Clear()
    {
        StopAllCoroutines();

        base.Clear();
    }
}
