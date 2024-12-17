using System;
using Script.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.UI
{
    public class UIPausePopUp : MonoBehaviour
    {
        [SerializeField] private Text scoreText;
        [SerializeField] private Text coinText;
        [SerializeField] private Text HPText;
        [SerializeField] private Text DamgeText;
        [SerializeField] private Text SpeedText;
        [SerializeField] private Text CritText;

        public void OnEnable()
        {
            scoreText.text = "Current score: " + EnemySpawner.Instance.GetScore().ToString();
            coinText.text = "Collected coins: " + PlayerManager.Instance.GetCoin().ToString();
            HPText.text = PlayerStatsManager.Instance.currentHealth.ToString();
            DamgeText.text = PlayerStatsManager.Instance.damage.ToString();
            SpeedText.text = PlayerStatsManager.Instance.moveSpeed.ToString();
            CritText.text = PlayerStatsManager.Instance.critChance.ToString();
        }

        public void OnPause()
        {
            GamePause.PauseGame();
        }

        public void OnResume()
        {
            gameObject.SetActive(false);
            GamePause.ContinueGame();
        }

        public void OnRestart()
        {
            SceneManager.LoadScene(1);
            GamePause.ContinueGame();
        }

        public void OnBackToMainMenu()
        {
            SceneManager.LoadScene(0);
            GamePause.ContinueGame();
        }
    }
}