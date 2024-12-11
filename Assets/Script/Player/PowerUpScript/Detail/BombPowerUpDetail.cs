using UnityEngine;

namespace Script.Player.PowerUpScript.Detail
{
    public class BombPowerUpDetail : PowerUpDetail
    {
        [SerializeField] private PowerUp prefab;
        [SerializeField] private Sprite icon;
        public override Sprite Icon => icon;
        private int nextLevel = 0;
        public override string PowerUpName { get; set; }

        public override string PowerUpDescription
        {
            get
            {
                string description = "";
                switch (nextLevel)
                {
                    case 1:
                        description = "Unlock Bomb";
                        break;
                    case 2:
                        description = "Increase Damage and Explosion Radious";
                        break;
                    case 3:
                        description = "Increase Damage and Explosion Radious";
                        break;
                    case 4:
                        description = "Increase Damage and Explosion Radious";
                        break;
                    case 5:
                        description = "Add Knockback Effect";
                        break;
                }
                return description;
            }
        }

        public override void Init()
        {
            var powerUp = WeaponPowerUpManager.Instance.GetWeaponPowerUp(typeof(Bomb));
            if (powerUp == null)
            {
                PowerUpName = "Bomb";
                nextLevel = 1;
            }
            else
            {
                PowerUpName = "Bomb Level Up";
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
                    WeaponPowerUpManager.Instance.UpdateWeapon(typeof(Bomb));
                    break;
            }
        } 
    }
}