using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DonkioteMovement : EnemyBasic
{
    protected override void move()
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
            transform.position = Vector2.MoveTowards(transform.position, FindTaget(), moveSpeed * 3 * Time.deltaTime);
            animator.SetFloat("Speed", moveDirection.sqrMagnitude);
        }
    }
}
