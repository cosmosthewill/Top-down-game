using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoliderUltimate : UltimateBase
{
    public int healAmount = 10;
    public override float UltDuration => 5f;

    private float _healTimer;
    public override void Initialize(Transform playerTransform)
    {
        transform.SetParent(playerTransform);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _healTimer += Time.deltaTime;
        if (_healTimer >= 1f)
        {
            PlayerStatsManager.Instance.TakeDmg(-healAmount); // Assuming Player has a Heal method
            _healTimer = 0;
        }
    }
}
