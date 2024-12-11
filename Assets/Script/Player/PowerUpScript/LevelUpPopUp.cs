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
            SoundManager.Instance.PlaySfx(SfxType.LevelUpSound);
            var statPowerUp0 = powerUpManifest.GetRandomStatPowerUp();
            statPowerUp0.Init();
            powerUpDetail[0].SetUp(statPowerUp0);

            PowerUpDetail statPowerUp1;
            do
            {
                statPowerUp1 = powerUpManifest.GetRandomStatPowerUp();
            } while (statPowerUp1 == statPowerUp0);
            statPowerUp1.Init();
            powerUpDetail[1].SetUp(statPowerUp1);

            int remainSlots = WeaponPowerUpManager.Instance.RemainSlot();

            switch (remainSlots)
            {
                case 2: 
                    var weaponPowerUp2 = powerUpManifest.GetRandomWeaponPowerUp();
                    weaponPowerUp2.Init();
                    powerUpDetail[2].SetUp(weaponPowerUp2);
                    break;
                case 1:
                    var weaponPowerUp1 = powerUpManifest.GetRandomWeaponPowerUp();
                    weaponPowerUp1.Init();
                    powerUpDetail[2].SetUp(weaponPowerUp1);
                    break;
                case 0:
                    var lowerLevelWeapon = WeaponPowerUpManager.Instance.GetLowerLevelWeapon();
                    if (lowerLevelWeapon.lvl == 5)
                    {
                        var statPowerUp = powerUpManifest.GetRandomStatPowerUp();
                        statPowerUp.Init();
                        powerUpDetail[2].SetUp(statPowerUp);
                    }
                    else
                    {
                        var weaponPowerUp0 = powerUpManifest.GetPowerUpByName(lowerLevelWeapon.powerUpName);
                        weaponPowerUp0.Init();
                        powerUpDetail[2].SetUp(weaponPowerUp0);
                    }
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