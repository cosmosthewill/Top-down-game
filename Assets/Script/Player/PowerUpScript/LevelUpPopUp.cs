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
            var statPowerUp = powerUpManifest.GetRandomStatPowerUp();
            statPowerUp.Init();
            powerUpDetail[0].SetUp(statPowerUp);
        }

        public void OnElementClicked()
        {
            gameObject.SetActive(false);
        }
    }
}