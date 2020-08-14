using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public List<Pool> pools;
    private static readonly Dictionary<int, Queue<GameObject>> PoolDictionary = new Dictionary<int, Queue<GameObject>>();

    private void Awake()
    {
        ConvertPoolListIntoQueue();
    }

    private void ConvertPoolListIntoQueue()
    {
        foreach (var pool in pools)
        {
            var objectPool = new Queue<GameObject>();

            for (var i = 0; i < pool.maximumActiveObjects; i++)
            {
                var temporaryVariable = Instantiate(pool.prefab);
                temporaryVariable.SetActive(false);
                objectPool.Enqueue(temporaryVariable);
            }
            if (PoolDictionary.ContainsKey(pool.index))
            {
                #if UNITY_EDITOR
                EndlessRunnerGameManager.DisplayDebugMessage("Replacing object pool " + pool.index);
                #endif
                PoolDictionary.Remove(pool.index);
            }
            PoolDictionary.Add(pool.index, objectPool);
        }
    }

    public static GameObject ReturnObjectFromPool(int index, Vector3 position, Quaternion rotation, bool setActive = true)
    {
        if (!PoolDictionary.ContainsKey(index))
        {
            Debug.LogError("Object pool contains no reference to index " + index);
            return null;
        }
        
            
        var returnObject = PoolDictionary[index].Dequeue();

        returnObject.transform.position = position;
        returnObject.transform.rotation = rotation;
        returnObject.SetActive(setActive);
            
        PoolDictionary[index].Enqueue(returnObject);
        return returnObject;
    }
}
    
[System.Serializable]
public struct Pool
{
    public int index;
    public GameObject prefab;
    public int maximumActiveObjects;
}