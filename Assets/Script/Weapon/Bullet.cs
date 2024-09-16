using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public bool isPlayerBullet;

    public void Init(int bulletDamage, bool isPlayerBullet)
    {
        this.damage = bulletDamage;
        this.isPlayerBullet = isPlayerBullet;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPlayerBullet && collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyBasic>().TakeDamage(damage);
            Destroy(gameObject);
        }
        if (!isPlayerBullet && collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
    
}
