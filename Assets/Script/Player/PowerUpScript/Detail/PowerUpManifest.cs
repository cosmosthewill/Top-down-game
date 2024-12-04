using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Player.PowerUpScript.Detail
{
    [CreateAssetMenu(fileName = "New PowerUp Manifest", menuName = "PowerUpManifest")]
    public class PowerUpManifest : ScriptableObject
    {
        private List<PowerUpDetail> statsPowerUps;
        private List<PowerUpDetail> weaponsPowerUps;

        public PowerUpDetail GetRandomStatPowerUp()
        {
            return statsPowerUps[UnityEngine.Random.Range(0, statsPowerUps.Count)];
        }

        public PowerUpDetail GetRandomWeaponPowerUp()
        {
            return weaponsPowerUps[UnityEngine.Random.Range(0, weaponsPowerUps.Count)];
        }
    }

    [Serializable]
    public class PowerUpDetail : MonoBehaviour 
    {
        public virtual string PowerUpName { get; }
        
        public virtual string PowerUpDescription { get; }

        public virtual void Init()
        {
        }

        public virtual void SetUpPowerUp()
        {
        }
    }
}