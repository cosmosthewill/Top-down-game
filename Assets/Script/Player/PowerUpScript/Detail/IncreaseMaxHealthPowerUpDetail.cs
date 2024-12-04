using Script.Player.PowerUpScript.Detail;
using UnityEngine;

namespace Script.Player.PowerUp.Detail
{
    public class IncreaseMaxHealthPowerUpDetail : PowerUpDetail
    {
        private int[] increasePercent = { 10, 15, 20, 25 };
        private int randPercentId = 0;
        
        public override string PowerUpName => "Increase Max Health";

        public override void Init()
        {
            randPercentId = Random.Range(0, increasePercent.Length);
        }
        public override string PowerUpDescription
        {
            get => $"Increase the max health by {increasePercent[randPercentId]}%";
        }

        public override void SetUpPowerUp()
        {
            float increase = PlayerStatsManager.Instance.maxHealth * (increasePercent[randPercentId] / 100f);
            PlayerStatsManager.Instance.maxHealth += increase;
            PlayerStatsManager.Instance.currentHealth += increase;
        }
    }
}