using UnityEngine;

namespace Script.Player.PowerUpScript.Detail
{
    public class IncreaseSPDPowerupDetails : PowerUpDetail
    {
        [SerializeField] private Sprite icon;
        public override Sprite Icon => icon;
        private int[] randPercent = { 5, 10};
        private int randId = 0;
        public override string PowerUpName => "Increase Speed";

        public override string PowerUpDescription
        {
            get
            {
                return $"Increase the speed by {randPercent[randId]}%";
            }
        }

        public override void Init()
        {
            randId = Random.Range(0, randPercent.Length);
        }

        public override void SetUpPowerUp()
        {
            float increase = PlayerStatsManager.Instance.moveSpeed * randPercent[randId] / 100f;
            PlayerStatsManager.Instance.moveSpeed += increase;
        }
    }
}