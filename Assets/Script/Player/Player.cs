using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //public
    public float moveSpeed = 50f;
    public float rollBoost = 50f;
    public float rollDuration;
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
        transform.position += moveInput * moveSpeed * Time.deltaTime;
        animator.SetFloat("Speed", moveInput.sqrMagnitude);

        //Roll
        if(Input.GetKeyDown(KeyCode.Space) && rollTime <= 0)
        {
            animator.SetBool("Roll", true);
            moveSpeed += rollBoost;
            rollTime = rollDuration;
            isRoll = true;
        }

        if (rollTime <= 0 && isRoll) 
        {
            animator.SetBool("Roll", false);
            moveSpeed -= rollBoost;
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
