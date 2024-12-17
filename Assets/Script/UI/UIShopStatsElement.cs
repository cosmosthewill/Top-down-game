using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script.UI
{
    public class UIShopStatsElement : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private string savedStatsName;
        [SerializeField] private Text descriptionText;
        [SerializeField] private Text costText;
        [SerializeField] private string descriptionBase;
        [SerializeField] private int cost;
        [SerializeField] private float changePerLevel = 5f;
        private int level = -1;
        private bool purchasable = false;
        
        public void SetElement(int coinAmount)
        {
            level = PlayerPrefs.GetInt($"{savedStatsName}_lvl", 0);
            costText.text = cost.ToString();
            descriptionText.text = $"{descriptionBase} {(level + 1) * changePerLevel}%";
            if (cost > coinAmount)
            {
                costText.color = Color.red;
                purchasable = false;
            }
            else
            {
                costText.color = Color.white;
                purchasable = true;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (purchasable == false) return;

            purchasable = false;
            
            PlayerPrefs.SetInt($"{savedStatsName}_lvl", level + 1);
            
            GetComponentInParent<UIShopPopUp>().OnPurchase(cost);
        }
    }
}