using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

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
    private float statusDuration = 0f;
    public GameObject debuffAni;

    //basic
    public float maxHealth;
    public float currentHealth;
    public bool isRange; //range enemy
    public bool isBoss;
    public float moveSpeed;
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public Animator animator;
    public Vector3 moveDirection;
    private Vector3 playerCenterOffset = new Vector3(0, -5f, 0);
    private float _fireTime;
    private float normalSpeed;

    //shot
    public bool isShotable = true;
    public UnityEngine.GameObject bullet;
    public float bulletSpeed;
    public float fireCd;

    //collision
    Player _playerTmp;
    public int monsterDmg;
    private bool isKnockedBack = false;
    private float knockbackDuration = 0.2f;
    private Coroutine knockbackRoutine;
    private GameObject _debuffAni;
    //floating damage
    public UnityEngine.GameObject popupDamage;

    //drop
    [SerializeField] private int expGain;       
    [SerializeField] private int manaGain;      
    [SerializeField] private UnityEngine.GameObject itemDrop;
    [SerializeField] private float chanceDropItem;
    [SerializeField] private float baseSpd;
    protected Vector3 FindTaget()
    {
        Vector3 playerPos = FindObjectOfType<Player>().transform.position;
        if (isRange) 
        {
            // If range enemy, move to a random point around the player
            float range = Random.Range(100f, 200f);
            Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
            return playerPos + randomDirection * range;
        }
        else 
            return  playerPos + playerCenterOffset;
    }

    void EnemyShot()
    {
        var bulletTmp = Instantiate(bullet, transform.position, Quaternion.identity);

        Rigidbody2D rb = bulletTmp.GetComponent<Rigidbody2D>();
        Vector3 playerPos = FindObjectOfType<Player>().transform.position + playerCenterOffset;
        Vector3 shotDirection = playerPos - transform.position;
        rb.AddForce(shotDirection.normalized * bulletSpeed, ForceMode2D.Impulse);
    }

    //Collision

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            /*if (_playerHealth !=  null) 
            {
                _playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            }*/
            InvokeRepeating("DamagePlayer", 0, 2f);//time to take dmg here
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _playerTmp = null;
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
    public void OnDeath()
    {
        PlayerExpBar.instance.GainExp(expGain);
        PlayerStatsManager.Instance.GainMana(manaGain);
        if (itemDrop != null)
        {
            Instantiate(itemDrop, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("abc");
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        //Debug.Log("def");
        currentHealth = 100; // Mathf.Round(Mathf.Pow(4, (3.6f + Timer.Instance.minutes / 8.5f)) - 127);
        
        //if (isRange) moveSpeed = (0.35f + 0.015f * Timer.Instance.minutes);
        moveSpeed = 0f;
        //Debug.Log("abc");
        monsterDmg = (int)(2 + Timer.Instance.minutes * 0.8f);

        currentStatus = EnemyStatus.Normal;
        normalSpeed = moveSpeed;
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
        
    }

    private void HandleStatusEffects()
    {
        if (statusDuration > 0f)
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
        currentStatus = status;
        statusDuration = duration;
    }

    protected virtual void move()
    {
        if (FindTaget() != null && !isKnockedBack)
        {
            moveDirection = FindTaget() - transform.position;

            //rotate
            if (moveDirection.x > 0)
            {
                sr.flipX = false;
            }
            else sr.flipX = true;
            
            rb.velocity = moveDirection.normalized * moveSpeed;
            //animator.SetFloat("Speed", moveDirection.sqrMagnitude);
        }
    }
    public void ApplyKnockback(Vector2 knockbackForce)
    {
        

        if (knockbackRoutine != null)
        {
            StopCoroutine(knockbackRoutine);
        }
        knockbackRoutine = StartCoroutine(KnockbackCoroutine(knockbackForce));
    }

    private IEnumerator KnockbackCoroutine(Vector2 knockbackForce)
    {
        isKnockedBack = true;
        rb.velocity = Vector2.zero;
        rb.AddForce(knockbackForce, ForceMode2D.Impulse);

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