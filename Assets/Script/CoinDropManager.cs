using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDropManager : MonoBehaviour
{
    public static CoinDropManager Instance;
    public GameObject coin;
    public GameObject food;
    private float amount = 0;
    public float chanceDropFood = 0.2f;
    public float chanceDropExp = 0.4f;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public void GenerateCoin(Vector3 position, bool isBoss)
    {
        if (Random.value < (1 - PlayerStatsManager.Instance.baseCoinDropRate)) return;
        position.x += Random.Range(2, 4);
        position.y += Random.Range(2, 4);
        if (isBoss) amount = Random.Range((Timer.Instance.minutes + 1) * 20, (Timer.Instance.minutes + 1) * 60);
        else amount = Random.Range((Timer.Instance.minutes + 1) * 5, (Timer.Instance.minutes + 1) * 10);
        amount *= (1 + PlayerStatsManager.Instance.bonusCoinAmount);
        GameObject coinDrop = Instantiate(coin,position,Quaternion.identity);
        CollectibleItems _coin = coinDrop.GetComponent<CollectibleItems>();
        _coin.SetValue((int)amount);
    }
    public void GenerateFood(Vector3 position,bool isBoss)
    {
        if (isBoss) chanceDropFood = Mathf.Clamp01(PlayerStatsManager.Instance.baseFoodDropRate + 0.2f);
        else chanceDropFood = PlayerStatsManager.Instance.baseFoodDropRate;
        position.x += Random.Range(2, 4);
        position.y += Random.Range(2, 4);
        if(Random.value < chanceDropFood)
        {
            GameObject foodDrop = Instantiate(food, position, Quaternion.identity);
            CollectibleItems _food = foodDrop.GetComponent<CollectibleItems>();
            _food.SetValue((int)(20f + Timer.Instance.minutes * 2));
        }
        
    }
    public void GenerateExp(Vector3 position,bool isBoss, GameObject exp, int amount) 
    {
        if (isBoss) chanceDropExp = Mathf.Clamp01(PlayerStatsManager.Instance.baseExpDropRate + 0.2f);
        else chanceDropExp = PlayerStatsManager.Instance.baseExpDropRate;
        position += new Vector3(Random.Range(2, 4), Random.Range(2, 4), 0);
        if(Random.value < chanceDropExp)
        {
            GameObject expDrop = Instantiate(exp, position, Quaternion.identity);
            CollectibleItems _exp = expDrop.GetComponent<CollectibleItems>();
            _exp.SetValue(amount);
        }
    }
}
