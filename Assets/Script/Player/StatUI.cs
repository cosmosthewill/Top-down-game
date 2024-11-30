using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatUI : MonoBehaviour
{
    public UnityEngine.GameObject[] statsSlots;
    // 0: Hp
    // 1: ATK
    // 2: SPD
    // 3: CRIT

    private CanvasGroup canvasGroup;
    private bool isStatOpen = false;
    public void updateHp()
    {
        statsSlots[0].transform.GetChild(0).GetComponentInChildren<TMP_Text>().text 
            = "HP " + PlayerStatsManager.Instance.currentHealth;
    }
    public void updateAtk()
    {
        statsSlots[1].transform.GetChild(0).GetComponentInChildren<TMP_Text>().text
            = "ATK " + PlayerStatsManager.Instance.damage;
    }
    public void updateSpd()
    {
        statsSlots[2].transform.GetChild(0).GetComponentInChildren<TMP_Text>().text
            = "Spd " + PlayerStatsManager.Instance.moveSpeed;
    }
    public void updateCrit()
    {
        statsSlots[3].transform.GetChild(0).GetComponentInChildren<TMP_Text>().text
            = "CRIT " + PlayerStatsManager.Instance.critChance * 100 + "%";
    }
    void updateAllStats()
    {
        updateHp();
        updateAtk();
        updateCrit();
        updateSpd();
    }
    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isStatOpen) 
            {
                Time.timeScale = 1;
                canvasGroup.alpha = 0;
                isStatOpen = false;

            }
            else
            {
                Time.timeScale = 0;
                canvasGroup.alpha = 1;
                isStatOpen = true;
            }
            
        }
        updateAllStats();
    }
}
