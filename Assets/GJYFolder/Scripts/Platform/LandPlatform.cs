using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandPlatform : Platform
{
    private Enemy[] _monsters;
    
    [SerializeField] LandPlatformSO _landSO;

    [Header("Tree Spawn Point")]
    [SerializeField] Transform _treeSpawnPoint;

    [Header("Monster Spawn Points")]
    [SerializeField] Transform _monsterSpawnStart;
    [SerializeField] Transform _monsterSpawnEnd;

    private Vector3 _monsterSpawnPos;

    private readonly int SPAWN_COUNT = 24;
    private readonly int LIMIT_STACK = 4;
    private readonly int MAX_SPAWN_MONSTER = 2;

    private readonly int PERCENTAGE = 1;

    private void Awake()
    {
        isInit = true;
        _monsters = new Enemy[MAX_SPAWN_MONSTER];
        _monsterSpawnPos = _monsterSpawnStart.position;
    }

    private void OnEnable()
    {
        Spawn();
    }

    private void Spawn()
    {
        // 풀에서 생성된 첫 1회는 발동 X
        if (!isInit)
            return;

        Clear();

        Vector3 treeSpawnPos = _treeSpawnPoint.position;
        int stack = 0; // 최대 4개만 연속으로 생성

        for (int i = 0; i < SPAWN_COUNT; i++)
        {
            int randTree = Random.Range(0, _landSO.spawnPrefabs.Length);
            int randPercentage = Random.Range(0, 10);

            // 생성X, 생성좌표 한 칸 옮기기
            if (randPercentage > PERCENTAGE || stack == LIMIT_STACK)
            {
                SpawnMonster(treeSpawnPos);

                treeSpawnPos += Vector3.right;
                stack = 0;
                continue;
            }

            // 나무 생성
            GameObject tree = ObjectPoolManager.GetObject(_landSO.spawnPrefabs[randTree].name, transform);
            tree.transform.position = treeSpawnPos;
            treeSpawnPos += Vector3.right;
            stack++;

            _obstacleQueue.Enqueue(tree);
        }
    }

    private void SpawnMonster(Vector3 treeSpawnPos)
    {
        // 현재 나무가 생성될 좌표가 Monster Start - End 사이가 아니라면 몬스터 생성 불가.
        if (treeSpawnPos.x < _monsterSpawnStart.position.x || 
            treeSpawnPos.x > _monsterSpawnEnd.position.x)
            return;

        // 몬스터도 확률적 스폰.
        int randPercentage = Random.Range(0, 10);
        if (randPercentage > PERCENTAGE)
            return;

        _monsterSpawnPos = treeSpawnPos;        

        for (int i = 0; i < MAX_SPAWN_MONSTER; i++)
        {
            if (_monsters[i] != null)
                continue;

            GameObject monster = ObjectPoolManager.GetObject(_landSO.monsterPrefabs.name, transform);
            monster.transform.position = _monsterSpawnPos;
            _monsters[i] = monster.GetComponent<Enemy>();
            break;
        }
    }

    public override void Clear()
    {
        for (int i = 0; i < MAX_SPAWN_MONSTER; i++)
        {
            if (_monsters[i] == null)
                continue;

            // 몬스터가 활성화면 풀에 반납이 안 된 것으로 판단.
            if (_monsters[i].gameObject.activeSelf)
                ObjectPoolManager.ReturnObject(_monsters[i].gameObject.name, _monsters[i].gameObject);

            // 풀에 반납이 되어도 배열에서 참조중이므로 다음 생성을 위해 null.
            _monsters[i] = null;
        }            

        _monsterSpawnPos = _monsterSpawnStart.position;

        base.Clear();
    }
}
