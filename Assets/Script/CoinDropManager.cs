using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDropManager : MonoBehaviour
{
    public static CoinDropManager Instance;
    public GameObject coin;
    private int amount = 0;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public void GenerateCoin(Vector3 position, bool isBoss)
    {
        //if (Random.value() < (1 - coinDropRate)) return;
        position.x += Random.Range(2, 4);
        position.y += Random.Range(2, 4);
        GameObject coinDrop = Instantiate(coin,position,Quaternion.identity);
        if (isBoss) amount = Random.Range(Timer.Instance.minutes * 20, Timer.Instance.minutes * 60);
        else amount = Random.Range(Timer.Instance.minutes * 5, Timer.Instance.minutes * 10);
        CollectibleItems _coin = coinDrop.GetComponent<CollectibleItems>();
        _coin.SetValue(amount);
    }
}
