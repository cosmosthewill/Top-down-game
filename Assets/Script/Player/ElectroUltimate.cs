using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroUltimate : UltimateBase
{
    public override float UltDuration => 8f;
    public override void Initialize(Transform playerTransform)
    {
        transform.SetParent(playerTransform);
    }
    private int dmg;
    private HashSet<Collider2D> activeCollisions = new HashSet<Collider2D>();
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlaySfx(SfxType.ElectroUltimate, true, 8f);
        dmg = (int)(PlayerStatsManager.Instance.damage * 0.7);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            StartCoroutine(Onhit(collision, dmg));
        }
    }
    private IEnumerator Onhit(Collider2D collision, float dmg)
    {
        if (activeCollisions.Contains(collision)) yield break;

        activeCollisions.Add(collision);
        float duration = 1f;//time to take dmg
        collision.gameObject.GetComponent<EnemyBasic>().TakeDamage((int)dmg);
        collision.gameObject.GetComponent<EnemyBasic>().ApplyStatus(EnemyBasic.EnemyStatus.Freeze, 0.5f);
        yield return new WaitForSeconds(duration);

        activeCollisions.Remove(collision);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
