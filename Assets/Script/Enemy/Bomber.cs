using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : EnemyBasic
{
    public GameObject explodeAni;
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SoundManager.Instance.PlaySfx(SfxType.Explode);
            Vector2 distance = transform.position - Player.Instance.ReturnPlayerCenter();
            //Debug.Log(distance * 50);
            Player.Instance.ApplyKnockback(distance * 30);
            PlayerStatsManager.Instance.TakeDmg(monsterDmg);
            currentHealth = -1;
            if (explodeAni != null)
            {
                GameObject _explode = Instantiate(explodeAni, transform.position, Quaternion.identity);
                Destroy(_explode, 0.3f);
            }
        }
    }
}
