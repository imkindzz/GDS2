using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //to change the enemy detection range for the damage, change the size of the trigger collider

    [SerializeField] private float damage; //the amount of damage that the player makes
    [SerializeField] private float damageDelay; //the time taken for the damage to be in effect

    private List<StatusBase> reachableStatus; //the enemy or boss statuses that are within the damageRadius

    private float damageTimer = 0f;

    #region Unity methods
    void Start()
    {
        reachableStatus = new List<StatusBase>();
    }

    void Update()
    {
        damageTimer += Time.deltaTime;

        if (damageTimer > damageDelay)
        {
            foreach (StatusBase status in reachableStatus) status.TakeDamage(damage);
            damageTimer = 0f;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) return;

        StatusBase status = collision.GetComponent<StatusBase>();
        if (status) reachableStatus.Add(status);

        Debug.LogWarning("reachableStatus");
        Debug.Log(reachableStatus.Count);
        foreach (StatusBase statusBase in reachableStatus) Debug.Log(statusBase.gameObject.name);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) return;

        StatusBase status = collision.GetComponent<StatusBase>();
        if (status) reachableStatus.Remove(status);

        Debug.LogWarning("reachableStatus");
        Debug.Log(reachableStatus.Count);
        foreach (StatusBase statusBase in reachableStatus) Debug.Log(statusBase.gameObject.name);
    }
    #endregion
}
