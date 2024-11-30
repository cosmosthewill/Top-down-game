using System.Collections;
using System.Collections.Generic;
using Script;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExpBar : MonoBehaviour
{
    [SerializeField] private GameObject levelUpPopUp;
    public static PlayerExpBar instance;
    public float maxExp;
    public float currentExp;
    public Image expBar;

    //test
    public float expGainPerSec = 5f;
    public int playerLvl;
    public TextMeshProUGUI lvlText;
    private void Awake()
    {
        instance = this;
    }
    public void GainExp(float amount)
    {
        currentExp += amount;
    }
    // Start is called before the first frame update
    void Start()
    {
        playerLvl = 1;
        maxExp = 10;
        currentExp = 0;
        lvlText.text = "Lv: " + playerLvl.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //currentExp += expGainPerSec * Time.deltaTime;
        expBar.fillAmount = currentExp/maxExp;

        if(currentExp >= maxExp)
        {
            playerLvl++;
            currentExp = 0;
            maxExp *= 2;
            lvlText.text = "Lv: " + playerLvl.ToString();
            GamePause.PauseGame();
            levelUpPopUp.SetActive(true);
        }

    }
}
