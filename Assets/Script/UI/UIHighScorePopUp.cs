using System;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class UIHighScorePopUp : MonoBehaviour
    {
        [SerializeField] private Text[] highScoreTexts;
        
        private void OnEnable()
        {
            for (int i = 0; i < 9; i++)
            {
                int score = PlayerPrefs.GetInt($"HighScore_{i}", 0);
                highScoreTexts[i].text = $"No{i + 1}.          {score}";
            }
        }

        public void OnClose()
        {
            gameObject.SetActive(false);
        }
    }
}