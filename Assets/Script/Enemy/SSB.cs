using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSB : EnemyBasic
{
    public float explosiveRange = 15f;
    public GameObject explodeAni;
    private Coroutine isStartCountDown;
    public Transform outerRadius; // Outer radius visual
    public Transform innerRadius; // Inner radius visual
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        InitStat();
        if (isBoss) SoundManager.Instance.PlaySfx(SfxType.BossAppear);
        //
        seeker = GetComponent<Seeker>();
        nextWayPointDistance = 1f;
        InvokeRepeating("UpdatePath", 0f, 0.2f);
        outerRadius.localScale = Vector3.zero;
        innerRadius.localScale = Vector3.zero;
    }
    private void Update()
    {
        HandleStatusEffects();
        if (Vector3.Distance(transform.position, Player.Instance.ReturnPlayerCenter()) <= explosiveRange)
        {
            if (isStartCountDown == null)
            {
                outerRadius.localScale = Vector3.one;
                innerRadius.localScale = Vector3.zero;
                isStartCountDown = StartCoroutine(StartCountdown());
                rb.velocity = Vector3.zero;
                canMove = false;
            }
        }
        if (!deadByPlayer) //self death
        {
            if (Vector3.Distance(transform.position, Player.Instance.ReturnPlayerCenter()) <= explosiveRange)
                PlayerStatsManager.Instance.TakeDmg(monsterDmg);
            OnDeath();
        }
        if (currentHealth <= 0)
        {
            OnDeath();
        }
    }
    private IEnumerator StartCountdown()
    {
        float timeRemaining = 3f;
        while (timeRemaining > 0) 
        {
            timeRemaining -= Time.deltaTime;
            float elapsedTime = 3f - timeRemaining;

            float scaleProgress = Mathf.Lerp(0, 1, elapsedTime / 3f);
            innerRadius.localScale = new Vector3(scaleProgress, scaleProgress, 1);

            if (Mathf.FloorToInt(timeRemaining + Time.deltaTime) != Mathf.FloorToInt(timeRemaining))
            {
                SoundManager.Instance.PlaySfx(SfxType.ExplodeAlert);
                GameObject countdownText = Instantiate(popupDamage, transform.position + new Vector3(0, 3f, 0), Quaternion.identity);
                countdownText.transform.localScale = new Vector3(3f, 3f, 1);
                countdownText.transform.GetChild(0).GetComponent<TextMesh>().text = Mathf.Ceil(timeRemaining).ToString();
                countdownText.transform.GetChild(0).GetComponent<TextMesh>().color = Color.yellow;
            }
            yield return null;
        }
        currentHealth = -1;
        deadByPlayer = false;
        if (explodeAni != null)
        {
            SoundManager.Instance.PlaySfx(SfxType.Explode);
            GameObject _explode = Instantiate(explodeAni, transform.position, Quaternion.identity);
            Destroy(_explode, 0.3f);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, explosiveRange);
    }
}
