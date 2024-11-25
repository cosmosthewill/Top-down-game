using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Runtime.Script.Character
{
    [CreateAssetMenu(menuName = "Character Manifest", fileName = "CharacterManifest")]
    public class CharacterManifest : ScriptableObject
    {
        #region Singleton

        private const string Path = "Assets/Runtime/ScriptableObject/CharacterManifest.asset";
        
        private static CharacterManifest _instance;

        public static CharacterManifest Instance
        {
            get
            {
                if (_instance == null)
                    _instance = Addressables.LoadAssetAsync<CharacterManifest>(Path).WaitForCompletion();
                
                _instance.LoadCharacters();
                return _instance; 
            }
        }

        #endregion
        
        public GameObject[] charactersData;
        private Dictionary<string, GameObject> characterPrefabs = new Dictionary<string, GameObject>();

        private void LoadCharacters()
        {
            foreach (var character in charactersData)
            {
                characterPrefabs.Add(character.name, character);
            }
        }

        public GameObject GetCharacters(string characterName)
        {
            if (characterPrefabs.TryGetValue(characterName, out GameObject characterPrefab))
            {
                return characterPrefab;
            }
            Debug.Log("Character Not Found");
            return characterPrefabs[characterPrefabs.Keys.First()];
        }
    }
}