using System;
using UnityEngine;

namespace Script.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance;
        [SerializeField] private GameObject[] characters;
        private int collectedCoin = 0;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            LoadCharacter(PlayerPrefs.GetInt("CharacterSelectedIndex"));
        }

        private void LoadCharacter(int index)
        {
            characters[index].SetActive(true);
        }

        public void CollectCoin(int value)
        {
            collectedCoin += value;
        }

        public int GetCoin()
        {
            return collectedCoin;
        }
    }
}