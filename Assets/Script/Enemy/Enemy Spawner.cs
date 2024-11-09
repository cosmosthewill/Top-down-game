using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject spawnEnemy;
    public float spawnInterval = 2f;
    Vector3 spawnArea;
    // Start is called before the first frame update
    void Start()
    {
        spawnArea = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0);
        StartCoroutine(SpawnEnemy(spawnInterval, spawnEnemy, spawnArea));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator SpawnEnemy(float spawnInterval, GameObject enemy, Vector3 spawnArea)
    {
        while (true) 
        {
            yield return new WaitForSeconds(spawnInterval);
            GameObject _enemy = Instantiate(enemy, spawnArea, Quaternion.identity);
            EnemyBasic _enemyScript = _enemy.GetComponent<EnemyBasic>();
            /*bool _shoting = (Random.Range(0, 2) == 0);//random shoting enemy
            _enemyScript.isRange = _shoting;
            _enemyScript.isShotable = _shoting;*/
        }
    }
}
