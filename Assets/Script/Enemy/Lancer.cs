using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Lancer : EnemyBasic
{
    private void Start()
    {
        //Debug.Log("abc");
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        InitStat();
        afterImage = GetComponent<AfterImage>();
        StartCoroutine(LancerBehavior());
    }
    private void Update()
    {
        HandleStatusEffects();
    }
    private bool isCharging = false;
    private AfterImage afterImage;
    //public float chargeDuration = 0.2f;
    private IEnumerator LancerBehavior()
    {
        while (true)
        {
            if (FindTarget() != null)
            {
                yield return new WaitForSeconds(1f);
                Vector3 targetPosition = FindTarget();
                moveDirection = targetPosition - transform.position;
                yield return new WaitForSeconds(0.5f);//charge to previous 1s target
                isCharging = true;
                afterImage.Activate(true);
                //Vector3 chargePosition = transform.position;
                //normalSpeed = moveSpeed;
                rb.velocity = moveDirection.normalized * moveSpeed;
                //rotate
                if (moveDirection.x > 0)
                {
                    transform.eulerAngles = Vector3.zero;
                }
                else transform.eulerAngles = new Vector3(0, 180, 0);
                yield return new WaitForSeconds(1f);
                rb.velocity = Vector2.zero; // Stop moving
                isCharging = false;
                afterImage.Activate(false);
            }
        }
    }
}
