using Script.Player.PowerUpScript.Detail;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseHealthPowerUpDetail : PowerUpDetail
{
    [SerializeField] private Sprite icon;
    public override Sprite Icon => icon;
    private int[] randPercent = { 10, 15, 20, 25 };
    private int randId = 0;
    public override string PowerUpName => "Increase Health";

    public override string PowerUpDescription
    {
        get
        {
            return $"Increase health  by {randPercent[randId]}%";
        }
    }

    public override void Init()
    {
        randId = Random.Range(0, randPercent.Length);
    }

    public override void SetUpPowerUp()
    {
        float increase = PlayerStatsManager.Instance.maxHealth * randPercent[randId] / 100f;
        PlayerStatsManager.Instance.TakeDmg((int) -increase);
    }
}
