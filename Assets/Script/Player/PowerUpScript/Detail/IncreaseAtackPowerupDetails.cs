using UnityEngine;

namespace Script.Player.PowerUpScript.Detail
{
    public class IncreaseAtackPowerupDetails : PowerUpDetail
    {
        [SerializeField] private Sprite icon;
        public override Sprite Icon => icon;
        private int[] randPercent = { 5, 10, 15, 20 };
        private int randId = 0;
        public override string PowerUpName => "Increase Attack";

        public override string PowerUpDescription
        {
            get
            {
                return $"Increase the attack by {randPercent[randId]}%";
            }
        }

        public override void Init()
        {
            randId = Random.Range(0, randPercent.Length);
        }

        public override void SetUpPowerUp()
        {
            float increase = PlayerStatsManager.Instance.damage * randPercent[randId] / 100f;
            PlayerStatsManager.Instance.damage += (int) increase;
        }
    }
}