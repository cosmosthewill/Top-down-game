using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public GameObject bullet;
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

        GameObject _bulletTmp = Instantiate(bullet, firePos.position, Quaternion.identity);

        Rigidbody2D rb = _bulletTmp.GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right * bulletForce, ForceMode2D.Impulse);

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
            FireBullet();
        }
        
    }
}
