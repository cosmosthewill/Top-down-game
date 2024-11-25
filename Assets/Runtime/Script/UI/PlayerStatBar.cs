using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Script.UI
{
    public class PlayerStatBar : MonoBehaviour
    {
        [SerializeField] private Image expBar;
        [SerializeField] private Text levelText;
        [SerializeField] private Image healthBar;
        [SerializeField] private Image manaBar;

        public void LoadPlayerStats(int currentExp, int expToNextLevel, int level, float currentHealth, float currentMaxHealth, float currentMana, float currentMaxMana)
        {
            expBar.fillAmount = currentExp / (float)expToNextLevel;
            levelText.text = $"Lv{level}";
            healthBar.fillAmount = currentHealth / currentMaxHealth;
            manaBar.fillAmount = currentMana / currentMaxMana;
        }
    }
}