using System;
using Runtime.Script.Character;
using Runtime.Script.Weapon;
using UnityEngine;

namespace Runtime.Script
{
    public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
    {
        [SerializeField] private PlayerStatManager playerStatManager;

        private BaseCharacter character;
        private BaseWeapon weapon;

        private float fireTime = 0f;
        private Vector3 moveDirection;

        private void OnEnable()
        {
            LoadCharacter();
        }

        private void LoadCharacter()
        {
            GameObject go = Instantiate(CharacterManifest.Instance.GetCharacters("Soldier"), transform);
            character = go.GetComponent<BaseCharacter>();
            character.transform.localPosition = Vector3.zero;
            weapon = character.Weapon;
            
            playerStatManager.LoadStats(character, weapon);
        }

        private void Update()
        {
            if (GameManager.isPaused) return;
            
            weapon.RotateGun();
            
            fireTime -= Time.deltaTime;
            if(Input.GetMouseButton(0) && fireTime <= 0)
            {
                weapon.FireBullet(playerStatManager.CurrentDamage, playerStatManager.CurrentBulletSpeed);
                fireTime = playerStatManager.CurrentFireCd;
            }

            moveDirection = Vector3.zero;
            
            if (Input.GetKey(KeyCode.W))
            {
                moveDirection.y = 1;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                moveDirection.y = -1;
            }
            
            if (Input.GetKey(KeyCode.A))
            {
                moveDirection.x = -1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                moveDirection.x = 1;
            }
            
            moveDirection.Normalize();
            transform.position += moveDirection * (playerStatManager.CurrentSpeed * Time.deltaTime);
            
            if (moveDirection.x != 0)
            {
                character.FlipX(moveDirection.x < 0);
            }
        }

        public void OnGetDamage(float damage)
        {
            playerStatManager.ChangeHealth(-damage);
        }
    }
}