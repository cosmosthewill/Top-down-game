using UnityEngine;
using UnityEngine.EventSystems;

namespace Script.UI
{
    public class UIMainMenuStartButton : UIMainMenuButton
    {
        [SerializeField] private GameObject startMenu;
        
        public override void OnPointerClick(PointerEventData eventData)
        {
            startMenu.SetActive(true);
        }
    }
}