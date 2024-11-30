using System;
using Script.Player.PowerUp.Detail;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script.Player.PowerUp
{
    public class LevelUpPopUp : MonoBehaviour
    {
        [SerializeField] private UIPowerUpDetail[] powerUpDetail;
        [SerializeField] private PowerUpManifest powerUpManifest;
        private void OnEnable()
        {
            Init();
        }

        private void Init()
        {
            for (int i = 0; i < powerUpDetail.Length; i++)
            {
                var detail = powerUpManifest.powerUps[Random.Range(0, powerUpManifest.powerUps.Count)];
                powerUpDetail[i].SetUp(detail);
            }
        }
    }
}