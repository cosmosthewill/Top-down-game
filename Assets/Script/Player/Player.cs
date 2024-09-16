using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //public
    bool isRoll = false;
    public Rigidbody rb;
    public Animator  animator;
    public Vector3 moveInput;
    public SpriteRenderer characterSR;
    //private
    
    private float rollTime;
    private void Start()
    {
        //animator = GetComponent<Animator>();
    }
    private void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        transform.position += moveInput * PlayerStatsManager.Instance.moveSpeed * Time.deltaTime;
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
        
    }
}
