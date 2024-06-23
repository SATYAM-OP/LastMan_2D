using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : Singleton<ObjectPooler>
{

    //TODO:
    //1. Add delegate to the ReturnToPool method.
   

    private Dictionary<string, List<GameObject>> pooledObjects;
    private Dictionary<string, Transform> poolParents;

    protected override void Awake()
    {
        base.Awake();

        pooledObjects = new Dictionary<string, List<GameObject>>();
        poolParents = new Dictionary<string, Transform>();
    }

    /// <summary>
    /// Use this method to check whether the pool already exsist or not
    /// </summary>
    /// <param name="prefab">prefab of which the pool we are checking</param>
    /// <returns></returns>
    public bool PoolExsist(GameObject prefab)
    {
        return pooledObjects.ContainsKey(prefab.name);
    }

    /// <summary>
    /// Use this method to check whether the pool already exsist or not
    /// </summary>
    /// <param name="key">string key is required (could be prefab name)</param>
    /// <returns></returns>
    public bool PoolExsist(string key)
    {
        return pooledObjects.ContainsKey(key);
    }

    /// <summary>
    /// Get the pool size of the given key(name of the prefab)
    /// </summary>
    /// <param name="key">name of the prefab</param>
    /// <returns></returns>
    public int GetPoolSize(string key)
    {
        return poolParents[key].childCount;
    }

    public int GetPoolSize(GameObject prefab)
    {
        return GetPoolSize(prefab.name);
    }


    /// <summary>
    /// Should be called in start or OnEnable method of the script that is using it
    /// </summary>
    /// <param name="prefab">prefab that need to be initialized</param>
    /// <param name="size">size of the pool(default size = 20)</param>
    /// <param name="parent">parent of the pooled objects (by default is null)</param>
    public void InitializePool(GameObject prefab, int size = 20, Transform parent = null)
    {
        string key = prefab.name;

        if (!PoolExsist(key))
        {
            pooledObjects.Add(key, new List<GameObject>());
            poolParents.Add(key, parent);
        }

        for (int i = 0; i < size; i++)
        {
            GameObject obj = null;

            if (parent == null)
            {
                 obj = Instantiate(prefab);
                 obj.name = prefab.name;
            }
            else
            {
                obj = Instantiate(prefab, parent);
                obj.name = prefab.name;
            }
            obj.SetActive(false);
            pooledObjects[key].Add(obj);
        }
        //Debug.Log("Parent of pool : " + poolParents[key].name);
    }

    /// <summary>
    /// Use to get the pooled object of a particular prefab.
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public GameObject GetPooledObject(GameObject prefab)
    {
        string key = prefab.name;

        if (pooledObjects.ContainsKey(key))
        {
            foreach (GameObject obj in pooledObjects[key])
            {
                if (!obj.activeInHierarchy)
                {
                    return obj;
                }
            }
        }

        Debug.LogWarning("No available objects in the pool for: " + key);
        return null;
    }

    /// <summary>
    /// Disable the objects and set other parameters to default for fututre use.
    /// </summary>
    /// <param name="obj">game object to deactivate</param>
    /// <param name="functionToCallAfterReturningToPool">Action function to call after the completion</param>
    public void ReturnToPool(GameObject obj,Action functionToCallAfterReturningToPool = null)
    {
        //Debug.Log(obj.name + " returned to pool");

        obj.SetActive(false);
        obj.transform.SetParent(poolParents[obj.name]);

        functionToCallAfterReturningToPool?.Invoke();
    }

}
