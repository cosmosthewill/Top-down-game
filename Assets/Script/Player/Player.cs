using System.Collections;
using System.Collections.Generic;
using Script.Player.PowerUpScript;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using static CollectibleItems;
using static UnityEditor.Progress;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //public
    bool isRoll = false;
    public Rigidbody2D rb;
    public Animator animator;
    public Vector3 moveInput;
    public SpriteRenderer characterSR;
    public static Player Instance;
    public float attractionRadius; // Radius within which items are attracted
    public float attractionSpeed;  // Speed at which items move toward the player
    public GameObject powerUpSlot1;
    public GameObject debuffAni;
    public Sprite avatar;
    public Image img;
    public Image rollImg;
    //public AfterImage afterImage;
    //private

    private float rollTime;
    private float rollCD = 3f;
    private bool isRollCd = false;
    private float _rollCD = 0f;
    private Vector3 playerCenterOffset = new Vector3(0, -5f, 0);
    protected Coroutine knockbackRoutine;
    protected bool isKnockedBack = false;
    protected float knockbackDuration = 0.2f;
    private bool isBlocked = false;

    //bounds
    private float playerWidth;
    private float playerHeight;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        img.sprite = avatar;
        playerWidth = transform.GetComponentInChildren<SpriteRenderer>().bounds.size.x / 2;
        playerHeight = transform.GetComponentInChildren<SpriteRenderer>().bounds.size.y / 2;
        attractionRadius = 20f;
        attractionSpeed = 50f;
        //animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CollectibleItems"))
        {
            CollectItem(other.gameObject);
        }
    }
    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("aa");
            isBlocked = true;
            rb.velocity = Vector2.zero;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("bb");
            isBlocked = false;
        }
    }*/
    //Collect Items
    void CollectItem(UnityEngine.GameObject item)
    {
        // Determine item type and apply effect
        ItemType itemType = item.GetComponent<CollectibleItems>().itemType;

        switch (itemType)
        {
            case ItemType.Exp:
                PlayerExpBar.instance.GainExp(item.GetComponent<CollectibleItems>().value);
                break;
            case ItemType.Health:
                PlayerStatsManager.Instance.TakeDmg(-item.GetComponent<CollectibleItems>().value);
                break;
            case ItemType.Speed:
                PlayerStatsManager.Instance.AddSpeed(item.GetComponent<CollectibleItems>().value);
                break;
            case ItemType.Mana:
                PlayerStatsManager.Instance.GainMana(item.GetComponent<CollectibleItems>().value);
                break;
            case ItemType.Coin:
                //missing
                break;

        }

        Destroy(item);
    }
    void OnDrawGizmos()
    {
        // Draw the attraction radius in the editor
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + playerCenterOffset, 15f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + playerCenterOffset, 40f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + playerCenterOffset, 20f);
    }

    public Vector3 ReturnPlayerCenter()
    {
        return transform.position + playerCenterOffset;
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
        //Debug.Log("a");
        isKnockedBack = true;
        rb.velocity = Vector2.zero;
        rb.AddForce(knockbackForce, ForceMode2D.Impulse);
        //transform.position += (Vector3) knockbackForce;
        yield return new WaitForSeconds(knockbackDuration);
        isKnockedBack = false;

    }
    private void Update()
    {
        //inside the map
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, MapBound.Instance.minBoundary.x + playerWidth, MapBound.Instance.maxBoundary.x - playerWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, MapBound.Instance.minBoundary.y + playerHeight, MapBound.Instance.maxBoundary.y - playerHeight);
        transform.position = viewPos;

        if (isKnockedBack && isBlocked) return;
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rb.velocity = moveInput * PlayerStatsManager.Instance.moveSpeed;
        //if (rb.velocity.sqrMagnitude > 0) afterImage.Activate(true);
        //else afterImage.Activate(false);
        animator.SetFloat("Speed", moveInput.sqrMagnitude);

        //Roll
        _rollCD -= Time.deltaTime;
        if(isRollCd) 
        {
            rollImg.fillAmount += 1 / rollCD * Time.deltaTime;
            if ( rollImg.fillAmount >= 1)
            {
                rollImg.fillAmount = 1;
                isRollCd = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && rollTime <= 0 && _rollCD <= 0)
        {
            isRollCd = true;
            rollImg.fillAmount = 0;
            animator.SetBool("Roll", true);
            PlayerStatsManager.Instance.moveSpeed += PlayerStatsManager.Instance.rollBoost;
            rollTime = PlayerStatsManager.Instance.rollDuration;
            isRoll = true;
        }

        if (rollTime <= 0 && isRoll)
        {
            animator.SetBool("Roll", false);
            PlayerStatsManager.Instance.moveSpeed -= PlayerStatsManager.Instance.rollBoost;
            isRoll = false;
        }
        else
        {
            rollTime -= Time.deltaTime;
        }

        //movement smooth
        if (moveInput.x != 0)
        {
            if (moveInput.x > 0)
            {
                characterSR.transform.localScale = new Vector3(1, 1, 0);
            }
            else characterSR.transform.localScale = new Vector3(-1, 1, 0);
        }

        // Find all colliders within the attraction radius
        Collider2D[] itemsInRange = Physics2D.OverlapCircleAll(transform.position + playerCenterOffset, attractionRadius);

        foreach (Collider2D item in itemsInRange)
        {
            if (item.CompareTag("CollectibleItems")) // Assuming items have the "Collectible" tag
            {
                // Move the item towards the player
                item.transform.position = Vector3.MoveTowards(item.transform.position, transform.position + playerCenterOffset, attractionSpeed * Time.deltaTime);

            }

        }
        //testing powerup
        if (Input.GetKeyDown(KeyCode.Z))
        {
            GameObject p1 = Instantiate(powerUpSlot1, transform.position, Quaternion.identity);
            PowerUp _p1 = p1.GetComponent<PowerUp>();
            _p1.lvl = 5;
        }
        if (Input.GetKeyDown(KeyCode.X))//cheat
        {
            //Time.timeScale = (1 - Time.timeScale);
            PlayerExpBar.instance.LevelUp();
        }
    }
}
