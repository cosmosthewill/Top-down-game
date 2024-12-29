using Pathfinding;
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
    //summon
    [SerializeField] private GameObject ailen;
    private float summonTime = 8f;
    private float _summonTime = 0f;
    private bool _isShotting = false;
    private bool isDead = false;
    private void Start()
    {
        isBoss = true;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        InitStat();
        Vector3 directionToPlayer = Player.Instance.ReturnPlayerCenter() - transform.position;
        currentAngle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        afterImage = GetComponent<AfterImage>();
        if (isBoss) SoundManager.Instance.PlaySfx(SfxType.BossAppear);
        //
        seeker = GetComponent<Seeker>();
        nextWayPointDistance = 1f;
        InvokeRepeating("UpdatePath", 0f, 0.2f);
        //seeker = GetComponent<Seeker>();
        //nextWayPointDistance = 1f;
        //if (isRange) InvokeRepeating("UpdatePath", 0f, 1f);
        //else InvokeRepeating("UpdatePath", 0f, 1f);
    }

    protected override void EnemyShot()
    {
        StartCoroutine(SpiralShoot());
    }
    private void Update()
    {
        shotTime += Time.deltaTime;
        _summonTime += Time.deltaTime;
        HandleStatusEffects();
        MovementDetect();
        //moving
        //move();
        if (shotTime >= spriralShotcd)
        {
            shotTime = 0f;
            if (Vector3.Distance(transform.position, Player.Instance.ReturnPlayerCenter()) < 100f) EnemyShot();
        }
        if (isMoving)
        {
            afterImage.Activate(true);
        }
        else
        {
            afterImage.Activate(false);
        }
        if (_summonTime >= summonTime)
        {
            Summon(transform.position);
            _summonTime = 0f;
        }
        if (currentHealth <= 0 && !isDead) OnDeath();
    }
    
    private IEnumerator SpiralShoot()
    {
        canMove = false;
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
        canMove = true;
    }
    private void Summon(Vector3 positon)
    {
        GameObject _ailen = Instantiate(ailen, positon, Quaternion.identity);
        Rigidbody2D rb = _ailen.GetComponent<Rigidbody2D>();
        EnemyBasic enemy = _ailen.GetComponent<EnemyBasic>();
        Vector2 distance = Player.Instance.ReturnPlayerCenter() - _ailen.transform.position;
        enemy.ApplyKnockback(distance.normalized * 90);
        //Debug.Log(distance * 700);
    }
    public override void OnDeath()
    {
        isDead = true;
        for (int i = 0; i < 4; i++)
        {
            Vector3 randomPos = transform.position + new Vector3(Random.Range(0, 4), Random.Range(0, 4), 0);
            Summon(randomPos);
        }
        base.OnDeath();    
    }
}
