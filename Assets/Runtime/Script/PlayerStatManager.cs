using Runtime.Script.Character;
using Runtime.Script.UI;
using Runtime.Script.Weapon;
using UnityEditor;
using UnityEngine;

namespace Runtime.Script
{
    public class PlayerStatManager : MonoBehaviour
    {
        [SerializeField] private PlayerStatBar playerStatBar;
        
        #region Character

        private float currentMaxHealth;
        private float currentHealth;

        private float currentMaxMana;
        private float currentMana;
        
        private float currentSpeed;
        
        public float CurrentMaxHealth => currentMaxHealth;
        public float CurrentHealth => currentHealth;
        public float CurrentMaxMana => currentMaxMana;
        public float CurrentMana => currentMana;
        public float CurrentSpeed => currentSpeed;

        #endregion

        #region Weapon

        private float currentDamage;
        private float currentFireCd;
        private float currentBulletSpeed;
        
        public float CurrentDamage => currentDamage;
        public float CurrentFireCd => currentFireCd;
        public float CurrentBulletSpeed => currentBulletSpeed;

        #endregion

        #region Level

        private int level;
        private int expToNextLevel;
        private int currentExp;
        
        public int Level => level;
        public int ExpToNextLevel => expToNextLevel;
        public int CurrentExp => currentExp;

        #endregion
        
        public void LoadStats(BaseCharacter character, BaseWeapon weapon)
        {
            currentMaxHealth = currentHealth = character.BaseHealth;
            currentMaxMana = currentMana = character.BaseMana;
            currentSpeed = character.BaseSpeed;
            
            currentDamage = weapon.BaseDamage;
            currentFireCd = weapon.BaseFireCd;
            currentBulletSpeed = weapon.BaseBulletSpeed;

            level = 1;
            expToNextLevel = 500;
            currentExp = 0;
            
            ResetStatsUI();
        }

        public void ChangeExp(int addExp)
        {
            currentExp += addExp;
            if (currentExp >= expToNextLevel)
            {
                currentExp = 0;
                level ++;
            }
            
            ResetStatsUI();
        }

        public void ChangeHealth(float addHealth)
        {
            currentHealth += addHealth;
            currentHealth = Mathf.Clamp(currentHealth, 0, currentMaxHealth);
            ResetStatsUI();
        }

        public void ChangeMana(float addMana)
        {
            currentMana += addMana;
            currentMana = Mathf.Clamp(currentMana, 0, currentMaxMana);
            ResetStatsUI();
        }

        private void ResetStatsUI()
        {
            playerStatBar.LoadPlayerStats(CurrentExp, ExpToNextLevel, Level, CurrentHealth, CurrentMaxHealth, CurrentMana, CurrentMaxMana);
        }
    }
}