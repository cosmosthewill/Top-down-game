using System;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class UIShopPopUp : MonoBehaviour
    {
        [SerializeField] private Text coinAmountText;
        [SerializeField] private UICharacterShopView characterShopView;
        [SerializeField] private UIStatsShopView statsShopView;
        private int coinAmount;
        private void OnEnable()
        {
            coinAmount = PlayerPrefs.GetInt("CoinAmount", 10000);
            coinAmountText.text = $"Coins: {coinAmount}";
            OpenCharacterShopView();
        }

        public void OnPurchase(int cost)
        {
            coinAmount -= cost;
            PlayerPrefs.SetInt("CoinAmount", coinAmount);
            coinAmountText.text = $"Coins: {coinAmount}";
            RefreshShop();
        }

        public void OpenCharacterShopView()
        {
            characterShopView.gameObject.SetActive(true);
            statsShopView.gameObject.SetActive(false);
            RefreshShop();
        }
        
        public void OpenStatsShopView()
        {
            characterShopView.gameObject.SetActive(false);
            statsShopView.gameObject.SetActive(true);
            RefreshShop();
        }

        public void RefreshShop()
        {
            characterShopView.OnRefresh(coinAmount);
            statsShopView.OnRefresh(coinAmount);
        }

        public void OnClose()
        {
            gameObject.SetActive(false);
        }
    }
}