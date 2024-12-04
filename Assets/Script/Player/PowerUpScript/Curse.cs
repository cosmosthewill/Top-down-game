using System.Collections.Generic;
using UnityEngine;

namespace Script.Player.PowerUpScript
{
    public class Curse : global::Script.Player.PowerUpScript.PowerUp
    {
        public override float cdTime => 7f;
        public override int lvl
        {
            get => _lvl;
            set => _lvl = Mathf.Clamp(value, 0, 5);
        }

        private int _lvl;
        public override float spawnTime
        {
            get => _spawnTime;
            set => _spawnTime = value;
        }
        private float _spawnTime;

        [SerializeField] private float baseDmg = 20f;
        [SerializeField] private float moveSpeed = 30f;
        private int bounceCount = 0;
        private HashSet<Collider2D> hitEnemies = new HashSet<Collider2D>();

        private SpriteRenderer sr;
        private Rigidbody2D rb;
        private float _cdTime;
        private Transform target;
        private float bounceRange = 100f;
        private bool isBounce = false;
        private Vector2 firstDirection;
        private void Start()
        {
            _cdTime = cdTime;
            sr = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
            bounceCount = 0;
            firstDirection = ThrowDirection();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                isBounce = true;
                float dmg = (PlayerStatsManager.Instance.damage + baseDmg) * (1 + lvl / 5);
                collision.gameObject.GetComponent<EnemyBasic>().TakeDamage((int)dmg);
                if (lvl == 5)
                {
                    collision.gameObject.GetComponent<EnemyBasic>().ApplyStatus(EnemyBasic.EnemyStatus.Freeze, 1f);
                }
                hitEnemies.Add(collision);
                NextTarget();
            }
        }
        Vector2 ThrowDirection()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 lookDir = mousePos - global::Player.Instance.transform.position;
            return lookDir;
        }
        private void NextTarget()
        {
            bounceCount++;
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, bounceRange);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Enemy") && !hitEnemies.Contains(hit))
                {
                    target = hit.transform; // Set the next target
                    return;
                }
            }
            Destroy(gameObject);
        }

        private void Update()
        {
            _cdTime -= Time.deltaTime;
            if (_cdTime <= 0) Destroy(gameObject);
            if (!isBounce)
            {
                rb.velocity = firstDirection.normalized * moveSpeed;
                // Rotate the bullet to face the target
                float angle = Mathf.Atan2(ThrowDirection().normalized.y, ThrowDirection().normalized.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
            else if (target != null && bounceCount <= lvl)
            {
                Vector2 direction = (target.position - transform.position).normalized;
                rb.velocity = direction * moveSpeed;
                // Rotate the bullet to face the target
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
            else Destroy(gameObject);
        }
    }
}
