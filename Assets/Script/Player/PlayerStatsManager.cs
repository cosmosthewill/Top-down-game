using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager Instance;

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

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else Destroy(gameObject);
    }
    private void Start()
    {
        currentMana = maxMana;
        currentHealth = maxHealth;
    }
    public void TakeDmg(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0)
        {
            Destroy(gameObject);
            Time.timeScale = 0;
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
    }
}
