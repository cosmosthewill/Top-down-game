using UnityEngine;

namespace Script.Player.PowerUpScript
{
    public class Axe : global::Script.Player.PowerUpScript.PowerUp
    {
        public override float cdTime => 3f;
        public override int lvl
        {
            get => _lvl;
            set => _lvl = Mathf.Clamp(value, 0, 5);
        }
        public override float spawnTime
        {
            get => _spawnTime;
            set => _spawnTime = value;
        }

        [SerializeField] private float pushBackForce = 10f;
        [SerializeField] private float baseDmg = 20f;
        [SerializeField] private float maxDistance;
        [SerializeField] private float moveSpeed;

        private float _cdtimer;
        private int _lvl;
        private float _spawnTime;
        private bool isReturn;
        private Rigidbody2D rb;
        Vector2 ThrowDirection()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 lookDir = mousePos - global::Player.Instance.transform.position;
            return lookDir;
        }
        private void Start()
        {
            _cdtimer = cdTime;
            isReturn = false;
            rb = GetComponent<Rigidbody2D>();
        }
        private void Update() 
        {
            if (!isReturn && Vector3.Distance(transform.position, global::Player.Instance.transform.position) >= maxDistance) 
            {
                isReturn = true;
            }
            if (isReturn) 
            {
                Vector2 returnDir = (global::Player.Instance.transform.position - transform.position).normalized;
                rb.velocity = returnDir * moveSpeed;
            }
            else
            {
                rb.velocity = ThrowDirection().normalized * moveSpeed;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && isReturn)
            {
                rb.velocity = Vector2.zero;
                Destroy(gameObject);
            }
            if(collision.CompareTag("Enemy"))
            {
                float dmg = (PlayerStatsManager.Instance.damage + baseDmg) * (1 + lvl / 2);
                collision.gameObject.GetComponent<EnemyBasic>().TakeDamage((int)dmg);
                if (lvl == 5)
                {
                    Vector2 distance = collision.gameObject.GetComponent<EnemyBasic>().transform.position - transform.position;
                    collision.gameObject.GetComponent<EnemyBasic>().ApplyKnockback(distance * pushBackForce);
                }
            }
        }
    }
}
