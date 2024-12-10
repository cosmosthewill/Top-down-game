using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoliderUltimate : UltimateBase
{
    public override float UltDuration => 5f;

    private float _healTimer;
    public override void Initialize(Transform playerTransform)
    {
        transform.SetParent(playerTransform);
    }
    public int healAmount;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlaySfx(SfxType.SoliderUltimate);
        healAmount = (int)(PlayerStatsManager.Instance.maxHealth * 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        _healTimer += Time.deltaTime;
        if (_healTimer >= 1f)
        {
            PlayerStatsManager.Instance.TakeDmg(-healAmount); 
            _healTimer = 0;
        }
    }
}
