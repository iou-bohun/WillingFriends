using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandPlatform : Platform
{
    [SerializeField] LandPlatformSO _landSO;
    [SerializeField] Transform _startTransform;    

    private readonly int SPAWN_COUNT = 24;
    private readonly int LIMIT_STACK = 4;

    private void OnEnable()
    {
        SpawnTrees();
    }

    private void SpawnTrees()
    {
        if (!isInit)
        {
            isInit = true;
            return;
        }

        Clear();

        Vector3 spawnPos = _startTransform.position;
        int stack = 0;

        for (int i = 0; i < SPAWN_COUNT; i++)
        {
            int randTree = Random.Range(0, _landSO.treePrefabs.Length);
            int randPercentage = Random.Range(0, 10);

            if (randPercentage > 1 || stack == LIMIT_STACK)
            {
                spawnPos += Vector3.right;
                stack = 0;
                continue;
            }

            GameObject tree = ObjectPool.GetObject(_landSO.treePrefabs[randTree].name, transform);
            tree.transform.position = spawnPos;
            spawnPos += Vector3.right;
            stack++;

            _obstacleQueue.Enqueue(tree);
        }
    }

    public override void Clear()
    {
        base.Clear();


    }
}
