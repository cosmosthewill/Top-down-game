using UnityEngine;

namespace Script
{
    public class GamePause : MonoBehaviour
    {
        public static bool isPaused = false;

        public static void PauseGame()
        {
            isPaused = true;
            Time.timeScale = 0;
        }

        public static void ContinueGame()
        {
            isPaused = false;
            Time.timeScale = 1;
        }
    }
}