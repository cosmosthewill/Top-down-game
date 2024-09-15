using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public bool isPlayerBullet;

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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
