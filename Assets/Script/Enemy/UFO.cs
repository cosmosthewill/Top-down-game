using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class UFO : EnemyBasic
{
    //shotAngle
    private float currentAngle = 0f;
    private float angleIncrement = 5f;
    private float fireRate = 0.02f;
    private AfterImage afterImage;
    private float shotTime = 7f;
    private float spriralShotcd = 5f;
    private void Start()
    {
        isBoss = true;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        InitStat();
        Vector3 directionToPlayer = Player.Instance.ReturnPlayerCenter() - transform.position;
        currentAngle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        afterImage = GetComponent<AfterImage>();
    }

    protected override void EnemyShot()
    {
        StartCoroutine(SpiralShoot());
    }
    private void Update()
    {
        shotTime += Time.deltaTime;
        //HandleStatusEffects();
        //moving
        move();
        if (shotTime >= spriralShotcd) 
        {
            EnemyShot();
            shotTime = 0f;
        }
        //Debug.Log($"Velocity: {rb.velocity.sqrMagnitude}");
        if (rb.velocity.sqrMagnitude > 0)
        {
            afterImage.Activate(true);
            //Debug.Log("Boss is moving. Activating AfterImage.");
        }
        else
        {
            //Debug.Log("Boss is idle. Deactivating AfterImage.");
            afterImage.Activate(false);
        }

    }
    
    private IEnumerator SpiralShoot()
    {
        float totalRotation = 0f;
        while(totalRotation < 360f)
        {
            Vector3 direction = new Vector3(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad), 0);
            GameObject bulletTmp = Instantiate(bullet, transform.position, Quaternion.identity);
            Bullet _bullet = bulletTmp.GetComponent<Bullet>();
            _bullet.Init(monsterDmg, false);
            Rigidbody2D rb = bulletTmp.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.AddForce(direction * bulletSpeed, ForceMode2D.Impulse);
            }
            currentAngle += angleIncrement;
            totalRotation += angleIncrement;
            if (currentAngle >= 360f) currentAngle -= 360f;
            yield return new WaitForSeconds(fireRate);
        }
    }
}
