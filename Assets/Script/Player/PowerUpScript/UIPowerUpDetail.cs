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

        public void SetUp(PowerUpDetail powerUpDetail)
        {
            this.powerUpDetail = powerUpDetail;
            powerUpName.text = powerUpDetail.PowerUpName;
            powerUpDescription.text = powerUpDetail.PowerUpDescription;
            icon.sprite = powerUpDetail.Icon;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            powerUpDetail.SetUpPowerUp();
            GetComponentInParent<LevelUpPopUp>().OnElementClicked();
        }
    }
}