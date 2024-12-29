using UnityEngine;

namespace Script.Player.PowerUpScript.Detail
{
    public class IncreaseMaxHealthPowerUpDetail : PowerUpDetail
    {
        [SerializeField] private Sprite icon;
        public override Sprite Icon => icon;
        private int[] randPercent = { 10, 15, 20, 25 };
        private int randId = 0;
        public override string PowerUpName => "Increase Max Health";

        public override string PowerUpDescription
        {
            get
            {
                return $"Increase the max health by {randPercent[randId]}%";
            }
        }

        public override void Init()
        {
            randId = Random.Range(0, randPercent.Length);
        }

        public override void SetUpPowerUp()
        {
            float increase = PlayerStatsManager.Instance.maxHealth * randPercent[randId] / 100f;
            PlayerStatsManager.Instance.maxHealth += increase;
        }
    }
}