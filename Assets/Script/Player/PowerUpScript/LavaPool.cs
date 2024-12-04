using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Player.PowerUpScript
{
    public class LavaPool : PowerUp
    {
        public override float cdTime => 5f;
        public override int lvl
        {
            get => _lvl;
            set => _lvl = Mathf.Clamp(value, 0, 5);
        }
        public override float spawnTime
        {
            get => 7.5f;
            set => _spawnTime = value;
        }

        public Animator animator;
        private HashSet<Collider2D> activeCollisions = new HashSet<Collider2D>();

        [SerializeField] private float baseDmg = 20f;


        private float _cdtimer;
        private Rigidbody2D rb;
        private bool isOrbiting;
        private float currentAngle = 0f;
        private float _spawnTime;
        private int _lvl;

        private void Start()
        {
            _cdtimer = cdTime;
            rb = GetComponent<Rigidbody2D>();
            animator.SetBool("Isloop", true);
        }

        private void Update()
        {
            _cdtimer -= Time.deltaTime;
            if (_cdtimer <= 0) 
            {
                animator.SetBool("Isend", true);
                Destroy(gameObject, 0.15f);
            }
        }

        private IEnumerator Onhit(Collider2D collision, float dmg)
        {
            if (activeCollisions.Contains(collision)) yield break;

            activeCollisions.Add(collision);
            float duration = 1.5f;//time to take dmg
            collision.gameObject.GetComponent<EnemyBasic>().TakeDamage((int)dmg);
            if (lvl == 5) //slow
            {
                collision.gameObject.GetComponent<EnemyBasic>().ApplyStatus(EnemyBasic.EnemyStatus.Slow, duration);
            }
            yield return new WaitForSeconds(duration);

            activeCollisions.Remove(collision);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                Debug.Log("a");
                float dmg = (PlayerStatsManager.Instance.damage + baseDmg) * (1 + lvl / 5);
                StartCoroutine(Onhit(collision, dmg));
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                //CancelInvoke("Onhit");
            }
        }
    }
}
