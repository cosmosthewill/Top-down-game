using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
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

    Vector3 RandomPosNearPlayer()
    {
        // Calculate a random spawn position within the distance constraints
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float spawnDistance = Random.Range(minDistance, maxDistance);
        Vector3 spawnPosition = Player.Instance.ReturnPlayerCenter() + (Vector3)(randomDirection * spawnDistance);
        return spawnPosition;
    }
    // Start is called before the first frame update
    void Start()
    {
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
            Debug.Log("a");
            spawmBossInterval = 30f;
            spawmEliteInterval = 15f;
        }

        else if (Timer.Instance.minutes > 10 && !stopCoroutine)
        {
            Debug.Log("b");
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
            yield return new WaitForSeconds(spawnInterval);
            GameObject enemy = spawmArray[Random.Range(0, spawmArray.Length)];
            UnityEngine.GameObject _enemy = Instantiate(enemy, spawnPosition, Quaternion.identity);
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
                GameObject enemyToSpawn = spawmArray[Random.Range(0, spawmArray.Length)];
                Instantiate(enemyToSpawn, RandomPosNearPlayer(), Quaternion.identity);

                yield return new WaitForSeconds(0.1f);
            }

        }
    }
}
