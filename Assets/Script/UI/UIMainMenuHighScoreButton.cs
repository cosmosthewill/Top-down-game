using UnityEngine;
using UnityEngine.EventSystems;

namespace Script.UI
{
    public class UIMainMenuHighScoreButton : UIMainMenuButton
    {
        [SerializeField] private GameObject highScoreMenu;
        
        public override void OnPointerClick(PointerEventData eventData)
        {
            highScoreMenu.SetActive(true);
        }
    }
}