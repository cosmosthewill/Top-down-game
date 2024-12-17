using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;
    public UnityEngine.GameObject[] spawnNormal;
    public GameObject[] spawnElite;
    public GameObject[] spawnBoss;

    private float spawnInterval = 2f;

    //normal
    private float normalWaveInterval = 30f;
    private int normalPerWave = 7;
    private float minDistance = 20f;
    private float maxDistance = 60f;
    private Coroutine spawnNormalCoroutine;
    private Coroutine spawnWaveNormalCoroutine;
    private bool stopCoroutine = false;
    //elite
    private float spawmEliteInterval = 30f;
    private float eliteWaveInterval = 10f;
    private int elitePerWave = 4;
    private Coroutine spawnEliteCoroutine;
    private Coroutine spawnWaveEliteCoroutine;

    //boss
    private float spawmBossInterval = 60f;
    private int bossPerWave = 4;
    private Coroutine spawnBossCoroutine;

    //control
    private int totalEnemiesOnField = 0;
    private int maxEnemiesOnField = 50;
    public LayerMask spawnLayerMask;
    public float spawnRadius = 5f;
    const int maxAttempts = 15;

    private int score = 0;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        spawnLayerMask = LayerMask.GetMask("Enemy", "Obstacles");
    }
    public void EnemyDestryed(bool deadByPlayer, bool isBoss)
    {
        if (deadByPlayer) scoreCount(isBoss);
        totalEnemiesOnField--;
    }
    public void scoreCount(bool isBoss)
    {
        if (isBoss) score += 200 * (Timer.Instance.minutes + 1) * 2;
        else score += 5 * (Timer.Instance.minutes + 1) * 2;
        
        Debug.Log("New score " + score);
    }

    public int GetScore()
    {
        return score;
    }
    
    Vector3 RandomPosNearPlayer()
    {
        for (int i = 0; i < maxAttempts; i++)
        {
            // Calculate a random spawn position within the distance constraints
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            float spawnDistance = Random.Range(minDistance, maxDistance);
            Vector3 spawnPosition = Player.Instance.ReturnPlayerCenter() + (Vector3)(randomDirection * spawnDistance);

            // Check if there's overlap with any other objects
            if (!Physics2D.OverlapCircle(spawnPosition, spawnRadius, spawnLayerMask))
            {
                return spawnPosition; // Valid position found
            }
        }
        return new Vector3(0, 0, 0);
    }
    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(WaitToSpawn());
    }

    IEnumerator WaitToSpawn()
    {
        yield return new WaitForSeconds(5f);
        //Normal
        spawnNormalCoroutine = StartCoroutine(SpawnEnemy(spawnInterval, spawnNormal, RandomPosNearPlayer()));
        spawnWaveNormalCoroutine = StartCoroutine(WaveSpawmEnemy(spawnNormal, normalPerWave, normalWaveInterval));

        //Elite
        spawnEliteCoroutine = StartCoroutine(SpawnEnemy(spawmEliteInterval, spawnElite, RandomPosNearPlayer()));

        //Boss
        spawnBossCoroutine = StartCoroutine(SpawnEnemy(spawmBossInterval, spawnBoss, RandomPosNearPlayer()));
    }

    // Update is called once per frame
    void Update()
    {
        normalPerWave = Mathf.Max(20, Timer.Instance.minutes * 2);
        if (Timer.Instance.minutes > 5)
        {
            spawmBossInterval = 30f;
            spawmEliteInterval = 15f;
        }

        else if (Timer.Instance.minutes > 10 && !stopCoroutine)
        {
            stopCoroutine = true;
            StopCoroutine(spawnNormalCoroutine);
            StopCoroutine(spawnWaveNormalCoroutine);
            spawmBossInterval = 15f;
            spawmEliteInterval = 2f;
            spawnWaveEliteCoroutine = StartCoroutine(WaveSpawmEnemy(spawnElite, elitePerWave, eliteWaveInterval));
        }
    }
    private IEnumerator SpawnEnemy(float spawnInterval, GameObject[] spawmArray, Vector3 spawnPosition)
    {
        while (true) 
        {
            if (totalEnemiesOnField < maxEnemiesOnField)
            {
                yield return new WaitForSeconds(spawnInterval);
                GameObject enemy = spawmArray[Random.Range(0, spawmArray.Length)];
                UnityEngine.GameObject _enemy = Instantiate(enemy, spawnPosition, Quaternion.identity);
                totalEnemiesOnField++;
            }
            else yield return null;
            //EnemyBasic _enemyScript = _enemy.GetComponent<EnemyBasic>();
        }
    }

    private IEnumerator WaveSpawmEnemy(GameObject[] spawmArray, int enemiesPerWave, float spawnWaveInterval)
    {
        // Wait before spawning the next wave
        while (true)
        {
            yield return new WaitForSeconds(spawnWaveInterval);
            for (int i = 0; i < enemiesPerWave; i++)
            {
                if(totalEnemiesOnField < maxEnemiesOnField)
                {
                    GameObject enemyToSpawn = spawmArray[Random.Range(0, spawmArray.Length)];
                    Instantiate(enemyToSpawn, RandomPosNearPlayer(), Quaternion.identity);
                    totalEnemiesOnField++;

                    yield return new WaitForSeconds(0.1f);

                }
                else yield return null;
            }

        }
    }
}
