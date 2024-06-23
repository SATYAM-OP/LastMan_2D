using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    //TODO:
    // 1. Randomize spawn time.
    // 2. Keep enemy count and add maximum number of enemies that can be present at a time.

    [Header("Functional Toogles")]
    [SerializeField] private bool canSpawn = true;

    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private Transform enemyParent;

    [Header("Wave system")]
    [SerializeField,Range(0f,5f)] private float minSpawnInterval = 5f;
    [SerializeField,Range(0f,10f)] private float maxSpawnInterval = 10f;

    [SerializeField] private float spawnIntervalDecrease = 2f;

    [Tooltip("Number of enemis spawning at a time")]
    [SerializeField] private int numberOfEnemyEachSpawn = 3;

    [Tooltip("Minimum distance at which enemy should be spawning from player")]
    [SerializeField,Range(1f,10f)] private float minimumSpawnDistance;

    [Tooltip("Maximum distance at which enemy should be spawning from player")]
    [SerializeField,Range(1f,10f)] private float maximumSpawnDistance;

    [Tooltip("Number of enemies per wave and its size will also determine the total number of waves (Give the number in ascending order.)")]
    [SerializeField] private List<int> enemyPerWaves;

    [SerializeField] private float waveEndTime;
    [SerializeField] private int enemySpawnNumberIncrease = 2;

    private int totalNumberOfEnemies;
    private int currentWave;
    private int numberOfEnemiesOfEachType;
    private float waveTimer;

    //Properties

    public static int EnemySpawned { get; set; } = 0;

    /// <summary>
    /// For other script to know the number of enemies without refrencing it.
    /// </summary>
    public static int TotalEnemies { get; private set; }

    /// <summary>
    /// Total or max number of enemies that can be spawned or present in the scene. 
    /// Use it for knowing the maximum number of enemies that are instailzed for that whole level.
    /// </summary>
    public int TotalNumberOfEnemies
    {
        get { return totalNumberOfEnemies; } 
        private set
        {
            int totalCount = 0;
            if (enemyPooler != null)
            {
                totalCount = 0;
                foreach (GameObject enemyPrefab in enemyPrefabs)
                {
                    totalCount += enemyPooler.GetPoolSize(enemyPrefab);
                }
                totalNumberOfEnemies = Mathf.Clamp(value, 0, totalCount);
            }

            totalNumberOfEnemies = value;
        }
    
    }

    /// <summary>
    /// Current wave that is going on (its 0 based)
    /// </summary>
    public int CurrentWave
    {
        get { return currentWave; }
        private set { 
            currentWave = Mathf.Clamp(value,0,enemyPerWaves.Count - 1);

            //Reducing other values
            numberOfEnemyEachSpawn += enemySpawnNumberIncrease;
            minSpawnInterval -= spawnIntervalDecrease;
            maximumSpawnDistance -= spawnIntervalDecrease;
        }
    }

    public int MaxEnemiesOfCurrentWave
    {
        get { return enemyPerWaves[CurrentWave]; }

        private set
        {
            enemyPerWaves[CurrentWave] = Mathf.Clamp(value,0,TotalNumberOfEnemies); 
        }
    }



    //private variables (external references).
    private ObjectPooler enemyPooler;


    private Vector3 lastEnemyPos;
    private Vector3 enemySpawnPos;
    private Quaternion spawnRotation;


    private Transform playerTransform;


    private void Awake()
    {
        //Finding player
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        //Setting up wave system.
        TotalNumberOfEnemies = enemyPerWaves[enemyPerWaves.Count -1];

        TotalEnemies = TotalNumberOfEnemies;

        numberOfEnemiesOfEachType = TotalNumberOfEnemies / 3;


    }
    private void Start()
    {
        //Setting up object pooler for enemy.
        enemyPooler = ObjectPooler.Instance;
        foreach (GameObject enemyPrefab in enemyPrefabs)
        {
            enemyPooler.InitializePool(enemyPrefab,numberOfEnemiesOfEachType,enemyParent);
        }


        //setting up spawner 
        CurrentWave = 0;
        //Debug.Log("BEfroe starting coroutine");
        StartCoroutine(SpawnEnemies());
    }

    private bool ShouldSpawn()
    {
        return canSpawn && EnemySpawned <= TotalNumberOfEnemies;
    }

    IEnumerator SpawnEnemies()
    {
        waveTimer += Time.deltaTime;
        if(waveTimer >= waveEndTime)
        {
            Debug.Log("Wave number increase");
            CurrentWave++;
        }
        while (ShouldSpawn())//Condition at which it should stop spawning like : if we are playing the level and the screen is not pasued.
        {
            //Debug.Log("Spawning");
            SpawnEnemy();
            float spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        for (int i = 0; i < numberOfEnemyEachSpawn; i++)
        {
            enemySpawnPos = GetRandomEdgePosition();
            //Debug.Log("Current spawn pos : "+ currentSpawnPos);
            /*while (currentEnemyPos == lastEnemyPos)
            {
                currentEnemyPos = GetRandomEdgePosition();
            }*/

            spawnRotation = Quaternion.identity;
            /*Instantiate(enemyPrefab, currentSpawnPos, spawnRotation,enemyParent);*/

            int indexOfEnemyToSpawn = Random.Range(0, enemyPrefabs.Count - 1);
            GameObject enemyGO = enemyPooler.GetPooledObject(enemyPrefabs[indexOfEnemyToSpawn]);
            if (enemyGO != null)
            {
                //Debug.Log("Enemy spawned");
                enemyGO.transform.SetPositionAndRotation(enemySpawnPos, spawnRotation);
                enemyGO.SetActive(true);
            }
        }
 


    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Returns the random world position of enemy</returns>
    private Vector3 GetRandomEdgePosition()
    {
        Vector3 randomOffset = RandomMethod1();

        Vector3 spawnPos = playerTransform.position + randomOffset;
     
        return spawnPos;
    }


    private Vector3 RandomMethod1()
    {
        Vector3 randomOffset = Vector3.zero;
        float distance = 0f;
        do
        {

            float xRange1 = Random.Range(-minimumSpawnDistance, minimumSpawnDistance);
            float xRange2 = Random.Range(-maximumSpawnDistance, maximumSpawnDistance);
            float x = Random.Range(xRange1, xRange2);
            randomOffset.x = x;


            float yRange1 = Random.Range(-minimumSpawnDistance, minimumSpawnDistance);
            float yRange2 = Random.Range(-maximumSpawnDistance, maximumSpawnDistance);
            float y = Random.Range(yRange1, yRange2);
            randomOffset.y = y;

            distance = Vector2.Distance(randomOffset,playerTransform.position);

        } while (distance < minimumSpawnDistance);
        return randomOffset;

    }

   
}
