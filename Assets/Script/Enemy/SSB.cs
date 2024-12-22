using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSB : EnemyBasic
{
    public float explosiveRange = 15f;
    public GameObject explodeAni;
    private Coroutine isStartCountDown;
    private void Update()
    {
        HandleStatusEffects();
        if (Vector3.Distance(transform.position, Player.Instance.ReturnPlayerCenter()) <= explosiveRange)
        {
            if (isStartCountDown == null)
            {
                isStartCountDown = StartCoroutine(StartCountdown());
                rb.velocity = Vector3.zero;
                canMove = false;
            }
        }
        if (!deadByPlayer)
        {
            if (Vector3.Distance(transform.position, Player.Instance.ReturnPlayerCenter()) <= explosiveRange * 2)
                PlayerStatsManager.Instance.TakeDmg(monsterDmg);
            Destroy(gameObject);
        }
        else if (currentHealth <= 0)
        {
            OnDeath();
        }
    }
    private IEnumerator StartCountdown()
    {
        float timeRemaining = 3f;
        while (timeRemaining > 0) 
        {
            GameObject countdownText = Instantiate(popupDamage, transform.position + new Vector3(0, 3f, 0), Quaternion.identity);
            countdownText.transform.localScale = new Vector3(3f, 3f, 1);
            countdownText.transform.GetChild(0).GetComponent<TextMesh>().text = Mathf.Ceil(timeRemaining).ToString();
            countdownText.transform.GetChild(0).GetComponent<TextMesh>().color = Color.yellow;
            timeRemaining -= 1f;
            SoundManager.Instance.PlaySfx(SfxType.ExplodeAlert);
            yield return new WaitForSeconds(1f);
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
}
