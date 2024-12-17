using System;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class UISettingPopUp : MonoBehaviour
    {
        [SerializeField] private Text backgroundMusicText, soundText;
        private int backgroundMusicVolume, soundVolume;
        
        private void OnEnable()
        {
            backgroundMusicVolume = PlayerPrefs.GetInt("MusicVolume", 10);
            backgroundMusicText.text = backgroundMusicVolume.ToString();
            soundVolume = PlayerPrefs.GetInt("SoundVolume", 10);
            soundText.text = soundVolume.ToString();
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

        public void OnApply()
        {
            PlayerPrefs.SetInt("MusicVolume", backgroundMusicVolume);
            PlayerPrefs.SetInt("SoundVolume", soundVolume);
            OnReturn();
        }

        public void OnReturn()
        {
            gameObject.SetActive(false);
        }
    }
}