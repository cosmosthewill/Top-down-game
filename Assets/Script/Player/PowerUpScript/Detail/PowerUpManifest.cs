using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script.Player.PowerUpScript.Detail
{
    [CreateAssetMenu(fileName = "New PowerUp Manifest", menuName = "PowerUpManifest")]
    public class PowerUpManifest : ScriptableObject
    {
        public List<PowerUpDetail> statsPowerUps;
        public List<PowerUpDetail> weaponsPowerUps;

        public PowerUpDetail GetRandomStatsPowerUp()
        {
            return statsPowerUps[Random.Range(0, statsPowerUps.Count)];
        }
        
        public PowerUpDetail GetRandomWeaponPowerUp()
        {
            return weaponsPowerUps[Random.Range(0, weaponsPowerUps.Count)];
        }
    }

    [Serializable]
    public class PowerUpDetail : MonoBehaviour 
    {
        public virtual string PowerUpName { get; }

        public virtual void Init()
        {
        }
        
        public virtual string PowerUpDescription { get; }

        public virtual void SetUpPowerUp()
        {
        }
    }
}