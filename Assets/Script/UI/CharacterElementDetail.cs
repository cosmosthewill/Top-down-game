using UnityEngine;

namespace Script.UI
{
    public class CharacterElementDetail : MonoBehaviour
    {
        [SerializeField] private string characterName;
        [SerializeField] private int cost;
        [SerializeField] private string description;
        
        public string CharacterName => characterName;
        public int Cost => cost;
        public string Description => description;
        
        public int HadBought => PlayerPrefs.GetInt($"{characterName}_available", cost == -1 ? 1 : 0);

        public void BuyCharacter()
        {
            PlayerPrefs.SetInt($"{characterName}_available", 1);
        }
    }
}