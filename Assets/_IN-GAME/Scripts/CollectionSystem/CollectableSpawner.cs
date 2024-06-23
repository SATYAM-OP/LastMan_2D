using System.Collections.Generic;
using UnityEngine;

public class CollectableSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> collectables;

    [SerializeField] private Transform collectableParent;

    private ObjectPooler pooler;


    private void Awake()
    {
        if (collectableParent == null)
            collectableParent = transform;

    }

    private void Start()
    {
        pooler = ObjectPooler.Instance;

        //Intialize the pool.
        //Debug.Log("Total enemies are : " + EnemySpawner.TotalEnemies);
        for (int i = 0; i < collectables.Count; i++)
        {
            //Debug.Log("Pooled intailized");
            pooler.InitializePool(collectables[i], 100, collectableParent);
            //Debug.Log(pooler.PoolExsist(collectables[i]));
        }
        
    }


    /// <summary>
    /// Spawn collectables
    /// </summary>
    /// <param name="index">Index of prefab which should be spawned</param>
    /// <param name="pos">Position of spawned object</param>
    /// <param name="rot">Rotation of spawned object</param>
    public void SpawnCollectable(int index,Vector2 pos, Quaternion rot)
    {
        //Debug.Log("Spawned");
        if (index >= 0 && index <= collectables.Count - 1)
        {
            //Debug.Log("It can be spawned");
            GameObject collectableGO = pooler.GetPooledObject(collectables[index]);
            if(collectableGO != null)
            {
                //Debug.Log("Got it from the pool");
                collectableGO.transform.SetPositionAndRotation(pos, rot);
                collectableGO.SetActive(true);
            }
        }
    }


    public int MaxNumberOfPrefabs { get { return collectables.Count; } }




}
