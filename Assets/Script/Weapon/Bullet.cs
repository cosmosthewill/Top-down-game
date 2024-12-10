using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public bool isPlayerBullet;
    [Header("StatusApply")]
    public EnemyBasic.EnemyStatus enemyStatus;
    public float duration;
    public void Init(int bulletDamage, bool isPlayerBullet)
    {
        this.damage = bulletDamage;
        this.isPlayerBullet = isPlayerBullet;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPlayerBullet && collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyBasic>().ApplyStatus(enemyStatus, duration);
            collision.gameObject.GetComponent<EnemyBasic>().TakeDamage(damage);
            Destroy(gameObject);
        }
        if (!isPlayerBullet && collision.gameObject.tag == "Player")
        {
            PlayerStatsManager.Instance.TakeDmg(damage);
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, GetComponent<CircleCollider2D>().radius);
    }
}

