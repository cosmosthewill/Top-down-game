using UnityEngine;

namespace Script.Player.PowerUpScript.Detail
{
    public class BombPowerUpDetail : PowerUpDetail
    {
        [SerializeField] private PowerUp prefab;
        private int nextLevel = 0;
        public override string PowerUpName => "Bomb";

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
                        description = "Level 2";
                        break;
                    case 3:
                        description = "Level 3";
                        break;
                    case 4:
                        description = "Level 4";
                        break;
                    case 5:
                        description = "Level 5";
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
                nextLevel = 1;
            }
            else
            {
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