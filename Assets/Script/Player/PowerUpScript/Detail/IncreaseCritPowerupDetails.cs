using UnityEngine;

namespace Script.Player.PowerUpScript.Detail
{
    public class IncreaseCritPowerupDetails : PowerUpDetail
    {
        [SerializeField] private Sprite icon;
        public override Sprite Icon => icon;
        private int[] randPercent = { 2, 4, 6};
        private int randId = 0;
        public override string PowerUpName => "Increase Crit Chance";

        public override string PowerUpDescription
        {
            get
            {
                return $"Increase the crit chance by {randPercent[randId]}%";
            }
        }

        public override void Init()
        {
            randId = Random.Range(0, randPercent.Length);
        }

        public override void SetUpPowerUp()
        {
            float increase =  randPercent[randId] / 100f;
            PlayerStatsManager.Instance.critChance += increase;
        }
    }
}