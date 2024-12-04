using Script.Player.PowerUp;
using Script.Player.PowerUp.Detail;
using Script.Player.PowerUpScript.Detail;
using UnityEngine;

namespace Script.Player.PowerUpScript
{
    public class LevelUpPopUp : MonoBehaviour
    {
        [SerializeField] private UIPowerUpDetail[] powerUpDetail;
        [SerializeField] private PowerUpManifest powerUpManifest;
        private void OnEnable()
        {
            Init();
        }

        private void Init()
        {
            var statPowerUpDetail = powerUpManifest.GetRandomStatsPowerUp();
            statPowerUpDetail.Init();
            powerUpDetail[0].SetUp(statPowerUpDetail);

            switch (WeaponPowerUpController.Instance.EmptySlot())
            {
                case 2:
                    var weaponPowerUpDetail = powerUpManifest.GetRandomWeaponPowerUp();
                    weaponPowerUpDetail.Init();
                    powerUpDetail[1].SetUp(weaponPowerUpDetail);
                    break;
                case 1:
                    powerUpDetail[1].gameObject.SetActive(false);
                    break;
                case 0:
                    powerUpDetail[1].gameObject.SetActive(false);
                    break;
            }
        }

        public void OnElementClicked()
        {
            gameObject.SetActive(false);
            GamePause.ContinueGame();
        }
    }
}