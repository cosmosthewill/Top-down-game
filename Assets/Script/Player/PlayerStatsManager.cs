using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager Instance;
    [SerializeField] private GameObject gameOverPopUp;

    [Header("Combat Stats")]
    public int damage;
    public float weaponRange;
    public float stunTime;
    public float critChance;
    public int critMultiplier;

    [Header("Movement Stats")]
    public float moveSpeed;
    public float rollBoost;
    public float rollDuration;

    [Header("Health Stats")]
    public float currentHealth;
    public float maxHealth;
    public float currentMana;
    public float maxMana;

    [Header("OtherStats")]
    public float basePickUpRange;
    public float baseCoinDropRate;
    public float baseExpDropRate;
    public float baseFoodDropRate;

    [Header("BuyStats")]
    public float bonusHealth;
    public float bonusSpd;
    public float bonusAttack;
    public float bonusCrit;
    public float bonusPickUpRange;
    public float bonusExpDrop;
    public float bonusFoodRate;
    public float bonusCoinDropRate;
    public float bonusCoinAmount;
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else Destroy(gameObject);
    }
    public void GetBonus()
    {
        bonusHealth = (PlayerPrefs.GetInt("BaseHealth_lvl", 0) * 5) / 100f;
        bonusSpd = (PlayerPrefs.GetInt("BaseSpeed_lvl", 0) * 3) / 100f;
        bonusAttack = (PlayerPrefs.GetInt("BaseAttack_lvl", 0) * 5) / 100f;
        bonusCrit = (PlayerPrefs.GetInt("BaseCrit_lvl", 0) * 2) / 100f;
        bonusPickUpRange = (PlayerPrefs.GetInt("PickUpRange_lvl", 0) * 10) / 100f;
        bonusExpDrop = (PlayerPrefs.GetInt("BonusExp_lvl", 0) * 5) / 100f;
        bonusFoodRate = (PlayerPrefs.GetInt("FoodDropRate_lvl", 0) * 2) / 100f;
        bonusCoinDropRate = (PlayerPrefs.GetInt("CoinDropRate_lvl", 0) * 5) / 100f;
        bonusCoinAmount= (PlayerPrefs.GetInt("CoinAmountBonus_lvl", 0) * 5) / 100f;
    }
    private void Start()
    {
        GetBonus();
        //
        maxHealth *= (1 + bonusHealth);
        moveSpeed *= (1 + bonusSpd);
        damage += (int)(damage * (1 + bonusAttack));
        critChance += bonusCrit;

        basePickUpRange *= (1 + bonusPickUpRange);
        baseExpDropRate *= (1 + bonusExpDrop);
        baseFoodDropRate += bonusFoodRate;
        baseCoinDropRate += bonusCoinDropRate;
        //

        currentMana = maxMana;
        currentHealth = maxHealth;
    }
    public void TakeDmg(int amount)
    {
        if (amount > 0) SoundManager.Instance.PlaySfx(SfxType.Hurt);
        currentHealth -= amount;
        if (currentHealth < 0)
        {
            Destroy(gameObject);
            GamePause.PauseGame();
            gameOverPopUp.SetActive(true);
        }
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }
    public void GainMana(int amount)
    {
        currentMana += amount;
        if (currentMana > maxMana) currentMana = maxMana;
    }
    public void AddSpeed(float amount)
    {
        moveSpeed += amount;
        moveSpeed = Mathf.Min(moveSpeed, 100f);
    }
}
