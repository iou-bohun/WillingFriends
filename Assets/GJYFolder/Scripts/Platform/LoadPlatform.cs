using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPlatform : Platform
{
    [SerializeField] LoadPlatformSO _loadSO;

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
        int randCar = Random.Range(0, _loadSO.carPrefabs.Length);        
        float randSpeed = Random.Range(_loadSO.minSpeed, _loadSO.maxSpeed);

        int driveDir = Random.Range(0, 2) == 0 ? -1 : 1;

        while (true)
        {
            GameObject go = ObjectPool.GetObject(_loadSO.carPrefabs[randCar].name, transform);
            go.transform.position = transform.position + (Vector3.left * driveDir * START_POS_ABS);
            go.transform.Rotate(Vector3.up * 90 * driveDir);

            Car car = go.GetComponent<Car>();
            car.Setup(this, transform.right * driveDir, randSpeed);

            _obstacleQueue.Enqueue(go);

            float randomDealy = Random.Range(_loadSO.minSpawnDelay, _loadSO.maxSpawnDelay);
            yield return new WaitForSeconds(randomDealy);
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
