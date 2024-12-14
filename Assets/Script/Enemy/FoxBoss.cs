using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox_boss : EnemyBasic
{
    private float shotTime = 0f;
    public float waveShotCd = 4f;
    public float angleBetweenDirections = 45f;
    private AfterImage afterImage;
    private void Start()
    {
        updateMoveCd = 2f;
        isBoss = true;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        InitStat();
        afterImage = GetComponent<AfterImage>();
        seeker = GetComponent<Seeker>();
        nextWayPointDistance = 1f;
        if (isRange) InvokeRepeating("UpdatePath", 0f, 1f);
        else InvokeRepeating("UpdatePath", 0f, 1f);
    }
    private void Update()
    {
        shotTime += Time.deltaTime;
        HandleStatusEffects();
        move();
        if (shotTime >= waveShotCd)
        {
            shotTime = 0f;
            if(Vector3.Distance(transform.position, Player.Instance.ReturnPlayerCenter()) < 100f) BossShotting();
        }
        if (rb.velocity.sqrMagnitude > 0) afterImage.Activate(true);
        else afterImage.Activate(false);
    }
    public void BossShotting()
    {
        StartCoroutine(ShotAllDirection());
    }
    protected override void EnemyShot(Vector2 direction)
    {
        GameObject bulletTmp = Instantiate(bullet, transform.position, Quaternion.identity);
        Bullet _bullet = bulletTmp.GetComponent<Bullet>();
        _bullet.Init(monsterDmg, false);
        Rigidbody2D rb = bulletTmp.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.AddForce(direction * bulletSpeed, ForceMode2D.Impulse);
        }
    }

    private IEnumerator ShotAllDirection()
    {
        for (int i = 0; i < 8; i++) // 8 directions
        {
            Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * (i * angleBetweenDirections)),
                                            Mathf.Sin(Mathf.Deg2Rad * (i * angleBetweenDirections)));

            StartCoroutine(ShootInDirection(direction));
        }
        yield return null;
    }
    private IEnumerator ShootInDirection(Vector2 direction)
    {
        int bulletsPerDirection = 8;
        for (int j = 0; j < bulletsPerDirection; j++) // 8 shots per direction
        {
            EnemyShot(direction);
            yield return new WaitForSeconds(0.2f);
        }
    }

}
