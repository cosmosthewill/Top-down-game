using UnityEngine;

namespace Runtime.Script
{
    public static class GameManager
    {
        public static bool isPaused = false;
        
        public static void PauseGame()
        {
            Time.timeScale = 0;
            isPaused = true;
        }

        public static void ResumeGame()
        {
            Time.timeScale = 1;
            isPaused = false;
        }
    }
}