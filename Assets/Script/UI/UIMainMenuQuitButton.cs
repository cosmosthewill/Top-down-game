using UnityEngine;
using UnityEngine.EventSystems;

namespace Script.UI
{
    public class UIMainMenuQuitButton : UIMainMenuButton
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            Application.Quit();
        }
    }
}