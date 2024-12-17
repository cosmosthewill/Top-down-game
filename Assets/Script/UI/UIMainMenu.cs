using UnityEngine;
using UnityEngine.EventSystems;

namespace Script.UI
{
    public class UIMainMenu : MonoBehaviour
    {
        
    }

    public class UIMainMenuButton : MonoBehaviour, IPointerClickHandler
    {
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Button Clicked");
        }
    }
}