using System;
using System.Collections;
using UnityEngine;

namespace Script.Player.PowerUpScript
{
    public class WeaponPowerUpController : MonoBehaviour
    {
        public static WeaponPowerUpController Instance;
    
        private PowerUp slot1 = null;
        private GameObject slot1Prefab = null;
        private PowerUp slot2 = null;
        private GameObject slot2Prefab = null;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public int EmptySlot()
        {
            int cnt = 0;
            if (slot1 == null) cnt++;
            if (slot2 == null) cnt++;
            return cnt;
        }

        public PowerUp ContainsWeapon(Type powerUpType)
        {
            if (slot1 != null && slot1.GetType() == powerUpType)
            {
                return slot1;
            }
            if (slot2 != null && slot2.GetType() == powerUpType)
            {
                return slot2;
            }

            return null;
        }

        public void AddWeapon(PowerUp powerUp, GameObject prefab)
        {
            if (slot1 == null)
            {
                slot1 = powerUp;
                slot1Prefab = prefab;
                StartCoroutine(ActiveWeaponPowerUp(1));
                return;
            }
            if (slot2 == null)
            {
                slot2 = powerUp;
                slot2Prefab = prefab;
                StartCoroutine(ActiveWeaponPowerUp(2));
            }
        }

        public void UpdateWeapon(Type powerUpType)
        {
            if (slot1 != null && slot1.GetType() == powerUpType)
            {
                slot1.lvl++;
                return;
            }
            if (slot2 != null && slot2.GetType() == powerUpType)
            {
                slot2.lvl++;
            }
        }

        private IEnumerator ActiveWeaponPowerUp(int slotId)
        {
            while (true)
            {
                float timeSpawn = 0f;
                if (slotId == 1)
                {
                    var weapon = Instantiate(slot1, global::Player.Instance.ReturnPlayerCenter(), Quaternion.identity);
                    timeSpawn = slot1.spawnTime;
                }
                else
                {
                    var weapon = Instantiate(slot2, global::Player.Instance.ReturnPlayerCenter(), Quaternion.identity);
                    timeSpawn = slot2.spawnTime;
                }

                yield return new WaitForSeconds(timeSpawn);
            }
        }
    }
}