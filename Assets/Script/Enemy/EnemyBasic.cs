using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;

public class EnemyBasic : MonoBehaviour
{
    //status
    public enum EnemyStatus
    {
        Normal,
        Slow,
        Freeze
    }
    public EnemyStatus currentStatus = EnemyStatus.Normal;
    private EnemyStatus previousStatus = EnemyStatus.Normal;
    private float statusDuration = 0f;
    public GameObject debuffAni;

    //basic
    protected float maxHealth;
    public float currentHealth;
    public bool isRange; //range enemy
    public bool isBoss;
    public float moveSpeed;
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public Animator animator;
    public Vector3 moveDirection;
    protected float _fireTime;
    protected float normalSpeed;
    protected float updateMove = 0f;
    
    //shot
    public bool isShotable = true;
    public UnityEngine.GameObject bullet;
    public float bulletSpeed;
    public float fireCd;

    //collision
    public int monsterDmg;
    protected bool isKnockedBack = false;
    protected float knockbackDuration = 0.2f;
    protected Coroutine knockbackRoutine;
    protected GameObject _debuffAni;
    //floating damage
    public UnityEngine.GameObject popupDamage;

    //drop
    [Header("Combat Stat")]
    [SerializeField] private UnityEngine.GameObject itemDrop;
    [SerializeField] private float chanceDropItem;
    [SerializeField] private float baseSpd;
    [SerializeField] private float baseHp;
    [SerializeField] private int baseDmg;
    [SerializeField] private int baseExpGain;
    [SerializeField] private int baseManaGain;
    public float updateMoveCd = 2f;

    //exp
    private int expGain;
    private int manaGain;
    //test
    //private AfterImage afterImage;
    protected Vector3 FindTarget()
    {
        Vector3 playerPos = Player.Instance.ReturnPlayerCenter();
        if (isRange)
        {
            //if (Vector3.Distance(playerPos, transform.position) <= 50f && Vector3.Distance(playerPos, transform.position) >= 25f) return transform.position;
            // If range enemy, move to a random point around the player
            float range = Random.Range(25f, 50f);
            //Vector3 diffDirection = (transform.position - playerPos).normalized;
            Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
            return playerPos + randomDirection * range;
        }
        else
            return playerPos;
    }

    protected virtual void EnemyShot()
    {
        var bulletTmp = Instantiate(bullet, transform.position, Quaternion.identity);
        Bullet _bulletTmp = bulletTmp.GetComponent<Bullet>();
        _bulletTmp.Init(monsterDmg, false);
        Rigidbody2D rb = bulletTmp.GetComponent<Rigidbody2D>();
        Vector3 playerPos = Player.Instance.ReturnPlayerCenter();
        Vector3 shotDirection = playerPos - transform.position;
        rb.AddForce(shotDirection.normalized * bulletSpeed, ForceMode2D.Impulse);
    }
    protected virtual void EnemyShot(Vector2 direction) { }
    //Collision

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            InvokeRepeating("DamagePlayer", 0, 1.5f);//time to take dmg here
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            CancelInvoke("DamagePlayer");
        }
    }
    void DamagePlayer()
    {
        //Debug.Log("dmg");
        PlayerStatsManager.Instance.TakeDmg(monsterDmg);
    }

    public void TakeDamage(int amount)
    {
        if (Random.value < PlayerStatsManager.Instance.critChance) //critical hit
        {
            amount *= PlayerStatsManager.Instance.critMultiplier;
            currentHealth -= amount;
            UnityEngine.GameObject points = Instantiate(popupDamage, transform.position, Quaternion.identity) as UnityEngine.GameObject;
            points.transform.localScale = new Vector3(1.5f, 1.5f, 1);
            points.transform.GetChild(0).GetComponent<TextMesh>().text = amount.ToString();
            points.transform.GetChild(0).GetComponent<TextMesh>().color = Color.red;
        }
        else //non-crit
        {
            currentHealth -= amount;
            UnityEngine.GameObject points = Instantiate(popupDamage, transform.position, Quaternion.identity) as UnityEngine.GameObject;
            points.transform.GetChild(0).GetComponent<TextMesh>().text = amount.ToString();
        }
        if (currentHealth < 0) //dead
        {
            OnDeath();
        }
    }
    public virtual void OnDeath()
    {
        if (isBoss) SoundManager.Instance.PlaySfx(SfxType.BossDeath);
        PlayerExpBar.instance.GainExp(expGain);
        PlayerStatsManager.Instance.GainMana(manaGain);
        if (itemDrop != null)
        {
            Instantiate(itemDrop, transform.position, Quaternion.identity);
        }
        CoinDropManager.Instance.GenerateCoin(transform.position, isBoss);
        Destroy(gameObject);
    }
    public void InitStat()
    {
        maxHealth = baseHp * (1 + Timer.Instance.minutes * 0.8f);
        moveSpeed = baseSpd * (1 + 0.3f * Timer.Instance.minutes);
        if (isRange) moveSpeed = baseSpd * (1 + 0.15f * Timer.Instance.minutes);
        monsterDmg = (int)(baseDmg * (1 + Timer.Instance.minutes * 0.8f));

        //drop
        expGain = baseExpGain + Timer.Instance.minutes * 10;
        manaGain = baseManaGain;
        currentStatus = EnemyStatus.Normal;
        normalSpeed = moveSpeed;
        currentHealth = maxHealth;
    }
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("abc");
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        InitStat();
        if (isBoss) SoundManager.Instance.PlaySfx(SfxType.BossAppear);
    }

    // Update is called once per frame
    void Update()
    {
        HandleStatusEffects();
        //moving
        move();

        //shoting
        if (isShotable)
        {
            _fireTime -= Time.deltaTime;
            if (_fireTime < 0)
            {
                _fireTime = fireCd;
                EnemyShot();
            }
        }
        if(currentHealth < 0) OnDeath();
    }

    protected void HandleStatusEffects()
    {
        if (statusDuration > 0f && !isBoss)
        {
            statusDuration -= Time.deltaTime;

            // Apply status effects
            switch (currentStatus)
            {
                case EnemyStatus.Normal:
                    moveSpeed = normalSpeed;
                    break;
                case EnemyStatus.Slow:
                    if (_debuffAni == null) _debuffAni = Instantiate(debuffAni, transform.position, Quaternion.identity);
                    _debuffAni.transform.position = transform.position;
                    moveSpeed = normalSpeed * 0.2f;
                    break;
                case EnemyStatus.Freeze:
                    if (_debuffAni == null) _debuffAni = Instantiate(debuffAni, transform.position, Quaternion.identity);
                    _debuffAni.transform.position = transform.position;
                    sr.color = Color.blue;
                    moveSpeed = 0f;
                    break;
            }
        }
        else
        {
            // Reset to normal status
            currentStatus = EnemyStatus.Normal;
            moveSpeed = normalSpeed;
            sr.color = Color.white;
            Destroy(_debuffAni);
        }
    }

    public void ApplyStatus(EnemyStatus status, float duration)
    {
        if (status == EnemyStatus.Slow && currentStatus == EnemyStatus.Freeze) return;
        currentStatus = status;
        statusDuration = duration;
    }

    protected virtual void move()
    {
        if (isKnockedBack) return;
        updateMove += Time.deltaTime;
        if (updateMove >= updateMoveCd) updateMove = 0f;
        else if (isRange)
        {
            float distance = Vector3.Distance(Player.Instance.ReturnPlayerCenter(), transform.position);
            if (distance >= 15f && distance <= 40f) rb.velocity = Vector3.zero;
            return;
        }

        if (FindTarget() != null && !isKnockedBack)
        {
            moveDirection = FindTarget() - transform.position;

            //rotate
            if (moveDirection.x > 0)
            {
                transform.eulerAngles = Vector3.zero;
            }
            else transform.eulerAngles = new Vector3(0, 180, 0);

            rb.velocity = moveDirection.normalized * moveSpeed;
            //animator.SetFloat("Speed", moveDirection.sqrMagnitude);
        }


    }
    public void ApplyKnockback(Vector2 knockbackForce)
    {
        if (isBoss) return;
        if (knockbackRoutine != null)
        {
            StopCoroutine(knockbackRoutine);
        }
        knockbackRoutine = StartCoroutine(KnockbackCoroutine(knockbackForce));
    }

    private IEnumerator KnockbackCoroutine(Vector2 knockbackForce)
    {
        Debug.Log("abc");
        isKnockedBack = true;
        rb.velocity = Vector2.zero;
        rb.AddForce(knockbackForce, ForceMode2D.Impulse);
        //rb.velocity = knockbackForce;
        yield return new WaitForSeconds(knockbackDuration);
        isKnockedBack = false;

    }
}

//spining
//float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
//if (angle < 0) angle += 360;
//rb.rotation = angle;
//if (rb.rotation > 90 && rb.rotation < 270) transform.localScale = new Vector3(10, -10, 0);
//else transform.localScale = new Vector3(10, 10, 0);