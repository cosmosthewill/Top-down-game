using UnityEngine;
using UnityEngine.EventSystems;

namespace Script.UI
{
    public class UIMainMenuSettingButton : UIMainMenuButton
    {
        [SerializeField] private GameObject settingMenu;
        
        public override void OnPointerClick(PointerEventData eventData)
        {
            settingMenu.SetActive(true);
        }
    }
}