using UnityEngine;

namespace Script.Player.PowerUpScript
{
    public class Bomb : PowerUp
    {
        public override float cdTime => 1f;
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
        public GameObject explodeAni;
        [SerializeField] private float explosionForce = 10f;
        [SerializeField] private float explosionRadius = 5f;
        [SerializeField] private float baseDmg = 20f;

        private float timer;
        private bool isExploded = false;
        private int _lvl;
        private float _spawnTime;
        void Explode()
        {
            var hitcolliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
            foreach (var hit in hitcolliders) 
            {
                EnemyBasic enemy = hit.GetComponent<EnemyBasic>();
                if (enemy != null) 
                {
                    float dmg = (PlayerStatsManager.Instance.damage + baseDmg) * (1 + lvl / 5);
                    enemy.TakeDamage((int)dmg);
                    if (lvl == 5)//knockBack
                    {
                        Vector2 distance = (enemy.transform.position - transform.position);
                        if (distance.magnitude > 0) 
                        {
                            float pushBackForce = explosionForce / distance.magnitude;
                            enemy.ApplyKnockback(pushBackForce * distance);
                            Debug.Log(distance);
                            Debug.Log(pushBackForce);
                        }
                    }
                }
            
            }
        
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
        private void Start()
        {
            timer = cdTime;

        }
        private void Update()
        {
            explosionRadius = lvl * 2;
            timer -= Time.deltaTime;
            if (timer <= 0f && !isExploded)
            {
                isExploded = true;
                Destroy(gameObject, 0.3f);
            }
        }

        private void OnDestroy()
        {
            Explode();
            if (explodeAni != null)
            {
                GameObject _explode = Instantiate(explodeAni, transform.position, Quaternion.identity);
                Destroy(_explode, 0.3f);
            }
        }
    }
}
