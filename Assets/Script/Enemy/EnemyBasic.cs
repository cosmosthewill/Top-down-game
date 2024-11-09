using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyBasic : MonoBehaviour
{
    //basic
    public float maxHealth;
    public float currentHealth;
    public bool isRange; //range enemy
    public float moveSpeed;
    public Rigidbody2D rb;
    public Animator animator;
    public Vector3 moveDirection;
    private Vector3 playerCenterOffset = new Vector3(0, -5f, 0);
    private float _fireTime;

    //shot
    public bool isShotable = true;
    public GameObject bullet;
    public float bulletSpeed;
    public float fireCd;

    //collision
    Player _playerTmp;
    public int monsterDmg;
    //floating damage
    public GameObject popupDamage;

    //drop
    [SerializeField] private int expGain;       
    [SerializeField] private int manaGain;      
    [SerializeField] private GameObject itemDrop;
    [SerializeField] private float chanceDropItem;
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
            GameObject points = Instantiate(popupDamage, transform.position, Quaternion.identity) as GameObject;
            points.transform.localScale = new Vector3(1.5f, 1.5f, 1);
            points.transform.GetChild(0).GetComponent<TextMesh>().text = amount.ToString();
            points.transform.GetChild(0).GetComponent<TextMesh>().color = Color.red;
        }
        else //non-crit
        {
            currentHealth -= amount;
            GameObject points = Instantiate(popupDamage, transform.position, Quaternion.identity) as GameObject;
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
        //Debug.Log("def");
        currentHealth = Mathf.Round(Mathf.Pow(4, (3.6f + Timer.Instance.minutes / 8.5f)) - 127);
        //Debug.Log("defghi");
        moveSpeed = (0.35f + 0.015f * Timer.Instance.minutes);
        //Debug.Log("abc");
        monsterDmg = (int)(2 + Timer.Instance.minutes * 0.8f);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("abc");
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
    protected virtual void move()
    {
        if (FindTaget() != null)
        {
            moveDirection = FindTaget() - transform.position;
            //spining
            //float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            //if (angle < 0) angle += 360;
            //rb.rotation = angle;
            //if (rb.rotation > 90 && rb.rotation < 270) transform.localScale = new Vector3(10, -10, 0);
            //else transform.localScale = new Vector3(10, 10, 0);
            transform.position = Vector2.MoveTowards(transform.position, FindTaget(), moveSpeed * Time.deltaTime);
            animator.SetFloat("Speed", moveDirection.sqrMagnitude);
        }
    }
}
