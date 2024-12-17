using System.Collections;
using System.Collections.Generic;
using Script;
using Script.UI;
using UnityEngine;
using TMPro;

public class StatUI : MonoBehaviour
{
    [SerializeField] private UIPausePopUp pausePopUp;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GamePause.isPaused) 
            {
                pausePopUp.OnResume();
            }
            else
            {
                pausePopUp.gameObject.SetActive(true);
                pausePopUp.OnPause();
            }
            
        }
    }
}
