using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMana : MonoBehaviour
{
    public Slider manaSlider;
    // Start is called before the first frame update
    void Start()
    {
        PlayerStatsManager.Instance.currentMana = PlayerStatsManager.Instance.maxMana;
        manaSlider.maxValue = PlayerStatsManager.Instance.maxMana;
        manaSlider.value = PlayerStatsManager.Instance.currentMana;
    }
    // Update is called once per frame
    void Update()
    {
        manaSlider.value = PlayerStatsManager.Instance.currentMana;
    }
}
