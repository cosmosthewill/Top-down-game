using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox_boss : EnemyBasic
{
    private float shotTime = 0f;
    public float waveShotCd = 4f;
    public float angleBetweenDirections = 45f;
    public AfterImage afterImage;
    private int cnt = 0;
    Coroutine ShottingC;
    private void Update()
    {
        MovementDetect();
        shotTime += Time.deltaTime;
        HandleStatusEffects();
        if (shotTime >= waveShotCd)
        {
            shotTime = 0f;
            if(Vector3.Distance(transform.position, Player.Instance.ReturnPlayerCenter()) < 100f) BossShotting();
        }
        if(cnt == 8)
        {
            canMove = true;
            cnt = 0;
        }
        if (isMoving) afterImage.Activate(true);
        else afterImage.Activate(false);
        if (currentHealth <= 0) OnDeath();
    }
    public void BossShotting()
    {
        //Debug.LogWarning(canMove);
        if (ShottingC != null) StopCoroutine(ShotAllDirection());
        ShottingC = StartCoroutine(ShotAllDirection());
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
        canMove = false;
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
        ++cnt;
    }

}
