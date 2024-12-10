using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamboUltimate : UltimateBase
{
    public override float UltDuration => 10f;
    public override void Initialize(Transform playerTransform)
    {
        transform.SetParent(playerTransform);
    }
    public GameObject theReaper;
    
    private void Start()
    {
        GameObject reaper = Instantiate(theReaper,transform.position, Quaternion.identity);
        reaper.transform.SetParent(Player.Instance.transform);
        Destroy(reaper, 1.1f);
        PlayerStatsManager.Instance.critChance += 1;
        PlayerStatsManager.Instance.critMultiplier += 8;
    }
    private void Update() 
    {
        
    }
    private void OnDestroy()
    {
        PlayerStatsManager.Instance.critChance -= 1;
        PlayerStatsManager.Instance.critMultiplier -= 8;
    }
}
