using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowStick : PowerUp
{
    public override float cdTime => 8f;
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

    public float orbitRadius = 2f;          // Radius of the orbit around the player
    public float orbitSpeed = 20f;        // Speed of orbit rotation (degrees per second)

    private float _cdtimer;
    private Rigidbody2D rb;
    private bool isOrbiting;
    private float currentAngle = 0f;
    private float _spawnTime;
    private int _lvl;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isOrbiting = true;
        _cdtimer = cdTime;
        currentAngle = 0f;
    }

    private void Update()
    {
        if (isOrbiting) 
        {
            _cdtimer -= Time.deltaTime;
            // Calculate the angular velocity in radians
            float angularSpeed = orbitSpeed * Mathf.Deg2Rad;
            currentAngle += angularSpeed * Time.fixedDeltaTime;

            // Calculate the velocity vector based on circular motion
            Vector2 direction = new Vector2(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle));
            Vector2 orbitPosition = (Vector2)Player.Instance.transform.position + direction * orbitRadius;

            // Move
            Vector2 velocity = (orbitPosition - rb.position) / Time.fixedDeltaTime;
            rb.velocity = velocity;

        }
        if (_cdtimer <= 0)
        {
            isOrbiting = false;
            rb.velocity = Vector2.zero;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            float dmg = (PlayerStatsManager.Instance.damage + baseDmg) * (1 + lvl / 2);
            collision.gameObject.GetComponent<EnemyBasic>().TakeDamage((int)dmg);
            if (lvl == 5) //knockback
            {
                Vector2 distance = collision.gameObject.GetComponent<EnemyBasic>().transform.position - transform.position;
                collision.gameObject.GetComponent<EnemyBasic>().ApplyKnockback(distance * pushBackForce);
            }
        }
    }
}
