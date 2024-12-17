using UnityEngine;
using UnityEngine.EventSystems;

namespace Script.UI
{
    public class UIMainMenuShopButton : UIMainMenuButton
    {
        [SerializeField] private GameObject shopMenu;
        
        public override void OnPointerClick(PointerEventData eventData)
        {
            shopMenu.SetActive(true);
        }
    }
}