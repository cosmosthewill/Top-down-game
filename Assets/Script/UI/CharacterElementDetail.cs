using UnityEngine;

namespace Script.UI
{
    public class CharacterElementDetail : MonoBehaviour
    {
        [SerializeField] private string characterName;
        [SerializeField] private int cost;
        //[SerializeField] private string description;
        [SerializeField] private Sprite gunIcon;
        [SerializeField] private Sprite ultIcon;
        [SerializeField] private string gunDescription;
        [SerializeField] private string ultDescription;
        
        public string CharacterName => characterName;
        public int Cost => cost;

        public Sprite GunIcon => gunIcon;
        public string GunDescription => gunDescription;
        public Sprite UltIcon => ultIcon;
        public string UltDescription => ultDescription;

        public int HadBought => PlayerPrefs.GetInt($"{characterName}_available", cost == -1 ? 1 : 0);

        public void BuyCharacter()
        {
            PlayerPrefs.SetInt($"{characterName}_available", 1);
        }
    }
}