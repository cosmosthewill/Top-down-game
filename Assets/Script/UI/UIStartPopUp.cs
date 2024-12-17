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
        [SerializeField] private Text characterDescriptionText;
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
            characterDescriptionText.text = characters[selectedIndex].Description;

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
            SceneManager.LoadScene(1);
        }

        public void OnClose()
        {
            gameObject.SetActive(false);
        }
    }
}