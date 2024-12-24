using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.UI
{
    public class UIStartPopUp : MonoBehaviour
    {
        [SerializeField] private CharacterElementDetail[] characters;
        [SerializeField] private Camera renderCamera;
        [SerializeField] private Text characterNameText;
        [SerializeField] private Image characterGunIcon;
        [SerializeField] private Text characterGunText;
        [SerializeField] private Image characterUltIcon;
        [SerializeField] private Text characterUltext;
        [SerializeField] private GameObject playButton;
        private int selectedIndex = 0;
        private void OnEnable()
        {
            selectedIndex = 0;
            SetSelected();
        }

        private void SetSelected()
        {
            renderCamera.transform.position = characters[selectedIndex].transform.position + new Vector3(0, -5f, -10f);
            characterNameText.text = characters[selectedIndex].CharacterName;
            characterGunIcon.preserveAspect = true;
            characterGunIcon.sprite = characters[selectedIndex].GunIcon;
            characterGunText.text = characters[selectedIndex].GunDescription;
            characterUltIcon.preserveAspect = true;
            characterUltIcon.sprite = characters[selectedIndex].UltIcon;
            characterUltext.text = characters[selectedIndex].UltDescription;

            playButton.SetActive(characters[selectedIndex].HadBought == 1);
        }

        public void ChangeSelected(int change)
        {
            selectedIndex += change;
            if (selectedIndex < 0) selectedIndex += characters.Length;
            if (selectedIndex >= characters.Length) selectedIndex -= characters.Length;
            SetSelected();
        }

        public void OnStartPlay()
        {
            PlayerPrefs.SetInt("CharacterSelectedIndex", selectedIndex);
            SceneManager.LoadScene(1);
            SoundManager.Instance.PlayBackgroundMusic(BGMType.GamePlay);
        }

        public void OnClose()
        {
            gameObject.SetActive(false);
        }
    }
}