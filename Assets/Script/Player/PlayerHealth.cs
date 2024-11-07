using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Slider healthSlider;
    // Start is called before the first frame update
    void Start()
    {
        PlayerStatsManager.Instance.currentHealth = PlayerStatsManager.Instance.maxHealth;
        healthSlider.maxValue = PlayerStatsManager.Instance.maxHealth;
        healthSlider.value = PlayerStatsManager.Instance.currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = PlayerStatsManager.Instance.currentHealth;
    }
}
