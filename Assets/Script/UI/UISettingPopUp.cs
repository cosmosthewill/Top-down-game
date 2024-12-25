using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class UISettingPopUp : MonoBehaviour
    {
        [SerializeField] private Text backgroundMusicText, soundText;
        private int backgroundMusicVolume, soundVolume;
        [SerializeField] private Toggle popupDmg;
        [SerializeField] private Toggle fullscreenTog;
        private int popup;
        public List<ResItem> resolution = new List<ResItem>();
        private int selectedResIndex;
        public Text resolutionLabel;
        private void OnEnable()
        {
            backgroundMusicVolume = PlayerPrefs.GetInt("MusicVolume", 10);
            backgroundMusicText.text = backgroundMusicVolume.ToString();
            soundVolume = PlayerPrefs.GetInt("SoundVolume", 10);
            soundText.text = soundVolume.ToString();
            popup = PlayerPrefs.GetInt("PopUpDmg", 1);
            popupDmg.isOn = popup == 1;
            fullscreenTog.isOn = Screen.fullScreen;
            bool foundRes = false;
            for (int i = 0; i < resolution.Count; i++) 
            {
                if (Screen.width == resolution[i].horizontal && Screen.height == resolution[i].vertical)
                {
                    foundRes = true;
                    selectedResIndex = i;
                    UpdateResLabel();
                }
            }
            if (!foundRes)
            {
                ResItem newRes = new ResItem();
                newRes.horizontal = Screen.width;
                newRes.vertical = Screen.height;

                resolution.Add(newRes);
                selectedResIndex = resolution.Count - 1;
                UpdateResLabel();
            }
        }

        public void OnChangeMusicChange(int change)
        {
            backgroundMusicVolume += change;
            backgroundMusicVolume = Mathf.Clamp(backgroundMusicVolume, 0, 20);
            backgroundMusicText.text = backgroundMusicVolume.ToString();
        }

        public void OnChangeSoundChange(int change)
        {
            soundVolume += change;
            soundVolume = Mathf.Clamp(soundVolume, 0, 20);
            soundText.text = soundVolume.ToString();
        }
        public void OnChangePopupDamage()
        {
            if (popupDmg.isOn) popup = 1;
            else popup = 0;
        }
        public void OnApply()
        {
            OnChangePopupDamage();
            PlayerPrefs.SetInt("MusicVolume", backgroundMusicVolume);
            PlayerPrefs.SetInt("SoundVolume", soundVolume);
            PlayerPrefs.SetInt("PopUpDmg", popup);
            SoundManager.Instance.UpdateVolume();
            Screen.SetResolution(resolution[selectedResIndex].horizontal, resolution[selectedResIndex].vertical,fullscreenTog.isOn);
            OnReturn();
        }

        public void OnReturn()
        {
            gameObject.SetActive(false);
        }
        public void ResChange(int change)
        {
            selectedResIndex += change;
            if (selectedResIndex < 0) selectedResIndex = 0;
            if (selectedResIndex > resolution.Count - 1) selectedResIndex = resolution.Count - 1;
            UpdateResLabel();
        }
        public void UpdateResLabel()
        {
            resolutionLabel.text = resolution[selectedResIndex].horizontal.ToString() + "x" + resolution[selectedResIndex].vertical.ToString(); 
        }
    }
}
[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;
}