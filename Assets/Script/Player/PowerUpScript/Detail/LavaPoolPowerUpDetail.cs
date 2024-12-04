using Script.Player.PowerUp.Detail;
using UnityEngine;

namespace Script.Player.PowerUpScript.Detail
{
    public class LavaPoolPowerUpDetail : PowerUpDetail
    {
        [SerializeField] private PowerUp prefab;
        
        private int weaponLevel = 0;
        public override string PowerUpName => "Lava Pool";

        public override string PowerUpDescription
        {
            get
            {
                string powerUpDescription = "";
                switch (weaponLevel)
                {
                    case 1:
                        powerUpDescription = "Description for level 1";
                        break;
                    case 2:
                        powerUpDescription = "Description for level 2";
                        break;
                    case 3:
                        powerUpDescription = "Description for level 3";
                        break;
                    case 4:
                        powerUpDescription = "Description for level 4";
                        break;
                }

                return powerUpDescription;
            }
        }

        public override void Init()
        {
            var weapon = WeaponPowerUpController.Instance.ContainsWeapon(typeof(LavaPool));
            if (weapon == null)
            {
                weaponLevel = 1;
            }
            else
            {
                weaponLevel = weapon.lvl + 1;
            }
        }

        public override void SetUpPowerUp()
        {
            switch (weaponLevel)
            {
                case 1:
                    WeaponPowerUpController.Instance.AddWeapon(prefab, prefab.gameObject);
                    break;
                default:
                    WeaponPowerUpController.Instance.UpdateWeapon(typeof(LavaPool));
                    break;
            }
        }
    }
}