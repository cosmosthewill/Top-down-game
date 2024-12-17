using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class UICharacterShopView : MonoBehaviour
    {
        [SerializeField] private CharacterElementDetail[] characters;
        [SerializeField] private Camera renderCamera;
        [SerializeField] private Text characterNameText;
        [SerializeField] private Text characterDescriptionText;
        [SerializeField] private Text characterCostText;
        [SerializeField] private GameObject purchaseButton;
        private int selectedIndex = 0;
        private int coinAmount = 0;
        private bool purchasable = false;
        public void OnRefresh(int coinAmount)
        {
            this.coinAmount = coinAmount;
            SetSelected();
        }

        private void SetSelected()
        {
            renderCamera.transform.position = characters[selectedIndex].transform.position + new Vector3(0, -5f, -10f);
            characterNameText.text = characters[selectedIndex].CharacterName;
            characterDescriptionText.text = characters[selectedIndex].Description;

            if (characters[selectedIndex].HadBought == 1)
            {
                purchaseButton.SetActive(false);
                return;
            }
            
            purchaseButton.SetActive(true);
            characterCostText.text = characters[selectedIndex].Cost.ToString();

            if (characters[selectedIndex].Cost > coinAmount)
            {
                characterCostText.color = Color.red;
                purchasable = false;
            }
            else
            {
                characterCostText.color = Color.white;
                purchasable = true;
            }
        }

        public void OnPurchase()
        {
            if (!purchasable) return;
            
            purchasable = false;
            
            characters[selectedIndex].BuyCharacter();
            
            GetComponentInParent<UIShopPopUp>().OnPurchase(characters[selectedIndex].Cost);
        }

        public void ChangeSelected(int change)
        {
            selectedIndex += change;
            
            if (selectedIndex < 0) selectedIndex += characters.Length;
            if (selectedIndex >= characters.Length) selectedIndex -= characters.Length;
            
            SetSelected();
        }
    }
}