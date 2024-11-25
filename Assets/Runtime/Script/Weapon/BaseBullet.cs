using System;
using Runtime.Script.Enemy;
using Runtime.Script.Pool;
using UnityEngine;

namespace Runtime.Script.Weapon
{
    public class BaseBullet : MonoBehaviour, IPoolObject
    {
        private float damage;
        private float lastedTime;
        private bool isPlayerBullet;

        private Vector3 currentDirection;

        public void Init(Vector3 direction, float damage, bool isPlayerBullet)
        {
            this.damage = damage;
            this.isPlayerBullet = isPlayerBullet;
            lastedTime = 2.5f;
            currentDirection = direction;
        }

        private void Update()
        {
            if (GameManager.isPaused) return; 
            
            lastedTime -= Time.deltaTime;
            if (lastedTime <= 0)
            {
                ReturnToPool();
            }
            
            transform.position += currentDirection * Time.deltaTime;
        }

        private void ReturnToPool()
        {
            BulletPool.Instance.ReturnToPool(this);
            gameObject.SetActive(false);
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (isPlayerBullet && collision.gameObject.tag == "Enemy")
            {
                collision.gameObject.GetComponent<BaseEnemy>().OnGetDamage(damage);
                ReturnToPool();
            }
            if (!isPlayerBullet && collision.gameObject.tag == "Player")
            {
                //PlayerStatsManager.Instance.TakeDmg(damage);
                ReturnToPool();
            }
        }
    }
}