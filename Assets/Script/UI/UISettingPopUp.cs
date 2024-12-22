using System;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class UISettingPopUp : MonoBehaviour
    {
        [SerializeField] private Text backgroundMusicText, soundText;
        private int backgroundMusicVolume, soundVolume;
        [SerializeField] private Toggle popupDmg;
        private int popup;
        private void OnEnable()
        {
            backgroundMusicVolume = PlayerPrefs.GetInt("MusicVolume", 10);
            backgroundMusicText.text = backgroundMusicVolume.ToString();
            soundVolume = PlayerPrefs.GetInt("SoundVolume", 10);
            soundText.text = soundVolume.ToString();
            popup = PlayerPrefs.GetInt("PopUpDmg", 1);
            popupDmg.isOn = popup == 1;
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
            OnReturn();
        }

        public void OnReturn()
        {
            gameObject.SetActive(false);
        }
    }
}