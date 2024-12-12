using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Player.PowerUpScript.Detail
{
    [CreateAssetMenu(fileName = "New PowerUp Manifest", menuName = "PowerUpManifest")]
    public class PowerUpManifest : ScriptableObject
    {
        public List<PowerUpDetail> statsPowerUps;
        public List<PowerUpDetail> weaponsPowerUps;

        public PowerUpDetail GetRandomStatPowerUp()
        {
            return statsPowerUps[UnityEngine.Random.Range(0, statsPowerUps.Count)];
        }

        public PowerUpDetail GetRandomWeaponPowerUp()
        {
            return weaponsPowerUps[UnityEngine.Random.Range(0, weaponsPowerUps.Count)];
        }

        public PowerUpDetail GetPowerUpByName(string powerUpName)
        {
            for (int i = 0; i < statsPowerUps.Count; i++)
            {
                if (powerUpName == statsPowerUps[i].PowerUpName)
                {
                    return statsPowerUps[i];
                }
            }
            
            for (int i = 0; i < weaponsPowerUps.Count; i++)
            {
                if (powerUpName == weaponsPowerUps[i].PowerUpName)
                {
                    return weaponsPowerUps[i];
                }
            }

            return null;
        }
    }

    [Serializable]
    public class PowerUpDetail : MonoBehaviour 
    {
        public virtual string PowerUpName { get; }
        
        public virtual string PowerUpDescription { get; }

        public virtual Sprite Icon { get; }

        public virtual void Init()
        {
        }

        public virtual void SetUpPowerUp()
        {
        }
    }
}