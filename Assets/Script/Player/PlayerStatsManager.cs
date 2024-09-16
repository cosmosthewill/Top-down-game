using System.Collections;
using System.Collections.Generic;
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

    
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else Destroy(gameObject);
    }
}
