using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum ShootingMode
    {
        Normal,
        Shotgun
    }
    public ShootingMode shootingMode = ShootingMode.Normal;
    public UnityEngine.GameObject bullet;
    public Transform firePos;
    public float fireCd = 0.2f;
    public float bulletForce;
    public AudioSource shootingSound;

    private float _fireTime;


    //

    void RotateGun()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mousePos - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = rotation;

        if (transform.eulerAngles.z > 90 && transform.eulerAngles.z < 270)
        {
            transform.localScale = new Vector3(0.5f, -0.5f, 0);
        }
        else
            transform.localScale = new Vector3(0.5f, 0.5f, 0);
    }

    //FireBullet
    void FireBullet()
    {
        _fireTime = fireCd;

        UnityEngine.GameObject _bulletTmp = Instantiate(bullet, firePos.position, Quaternion.identity);
        //GameObject _bulletTmp1 = Instantiate(bullet, firePos.position, Quaternion.identity);
        //GameObject _bulletTmp2 = Instantiate(bullet, firePos.position, Quaternion.identity);

        Rigidbody2D rb = _bulletTmp.GetComponent<Rigidbody2D>();
        //Rigidbody2D rb1 = _bulletTmp1.GetComponent<Rigidbody2D>();
        //Rigidbody2D rb2 = _bulletTmp2.GetComponent<Rigidbody2D>();

        //Vector3 center = transform.right;
        rb.AddForce(transform.right * bulletForce, ForceMode2D.Impulse);
        //rb1.AddForce(new Vector3 (Mathf.Cos(20 * Mathf.Deg2Rad), Mathf.Sin(20 * Mathf.Deg2Rad),center.z) * bulletForce, ForceMode2D.Impulse);
        //rb2.AddForce(new Vector3 (Mathf.Cos(-20 * Mathf.Deg2Rad), Mathf.Sin(-20 * Mathf.Deg2Rad), center.z) * bulletForce, ForceMode2D.Impulse);
        //Debug.Log(center);
        //damage calulator
        Bullet bulletScript = _bulletTmp.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.Init(PlayerStatsManager.Instance.damage, true);
        }
    }
    void ShotGunFire()
    {
        _fireTime = fireCd;
        int bulletCount = 5;
        float spreadAngle = 30f;

        // Calculate the angle between each bullet
        float angleStep = spreadAngle / (bulletCount - 1);

        for (int i = 0; i < bulletCount; i++)
        {
            // Calculate the rotation offset
            float offsetAngle = -spreadAngle / 2f + (i * angleStep);

            // Create the bullet
            UnityEngine.GameObject _bulletTmp = Instantiate(bullet, firePos.position, Quaternion.identity);

            // Get the Rigidbody2D component
            Rigidbody2D rb = _bulletTmp.GetComponent<Rigidbody2D>();

            // Calculate the rotated direction
            Vector3 spreadDirection = Quaternion.Euler(0, 0, offsetAngle) * transform.right;

            // Apply force to the bullet
            rb.AddForce(spreadDirection * bulletForce, ForceMode2D.Impulse);

            // Initialize bullet damage
            Bullet bulletScript = _bulletTmp.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.Init(PlayerStatsManager.Instance.damage, true);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RotateGun();

        //FireGun
        _fireTime -= Time.deltaTime;
        if(Input.GetMouseButton(0) && _fireTime < 0)
        {
            shootingSound.Play();
            switch (shootingMode)
            {
                case ShootingMode.Normal:
                    FireBullet();
                    break;
                case ShootingMode.Shotgun:
                    ShotGunFire();
                    break;
            }
        }
        
    }
}
