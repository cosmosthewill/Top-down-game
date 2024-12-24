using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySfx(SfxType.ButtonHover);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySfx(SfxType.ButtonConfirm);
        }
    }
}
