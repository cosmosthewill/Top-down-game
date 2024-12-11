using UnityEngine;

namespace Script.Player.PowerUpScript.Detail
{
    public class LavaPoolPowerUpDetail : PowerUpDetail
    {
        [SerializeField] private PowerUp prefab;
        [SerializeField] private Sprite icon;
        private int nextLevel = 0;
        public override string PowerUpName { get; set; }

        public override Sprite Icon => icon;

        public override string PowerUpDescription
        {
            get
            {
                string description = "";
                switch (nextLevel)
                {
                    case 1:
                        description = "Obtain PowerUp Laval Pool";
;                       break;
                    case 2:
                        description = "Increase Damage";
                        break;
                    case 3:
                        description = "Increase Damage";
                        break;
                    case 4:
                        description = "Increase Damage";
                        break;
                    case 5:
                        description = "Add Slow Effect";
                        break;
                }
                return description;
            }
        }

        public override void Init()
        {
            var powerUp = WeaponPowerUpManager.Instance.GetWeaponPowerUp(typeof(LavaPool));
            if (powerUp == null)
            {
                PowerUpName = "Lava Pool";
                nextLevel = 1;
            }
            else
            {
                PowerUpName = "Lava Pool Level Up";
                nextLevel = powerUp.lvl + 1;
            }
        }

        public override void SetUpPowerUp()
        {
            switch (nextLevel)
            {
                case 1:
                    WeaponPowerUpManager.Instance.AddWeapon(prefab);
                    break;
                default:
                    WeaponPowerUpManager.Instance.UpdateWeapon(typeof(LavaPool));
                    break;
            }
        }
    }
}