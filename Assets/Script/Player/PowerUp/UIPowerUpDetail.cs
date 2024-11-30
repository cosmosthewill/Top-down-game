using Script.Player.PowerUp.Detail;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script.Player.PowerUp
{
    public class UIPowerUpDetail : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Text powerUpName;
        private PowerUpDetail powerUpDetail;

        public void SetUp(PowerUpDetail powerUpDetail)
        {
            this.powerUpDetail = powerUpDetail;
            powerUpName.text = powerUpDetail.PowerUpName;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Clicked");
            powerUpDetail.SetUpPowerUp();
        }
    }
}