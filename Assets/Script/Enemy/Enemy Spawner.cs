using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public UnityEngine.GameObject[] spawnEnemy;
    public GameObject[] spawnBoss;
    private float waveInterval = 10f;
    private int enemiesPerWave = 5;
    private float minDistance = 20f;
    private float maxDistance = 60f;
    private float spawnInterval = 2f;
    Vector3 spawnArea;
    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.GameObject rdEnemy = spawnEnemy[Random.Range(0, spawnEnemy.Length)];
        // Calculate a random spawn position within the distance constraints
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float spawnDistance = Random.Range(minDistance, maxDistance);
        Vector3 spawnPosition = Player.Instance.ReturnPlayerCenter() + (Vector3)(randomDirection * spawnDistance);
        StartCoroutine(SpawnEnemy(spawnInterval, rdEnemy, spawnPosition));
    }

    // Update is called once per frame
    void Update()
    {
        enemiesPerWave = Mathf.Max(20, Timer.Instance.minutes * 2);
    }
    private IEnumerator SpawnEnemy(float spawnInterval, UnityEngine.GameObject enemy, Vector3 spawnPosition)
    {
        while (true) 
        {
            yield return new WaitForSeconds(spawnInterval);
            enemy = spawnEnemy[Random.Range(0, spawnEnemy.Length)];
            UnityEngine.GameObject _enemy = Instantiate(enemy, spawnPosition, Quaternion.identity);
            //EnemyBasic _enemyScript = _enemy.GetComponent<EnemyBasic>();
        }
    }

    private IEnumerator WaveSpawmEnemy()
    {
        while (true)
        {
            for (int i = 0; i < enemiesPerWave; i++)
            {
                SpawnEnemyNearPlayer();
                //yield return new WaitForSeconds(spawnInterval);
            }

            // Wait before spawning the next wave
            yield return new WaitForSeconds(waveInterval);
        }
    }

    private void SpawnEnemyNearPlayer()
    {

        // Calculate a random spawn position within the distance constraints
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float spawnDistance = Random.Range(minDistance, maxDistance);
        Vector3 spawnPosition = Player.Instance.ReturnPlayerCenter() + (Vector3)(randomDirection * spawnDistance);
        GameObject enemyToSpawn = spawnEnemy[Random.Range(0, spawnEnemy.Length)];
        Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
    }
}
