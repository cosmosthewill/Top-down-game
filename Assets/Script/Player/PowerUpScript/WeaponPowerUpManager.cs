using System;
using System.Collections;
using UnityEngine;

namespace Script.Player.PowerUpScript
{
    public class WeaponPowerUpManager : MonoBehaviour
    {
        public static WeaponPowerUpManager Instance;
        private const int NUMBER_OF_SLOTS = 2;
        private PowerUp[] slots = new PowerUp[NUMBER_OF_SLOTS];

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        public int RemainSlot()
        {
            int cnt = 0;
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] == null)
                    cnt++;
            }
            return cnt;
        }

        public PowerUp GetWeaponPowerUp(Type powerUpType)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] != null && powerUpType == slots[i].GetType())
                {
                    return slots[i];
                }
            }

            return null;
        }

        public void AddWeapon(PowerUp powerUp)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] == null)
                {
                    slots[i] = powerUp;
                    slots[i].lvl = 1;
                    StartCoroutine(AddWeapon(i));
                    return;
                }
            }
        }

        private IEnumerator AddWeapon(int id)
        {
            while (true)
            {
                Instantiate(slots[id], global::Player.Instance.ReturnPlayerCenter(), Quaternion.identity);
                float timeSpawn = slots[id].spawnTime;
                yield return new WaitForSeconds(timeSpawn);
            }
        }

        public void UpdateWeapon(Type powerUpType)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] != null && powerUpType == slots[i].GetType())
                {
                    slots[i].lvl++;
                    return;
                }
            }
        }

        public PowerUp GetLowerLevelWeapon()
        {
            int minLevel = slots[0].lvl;
            int index = 0;
            for (int i = 1; i < slots.Length; i++)
            {
                if (minLevel > slots[i].lvl)
                {
                    minLevel = slots[i].lvl;
                    index = i;
                }
            }

            return slots[index];
        }
    }
}