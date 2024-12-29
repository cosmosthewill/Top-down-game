using Script.Player.PowerUpScript.Detail;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script.Player.PowerUpScript
{
    public class UIPowerUpDetail : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Text powerUpName;
        [SerializeField] private Text powerUpDescription;
        [SerializeField] private Image icon;
        private PowerUpDetail powerUpDetail;

        private float lastClickTime;
        private const float DOUBLE_CLICK_TIME = 0.3f;
        public void SetUp(PowerUpDetail powerUpDetail)
        {
            this.powerUpDetail = powerUpDetail;
            powerUpName.text = powerUpDetail.PowerUpName;
            powerUpDescription.text = powerUpDetail.PowerUpDescription;
            icon.sprite = powerUpDetail.Icon;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Time.time - lastClickTime <= DOUBLE_CLICK_TIME)
            {
                // Double-click
                powerUpDetail.SetUpPowerUp();
                GetComponentInParent<LevelUpPopUp>().OnElementClicked();
            }
            lastClickTime = Time.time;
        }
    }
}