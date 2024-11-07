using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateController : MonoBehaviour
{
    public GameObject UltPrefab;
    public float UltDuration; //sec

    private float _UltTime;
    // Start is called before the first frame update

    public void ActiveUlt()
    {
        GameObject ultimateObject = Instantiate(UltPrefab, transform.position, Quaternion.identity);
        // Check if it inherits from UltimateAbilityBase
        UltimateBase ultimateAbility = ultimateObject.GetComponent<UltimateBase>();
        if (ultimateAbility != null)
        {
            UltDuration = ultimateAbility.UltDuration;
            ultimateAbility.Initialize(transform);
        }
        StartCoroutine(UltimateDuration(ultimateObject));
    }
    private IEnumerator UltimateDuration(GameObject ultimateObject)
    {
        yield return new WaitForSeconds(UltDuration);
        Destroy(ultimateObject); // Remove the healing circle after duration
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && PlayerStatsManager.Instance.currentMana == PlayerStatsManager.Instance.maxMana) 
        {
            PlayerStatsManager.Instance.currentMana = 0;
            ActiveUlt();
        }
        //cheat
        if(Input.GetKeyDown(KeyCode.E))
        {
            PlayerStatsManager.Instance.currentMana = PlayerStatsManager.Instance.maxMana;
            PlayerStatsManager.Instance.currentHealth = PlayerStatsManager.Instance.maxHealth;
        }
    }
}
