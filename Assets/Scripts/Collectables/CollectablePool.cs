using System;
using System.Collections.Generic;
using UnityEngine;

public class CollectablePool : MonoBehaviour
{
    [SerializeField] private GameObject collectable;
    [SerializeField] private int poolNumber;
    [SerializeField] private CollectableType collectableType;

    public delegate void SpawnObject(CollectableType type, Vector2 position);
    public static SpawnObject spawnObject;

    private List<GameObject> _collectablePool;
    private Queue<int> _collectableQueue;

    private void OnEnable()
    {
        _collectablePool = new List<GameObject>(poolNumber);
        _collectableQueue = new Queue<int>(poolNumber);
        spawnObject += CollectionTypeCheck;
        PoolObjects();
    }

    private void OnDisable()
    {
        spawnObject -= CollectionTypeCheck;
    }

    private void PoolObjects()
    {
        for (var i = 0; i < poolNumber; i++)
        {
            var poolObj = Instantiate(collectable, gameObject.transform);
            _collectableQueue.Enqueue(i);
            _collectablePool.Add(poolObj);
        }
    }

    private void CollectionTypeCheck(CollectableType type, Vector2 emitterPosition)
    {
        if (type != collectableType) return;
        SpawnCollectable(emitterPosition);
    }

    private void SpawnCollectable(Vector2 spawnPosition)
    {
        var positionInQueue = _collectableQueue.Dequeue();
        var collectableToSpawn = _collectablePool[positionInQueue];
        collectableToSpawn.transform.position = spawnPosition;
        collectableToSpawn.SetActive(true);
        _collectableQueue.Enqueue(positionInQueue);
    }
}
