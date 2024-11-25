using System;
using Runtime.Script.Weapon;
using UnityEngine;

namespace Runtime.Script.Character
{
    public class BaseCharacter : MonoBehaviour
    {
        [SerializeField] private string characterName;
        [SerializeField] private SpriteRenderer spriteRenderer;

        [Header("Base Stats")]
        [SerializeField] private float baseHealth;
        [SerializeField] private float baseMana;
        [SerializeField] private float baseSpeed;

        [SerializeField] private BaseWeapon weapon;
        
        public float BaseHealth => baseHealth;
        public float BaseMana => baseMana;
        public float BaseSpeed => baseSpeed;
        public BaseWeapon Weapon => weapon;

        public void FlipX(bool flipped)
        {
            spriteRenderer.flipX = flipped;
        }
        
        public virtual void PlayUltimate()
        {
            Debug.Log("Player plays ultimate");
            // Custom for each soldier
        }
    }
}