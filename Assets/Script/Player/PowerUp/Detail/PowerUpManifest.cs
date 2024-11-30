using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Player.PowerUp.Detail
{
    [CreateAssetMenu(fileName = "New PowerUp Manifest", menuName = "PowerUpManifest")]
    public class PowerUpManifest : ScriptableObject
    {
        public List<PowerUpDetail> powerUps;
    }

    [Serializable]
    public class PowerUpDetail : MonoBehaviour 
    {
        public virtual string PowerUpName { get; }

        public virtual void SetUpPowerUp()
        {
            Debug.Log(PowerUpName);
        }
    }
}