using System.Collections;
using System.Collections.Generic;
using Script.Player.PowerUpScript;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using static CollectibleItems;
using static UnityEditor.Progress;

public class Player : MonoBehaviour
{
    //public
    bool isRoll = false;
    public Rigidbody2D rb;
    public Animator  animator;
    public Vector3 moveInput;
    public SpriteRenderer characterSR;
    public static Player Instance;
    public float attractionRadius = 2500f; // Radius within which items are attracted
    public float attractionSpeed = 250f;  // Speed at which items move toward the player
    public GameObject powerUpSlot1;
    public GameObject debuffAni;

    //private

    private float rollTime;
    private Vector3 playerCenterOffset = new Vector3(0, -5f, 0);

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
        Gizmos.DrawWireSphere(transform.position + playerCenterOffset, attractionRadius);
    }

    public Vector3  ReturnPlayerCenter()
    {
        return transform.position - playerCenterOffset;
    }
    private void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rb.velocity = moveInput * PlayerStatsManager.Instance.moveSpeed;
        animator.SetFloat("Speed", moveInput.sqrMagnitude);

        //Roll
        if(Input.GetKeyDown(KeyCode.Space) && rollTime <= 0)
        {
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
            else characterSR.transform.localScale = new Vector3 (-1, 1, 0);
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
        if(Input.GetKeyDown(KeyCode.Z)) 
        {
            GameObject p1 = Instantiate(powerUpSlot1, transform.position, Quaternion.identity);
            PowerUp _p1 = p1.GetComponent<PowerUp>();
            _p1.lvl = 5;
        }
        if(Input.GetKeyDown(KeyCode.X))
        {
            Time.timeScale = (1 - Time.timeScale);
        }
    }
}
