using System;
using Script.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.UI
{
    public class UIGameOverPopUp : MonoBehaviour
    {
        [SerializeField] private Text scoreText;
        [SerializeField] private Text coinText;

        private void OnEnable()
        {
            scoreText.text = "Your score: " + EnemySpawner.Instance.GetScore().ToString();
            coinText.text = "Collected coins: " + PlayerManager.Instance.GetCoin().ToString();

            SaveCoin();
            SaveHighScore();
            SoundManager.Instance.PlayBackgroundMusic(BGMType.VictoryTheme);
        }

        private void SaveCoin()
        {
            int currenCoin = PlayerPrefs.GetInt("CoinAmount", 0);
            currenCoin += PlayerManager.Instance.GetCoin();
            PlayerPrefs.SetInt("CoinAmount", currenCoin);
        }

        private void SaveHighScore()
        {
            int[] highScore = new int[11];
            for (int i = 0; i < 10; i++)
            {
                int score = PlayerPrefs.GetInt($"HighScore_{i}", 0);
                highScore[i] = score;
            }

            highScore[10] = EnemySpawner.Instance.GetScore();
            for (int i = 10; i > 0; i--)
            {
                if (highScore[i] > highScore[i - 1])
                {
                    (highScore[i], highScore[i - 1]) = (highScore[i - 1], highScore[i]);
                }
            }
            for (int i = 0; i < 10; i++)
            {
                PlayerPrefs.SetInt($"HighScore_{i}", highScore[i]);
            }
        }

        public void OnRetry()
        {
            SceneManager.LoadScene(1);
            GamePause.ContinueGame();
            SoundManager.Instance.PlayBackgroundMusic(BGMType.GamePlay);
        }

        public void OnBackToMainMenu()
        {
            SceneManager.LoadScene(0);
            GamePause.ContinueGame();
            SoundManager.Instance.PlayBackgroundMusic(BGMType.MainMenu);
        }
    }
}