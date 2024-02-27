using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPlatform : Platform
{
    [SerializeField] LoadPlatformSO _loadSO;

    private WaitForSeconds _delayTime;

    private readonly int START_POS_ABS = 10;

    private void OnEnable()
    {
        SpawnCars();
    }

    private void SpawnCars()
    {
        if (!isInit)
        {
            isInit = true;
            return;
        }

        Clear();

        StartCoroutine(Co_SpawnCars());
    }

    private IEnumerator Co_SpawnCars()
    {
        float randomDealy = Random.Range(_loadSO.minSpawnDelay, _loadSO.maxSpawnDelay);
        _delayTime = new WaitForSeconds(randomDealy);

        int randCar = Random.Range(0, _loadSO.carPrefabs.Length);
        int driveDir = Random.Range(0, 2) == 0 ? -1 : 1;

        while (true)
        {
            GameObject go = ObjectPool.GetObject(_loadSO.carPrefabs[randCar].name, transform);
            go.transform.position = transform.position + (Vector3.left * driveDir * START_POS_ABS);
            go.transform.Rotate(Vector3.up * 90 * driveDir);

            Car car = go.GetComponent<Car>();
            car.Setup(transform.right * driveDir, this);

            _obstacleQueue.Enqueue(go);

            yield return _delayTime;
        }
    }

    public void DisableCar()
    {
        if (_obstacleQueue.Count == 0)
            return;

        GameObject firstCar = _obstacleQueue.Dequeue();
        ObjectPool.ReturnObject(firstCar.name, firstCar);
    }

    public override void Clear()
    {
        StopAllCoroutines();

        base.Clear();
    }
}
