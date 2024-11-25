using System;
using UnityEngine;

namespace Runtime.Script.Enemy
{
    public class BaseEnemy : MonoBehaviour
    {
        [SerializeField] private string monsterName;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator animator;

        #region BaseStats

        [Header("Base Stats")]
        [SerializeField] private float baseHealth;
        [SerializeField] private float baseDamage;
        [SerializeField] private float baseSpeed;
        [SerializeField] private float baseAttackCd;
        [SerializeField] private float baseAttackRange;
        [SerializeField] private float baseExpGain;
        [SerializeField] private float baseHealthGain;
        [SerializeField] private float baseManaGain;

        #endregion

        #region CurrentStats

        private float currentHealth;
        private float currentDamage;
        private float currentSpeed;
        private float currentAttackCd;
        private float currentAttackRange;
        private float currentExpGain;
        private float currentHealthGain;
        private float currentManaGain;

        #endregion
        
        private Vector3 playerCenterOffset = new Vector3(0, -5f, 0);
        private float attackTime = 0f;

        //[SerializeField] private BaseWeapon weapon;

        private void OnEnable()
        {
            Init();
        }

        public void Init()
        {
            currentHealth = baseHealth;
            currentDamage = baseDamage;
            currentSpeed = baseSpeed;
            currentAttackCd = baseAttackRange;
            currentAttackRange = baseAttackRange;
            currentExpGain = baseExpGain;
            currentHealthGain = baseHealthGain;
            currentManaGain = baseManaGain;
        }
        
        private void Update()
        {
            if (GameManager.isPaused) return;
            
            Movement();

            if (attackTime <= 0)
            {
                Attack();
            }
            else
            {
                attackTime -= Time.deltaTime;
            }
        }

        public virtual Vector3 FindTarget()
        {
            Vector3 playerPosition = PlayerManager.Instance.transform.position;
            return playerPosition + playerCenterOffset;
        }

        public virtual void Movement()
        {
            // Move direct to player
            Vector3 targetPosition = FindTarget();
            Vector3 moveDirection = targetPosition - transform.position;
            moveDirection.Normalize();
            //transform.position = Vector2.MoveTowards(transform.position, FindTaget(), moveSpeed * Time.deltaTime);
            transform.position += moveDirection * (currentSpeed * Time.deltaTime);
            animator.SetFloat("Speed", moveDirection.sqrMagnitude);
        }

        public virtual void Attack()
        {
            // Melee attack
            Vector3 targetPosition = FindTarget();
            float distance = (transform.position - targetPosition).sqrMagnitude;
            if (distance <= currentAttackRange)
            {
                // Attack
                PlayerManager.Instance.OnGetDamage(currentDamage);
                attackTime = currentAttackCd;
            }
        }

        public virtual void OnGetDamage(float damage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                EnemyDestroyed();
            }
        }

        public virtual void EnemyDestroyed()
        {
            
            Destroy(gameObject);
        }
    }
}